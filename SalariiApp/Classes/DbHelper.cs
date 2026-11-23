using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SalariiApp.Classes
{
    public class TaxParams
    {
        public decimal CasProcent { get; set; }
        public decimal CassProcent { get; set; }
        public decimal ImpozitProcent { get; set; }
        public string Parola { get; set; }
    }

    public class AngajatModel
    {
        public int NrCrt { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Functie { get; set; }
        public int SalarBaza { get; set; }
        public decimal SporProcent { get; set; }
        public int PremiiBrute { get; set; }
        public int Retineri { get; set; }
        public int TotalBrut { get; set; }
        public int Cas { get; set; }
        public int Cass { get; set; }
        public int BrutImpozabil { get; set; }
        public int Impozit { get; set; }
        public int ViratCard { get; set; }
        public byte[] Poza { get; set; }
    }

    public class TotaluriModel
    {
        public long TotalBrut { get; set; }
        public long TotalVirat { get; set; }
    }

    public static class DbHelper
    {
        private static readonly string ConnectionString = GetConnectionString();

        private static string GetConnectionString()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["OracleConnection"];
            if (setting != null && !string.IsNullOrWhiteSpace(setting.ConnectionString))
            {
                return setting.ConnectionString;
            }

            throw new ConfigurationErrorsException(
                "Configureaza conexiunea OracleConnection in Oracle.local.config.");
        }

        private static IDbConnection CreateOracleConnection()
        {
            Type factoryType = Type.GetType(
                "Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess", false);
            if (factoryType == null)
            {
                throw new InvalidOperationException(
                    "Lipseste driverul Oracle.ManagedDataAccess. Restaureaza pachetele NuGet ale solutiei si porneste Oracle XE.");
            }

            FieldInfo instanceField = factoryType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            DbProviderFactory factory = instanceField == null
                ? null
                : instanceField.GetValue(null) as DbProviderFactory;
            if (factory == null)
            {
                throw new InvalidOperationException("Driverul Oracle nu a putut fi initializat.");
            }

            IDbConnection connection = factory.CreateConnection();
            if (connection == null)
            {
                throw new InvalidOperationException("Driverul Oracle nu a creat conexiunea la baza de date.");
            }

            try
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                connection.Dispose();
                throw new InvalidOperationException(
                    "Conectarea la Oracle a esuat. Verifica serviciul XEPDB1 si datele din Oracle.local.config.", ex);
            }
        }

        private static void SetBindByName(IDbCommand command)
        {
            PropertyInfo bindByName = command.GetType().GetProperty("BindByName");
            if (bindByName != null && bindByName.CanWrite)
            {
                bindByName.SetValue(command, true, null);
            }
        }

        private static void AddParameter(IDbCommand command, string name, object value, DbType type, int size)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value ?? DBNull.Value;
            if (size > 0)
            {
                parameter.Size = size;
            }

            if (type == DbType.Binary)
            {
                PropertyInfo oracleDbType = parameter.GetType().GetProperty("OracleDbType");
                if (oracleDbType != null && oracleDbType.CanWrite)
                {
                    object blobType = Enum.Parse(oracleDbType.PropertyType, "Blob", true);
                    oracleDbType.SetValue(parameter, blobType, null);
                }
            }

            command.Parameters.Add(parameter);
        }

        private static void AddParameter(IDbCommand command, string name, object value, DbType type)
        {
            AddParameter(command, name, value, type, 0);
        }

        private static object DbValue(object value)
        {
            return value == null ? DBNull.Value : value;
        }

        private static byte[] ReadPhoto(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            byte[] bytes = value as byte[];
            if (bytes != null)
            {
                return bytes;
            }

            PropertyInfo valueProperty = value.GetType().GetProperty("Value");
            if (valueProperty != null)
            {
                bytes = valueProperty.GetValue(value, null) as byte[];
                if (bytes != null)
                {
                    return bytes;
                }
            }

            Stream stream = value as Stream;
            if (stream != null)
            {
                using (MemoryStream buffer = new MemoryStream())
                {
                    stream.CopyTo(buffer);
                    return buffer.ToArray();
                }
            }

            throw new InvalidOperationException("Imaginea angajatului nu a putut fi citita din Oracle.");
        }

        public static decimal ParseDecimal(string value)
        {
            string normalized = (value ?? string.Empty).Trim().Replace(',', '.');
            decimal result;
            if (!decimal.TryParse(normalized, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out result))
            {
                throw new FormatException("Introduceti un numar valid.");
            }

            return result;
        }

        private static void ValidateTaxRate(decimal value, string fieldName)
        {
            if (value < 0 || value > 100 || decimal.Round(value, 2) != value)
            {
                throw new ArgumentOutOfRangeException(fieldName, "Procentul trebuie sa fie intre 0 si 100, cu cel mult doua zecimale.");
            }
        }

        private static void ValidateAngajat(string nume, string prenume, string functie,
            int salarBaza, decimal spor, int premii, int retineri, byte[] poza)
        {
            if (string.IsNullOrWhiteSpace(nume) || nume.Trim().Length > 50)
                throw new ArgumentException("Numele este obligatoriu si trebuie sa aiba cel mult 50 de caractere.");
            if (string.IsNullOrWhiteSpace(prenume) || prenume.Trim().Length > 50)
                throw new ArgumentException("Prenumele este obligatoriu si trebuie sa aiba cel mult 50 de caractere.");
            if (string.IsNullOrWhiteSpace(functie) || functie.Trim().Length > 50)
                throw new ArgumentException("Functia este obligatorie si trebuie sa aiba cel mult 50 de caractere.");
            if (salarBaza < 1 || salarBaza > 2000000)
                throw new ArgumentOutOfRangeException("salarBaza", "Salariul de baza trebuie sa fie intre 1 si 2.000.000 RON.");
            if (spor < 0 || spor > 1000 || decimal.Round(spor, 2) != spor)
                throw new ArgumentOutOfRangeException("spor", "Sporul trebuie sa fie intre 0 si 1000%, cu cel mult doua zecimale.");
            if (premii < 0 || premii > 1000000)
                throw new ArgumentOutOfRangeException("premii", "Premiile trebuie sa fie intre 0 si 1.000.000 RON.");
            if (retineri < 0 || retineri > 1000000)
                throw new ArgumentOutOfRangeException("retineri", "Retinerile trebuie sa fie intre 0 si 1.000.000 RON.");
            if (poza != null && poza.Length > 5 * 1024 * 1024)
                throw new ArgumentOutOfRangeException("poza", "Imaginea trebuie sa aiba cel mult 5 MB.");
        }

        public static void CalculFormule(int salarBaza, decimal sporProc, int premiiBrute, int retineri,
            decimal casProc, decimal cassProc, decimal impProc,
            out int totalBrut, out int cas, out int cass, out int brutImpozabil, out int impozit, out int viratCard)
        {
            if (salarBaza < 1 || premiiBrute < 0 || retineri < 0 || sporProc < 0)
            {
                throw new ArgumentOutOfRangeException("salariu", "Valorile salariale nu pot fi negative, iar salariul de baza trebuie sa fie pozitiv.");
            }

            ValidateTaxRate(casProc, "casProc");
            ValidateTaxRate(cassProc, "cassProc");
            ValidateTaxRate(impProc, "impProc");

            decimal brutDec = salarBaza * (1m + sporProc / 100m) + premiiBrute;
            totalBrut = (int)Math.Round(brutDec, MidpointRounding.AwayFromZero);
            cas = (int)Math.Round(totalBrut * casProc / 100m, MidpointRounding.AwayFromZero);
            cass = (int)Math.Round(totalBrut * cassProc / 100m, MidpointRounding.AwayFromZero);
            brutImpozabil = totalBrut - cas - cass;
            impozit = (int)Math.Round(brutImpozabil * impProc / 100m, MidpointRounding.AwayFromZero);
            viratCard = totalBrut - impozit - cas - cass - retineri;
        }

        public static TaxParams GetParametriTaxe()
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT CAS_PROCENT, CASS_PROCENT, IMPOZIT_PROCENT, PAROLA FROM PARAMETRI_TAXE WHERE ID = 1";
                SetBindByName(command);
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Nu exista procente configurate. Ruleaza scriptul de creare a bazei de date.");
                    }

                    TaxParams result = new TaxParams();
                    result.CasProcent = Convert.ToDecimal(reader["CAS_PROCENT"], CultureInfo.InvariantCulture);
                    result.CassProcent = Convert.ToDecimal(reader["CASS_PROCENT"], CultureInfo.InvariantCulture);
                    result.ImpozitProcent = Convert.ToDecimal(reader["IMPOZIT_PROCENT"], CultureInfo.InvariantCulture);
                    result.Parola = Convert.ToString(reader["PAROLA"], CultureInfo.InvariantCulture);
                    return result;
                }
            }
        }

        public static bool UpdateParametriTaxe(decimal cas, decimal cass, decimal imp)
        {
            ValidateTaxRate(cas, "cas");
            ValidateTaxRate(cass, "cass");
            ValidateTaxRate(imp, "imp");

            using (IDbConnection connection = CreateOracleConnection())
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "UPDATE PARAMETRI_TAXE SET CAS_PROCENT = :cas, CASS_PROCENT = :cass, IMPOZIT_PROCENT = :imp WHERE ID = 1";
                    SetBindByName(command);
                    AddParameter(command, "cas", cas, DbType.Decimal);
                    AddParameter(command, "cass", cass, DbType.Decimal);
                    AddParameter(command, "imp", imp, DbType.Decimal);
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException("Procentele de taxe nu au putut fi actualizate.");
                    }
                }

                ExecuteRecalculation(connection, transaction);
                transaction.Commit();
            }

            return true;
        }

        public static string HashPassword(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder result = new StringBuilder(bytes.Length * 2);
                foreach (byte value in bytes)
                {
                    result.Append(value.ToString("x2", CultureInfo.InvariantCulture));
                }

                return result.ToString();
            }
        }

        private static bool ConstantTimeEquals(string left, string right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            int difference = left.Length ^ right.Length;
            int length = Math.Max(left.Length, right.Length);
            for (int i = 0; i < length; i++)
            {
                char leftChar = i < left.Length ? left[i] : (char)0;
                char rightChar = i < right.Length ? right[i] : (char)0;
                difference |= leftChar ^ rightChar;
            }

            return difference == 0;
        }

        public static bool VerificaParola(string parolaIntrodusa)
        {
            if (string.IsNullOrWhiteSpace(parolaIntrodusa))
            {
                return false;
            }

            TaxParams parameters = GetParametriTaxe();
            return ConstantTimeEquals(parameters.Parola, HashPassword(parolaIntrodusa));
        }

        public static bool UpdateParola(string nouaParola)
        {
            if (string.IsNullOrWhiteSpace(nouaParola) || nouaParola.Length > 100)
            {
                throw new ArgumentException("Parola trebuie sa aiba intre 1 si 100 de caractere.");
            }

            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE PARAMETRI_TAXE SET PAROLA = :parola WHERE ID = 1";
                SetBindByName(command);
                AddParameter(command, "parola", HashPassword(nouaParola), DbType.String, 64);
                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException("Parola nu a putut fi actualizata.");
                }
            }

            return true;
        }

        private static DataTable QueryAngajati(string filtruNume, string filtruPrenume, string filtruGeneral)
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                StringBuilder sql = new StringBuilder(
                    "SELECT NR_CRT, NUME, PRENUME, FUNCTIE, SALAR_BAZA, SPOR_PROCENT, PREMII_BRUTE, RETINERI, TOTAL_BRUT, CAS, CASS, BRUT_IMPOZABIL, IMPOZIT, VIRAT_CARD FROM ANGAJATI");
                bool hasCondition = false;

                if (filtruNume != null)
                {
                    sql.Append(" WHERE LOWER(NUME) LIKE :nume");
                    string namePattern = "%" + filtruNume.Trim().ToLowerInvariant() + "%";
                    AddParameter(command, "nume", namePattern, DbType.String, namePattern.Length);
                    hasCondition = true;
                }

                if (filtruPrenume != null)
                {
                    sql.Append(hasCondition ? " AND" : " WHERE");
                    sql.Append(" LOWER(PRENUME) LIKE :prenume");
                    string firstNamePattern = "%" + filtruPrenume.Trim().ToLowerInvariant() + "%";
                    AddParameter(command, "prenume", firstNamePattern, DbType.String, firstNamePattern.Length);
                    hasCondition = true;
                }

                if (!string.IsNullOrWhiteSpace(filtruGeneral))
                {
                    sql.Append(hasCondition ? " AND" : " WHERE");
                    sql.Append(" (LOWER(NUME) LIKE :filtru OR LOWER(PRENUME) LIKE :filtru OR LOWER(FUNCTIE) LIKE :filtru)");
                    string generalPattern = "%" + filtruGeneral.Trim().ToLowerInvariant() + "%";
                    AddParameter(command, "filtru", generalPattern, DbType.String, generalPattern.Length);
                }

                sql.Append(" ORDER BY NR_CRT");
                command.CommandText = sql.ToString();
                SetBindByName(command);

                using (IDataReader reader = command.ExecuteReader())
                {
                    DataTable table = new DataTable("ANGAJATI");
                    table.Load(reader);
                    return table;
                }
            }
        }

        public static DataTable GetAngajati(string nume, string prenume)
        {
            string nameFilter = string.IsNullOrWhiteSpace(nume) ? null : nume;
            string firstNameFilter = string.IsNullOrWhiteSpace(prenume) ? null : prenume;
            return QueryAngajati(nameFilter, firstNameFilter, null);
        }

        public static DataTable GetAngajati(string filtruCautare = "")
        {
            return QueryAngajati(null, null, filtruCautare);
        }

        public static AngajatModel GetAngajatById(int id)
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM ANGAJATI WHERE NR_CRT = :id";
                SetBindByName(command);
                AddParameter(command, "id", id, DbType.Int32);

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    AngajatModel result = new AngajatModel();
                    result.NrCrt = Convert.ToInt32(reader["NR_CRT"], CultureInfo.InvariantCulture);
                    result.Nume = Convert.ToString(reader["NUME"], CultureInfo.InvariantCulture);
                    result.Prenume = Convert.ToString(reader["PRENUME"], CultureInfo.InvariantCulture);
                    result.Functie = Convert.ToString(reader["FUNCTIE"], CultureInfo.InvariantCulture);
                    result.SalarBaza = Convert.ToInt32(reader["SALAR_BAZA"], CultureInfo.InvariantCulture);
                    result.SporProcent = Convert.ToDecimal(reader["SPOR_PROCENT"], CultureInfo.InvariantCulture);
                    result.PremiiBrute = Convert.ToInt32(reader["PREMII_BRUTE"], CultureInfo.InvariantCulture);
                    result.Retineri = Convert.ToInt32(reader["RETINERI"], CultureInfo.InvariantCulture);
                    result.TotalBrut = Convert.ToInt32(reader["TOTAL_BRUT"], CultureInfo.InvariantCulture);
                    result.Cas = Convert.ToInt32(reader["CAS"], CultureInfo.InvariantCulture);
                    result.Cass = Convert.ToInt32(reader["CASS"], CultureInfo.InvariantCulture);
                    result.BrutImpozabil = Convert.ToInt32(reader["BRUT_IMPOZABIL"], CultureInfo.InvariantCulture);
                    result.Impozit = Convert.ToInt32(reader["IMPOZIT"], CultureInfo.InvariantCulture);
                    result.ViratCard = Convert.ToInt32(reader["VIRAT_CARD"], CultureInfo.InvariantCulture);
                    result.Poza = ReadPhoto(reader["POZA"]);
                    return result;
                }
            }
        }

        public static bool InsertAngajat(string nume, string prenume, string functie,
            int salarBaza, decimal spor, int premii, int retineri, byte[] poza)
        {
            nume = (nume ?? string.Empty).Trim();
            prenume = (prenume ?? string.Empty).Trim();
            functie = (functie ?? string.Empty).Trim();
            ValidateAngajat(nume, prenume, functie, salarBaza, spor, premii, retineri, poza);

            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO ANGAJATI (NUME, PRENUME, FUNCTIE, SALAR_BAZA, SPOR_PROCENT, PREMII_BRUTE, RETINERI, POZA) VALUES (:nume, :prenume, :functie, :salariu, :spor, :premii, :retineri, :poza)";
                SetBindByName(command);
                AddParameter(command, "nume", nume, DbType.String, 50);
                AddParameter(command, "prenume", prenume, DbType.String, 50);
                AddParameter(command, "functie", functie, DbType.String, 50);
                AddParameter(command, "salariu", salarBaza, DbType.Int32);
                AddParameter(command, "spor", spor, DbType.Decimal);
                AddParameter(command, "premii", premii, DbType.Int32);
                AddParameter(command, "retineri", retineri, DbType.Int32);
                AddParameter(command, "poza", DbValue(poza), DbType.Binary);
                command.ExecuteNonQuery();
            }

            return true;
        }

        public static bool UpdateAngajat(int id, string nume, string prenume, string functie,
            int salarBaza, decimal spor, int premii, int retineri, byte[] poza = null)
        {
            nume = (nume ?? string.Empty).Trim();
            prenume = (prenume ?? string.Empty).Trim();
            functie = (functie ?? string.Empty).Trim();
            ValidateAngajat(nume, prenume, functie, salarBaza, spor, premii, retineri, poza);

            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                string photoUpdate = poza == null ? string.Empty : ", POZA = :poza";
                command.CommandText = "UPDATE ANGAJATI SET NUME = :nume, PRENUME = :prenume, FUNCTIE = :functie, SALAR_BAZA = :salariu, SPOR_PROCENT = :spor, PREMII_BRUTE = :premii, RETINERI = :retineri" + photoUpdate + " WHERE NR_CRT = :id";
                SetBindByName(command);
                AddParameter(command, "nume", nume, DbType.String, 50);
                AddParameter(command, "prenume", prenume, DbType.String, 50);
                AddParameter(command, "functie", functie, DbType.String, 50);
                AddParameter(command, "salariu", salarBaza, DbType.Int32);
                AddParameter(command, "spor", spor, DbType.Decimal);
                AddParameter(command, "premii", premii, DbType.Int32);
                AddParameter(command, "retineri", retineri, DbType.Int32);
                if (poza != null)
                {
                    AddParameter(command, "poza", poza, DbType.Binary);
                }
                AddParameter(command, "id", id, DbType.Int32);
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool DeleteAngajat(int id)
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM ANGAJATI WHERE NR_CRT = :id";
                SetBindByName(command);
                AddParameter(command, "id", id, DbType.Int32);
                return command.ExecuteNonQuery() == 1;
            }
        }

        private static void ExecuteRecalculation(IDbConnection connection, IDbTransaction transaction)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "BEGIN PROC_RECALCULARE_SALARII; END;";
                SetBindByName(command);
                command.ExecuteNonQuery();
            }
        }

        public static void RecalculeazaToateSalariile()
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                ExecuteRecalculation(connection, transaction);
                transaction.Commit();
            }
        }

        public static TotaluriModel GetTotaluri()
        {
            using (IDbConnection connection = CreateOracleConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT NVL(SUM(TOTAL_BRUT), 0) AS TOTAL_BRUT, NVL(SUM(VIRAT_CARD), 0) AS TOTAL_VIRAT FROM ANGAJATI";
                using (IDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    TotaluriModel result = new TotaluriModel();
                    result.TotalBrut = Convert.ToInt64(reader["TOTAL_BRUT"], CultureInfo.InvariantCulture);
                    result.TotalVirat = Convert.ToInt64(reader["TOTAL_VIRAT"], CultureInfo.InvariantCulture);
                    return result;
                }
            }
        }
    }
}
