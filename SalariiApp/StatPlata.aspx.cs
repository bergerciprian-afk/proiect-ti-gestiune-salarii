using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class StatPlata : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime now = DateTime.Now;
                CultureInfo culture = CultureInfo.GetCultureInfo("ro-RO");
                string lunaCurenta = now.ToString("MMMM yyyy", culture);
                txtLunaCalcul.Text = lunaCurenta;
                lblLunaAfisata.Text = lunaCurenta;
                lblDataEmiterii.Text = now.ToString("dd.MM.yyyy");
                IncarcaStatPlata();
            }
        }

        private void IncarcaStatPlata()
        {
            try
            {
                TaxParams taxe = DbHelper.GetParametriTaxe();
                lblCasProcent.Text = taxe.CasProcent.ToString("0.##");
                lblCassProcent.Text = taxe.CassProcent.ToString("0.##");
                lblImpozitProcent.Text = taxe.ImpozitProcent.ToString("0.##");

                DataTable dt = DbHelper.GetAngajati("");
                rptStatPlata.DataSource = dt;
                rptStatPlata.DataBind();

                TotaluriModel totaluri = DbHelper.GetTotaluri();
                lblTotalBrutGeneral.Text = totaluri.TotalBrut.ToString("N0") + " RON";
                lblTotalViratGeneral.Text = totaluri.TotalVirat.ToString("N0") + " RON";
            }
            catch (Exception ex)
            {
                string message = HttpUtility.JavaScriptStringEncode("Eroare la incarcarea statului de plata: " + ex.Message);
                ClientScript.RegisterStartupScript(GetType(), "statPlataError", "alert('" + message + "');", true);
            }
        }

        protected void btnAplicaAntet_Click(object sender, EventArgs e)
        {
            string luna = txtLunaCalcul.Text.Trim();
            string info = txtInfoManual.Text.Trim();

            if (!string.IsNullOrEmpty(luna))
            {
                lblLunaAfisata.Text = Server.HtmlEncode(luna);
            }

            if (!string.IsNullOrEmpty(info))
            {
                lblInfoManualAfisat.Text = "| Note: " + Server.HtmlEncode(info);
            }
            else
            {
                lblInfoManualAfisat.Text = string.Empty;
            }
        }

        protected void btnDescarcaCsv_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DbHelper.GetAngajati("");
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("NR_CRT,NUME,PRENUME,FUNCTIE,SALAR_BAZA,SPOR_PROCENT,PREMII_BRUTE,TOTAL_BRUT,CAS,CASS,BRUT_IMPOZABIL,IMPOZIT,RETINERI,VIRAT_CARD");

                foreach (DataRow r in dt.Rows)
                {
                    object[] values = {
                        r["NR_CRT"], r["NUME"], r["PRENUME"], r["FUNCTIE"], r["SALAR_BAZA"],
                        r["SPOR_PROCENT"], r["PREMII_BRUTE"], r["TOTAL_BRUT"], r["CAS"], r["CASS"],
                        r["BRUT_IMPOZABIL"], r["IMPOZIT"], r["RETINERI"], r["VIRAT_CARD"]
                    };
                    string[] cells = Array.ConvertAll(values, CsvCell);
                    sb.AppendLine(string.Join(",", cells));
                }

                string lunaSlug = FileNamePart(txtLunaCalcul.Text);
                if (string.IsNullOrEmpty(lunaSlug)) lunaSlug = DateTime.Now.ToString("yyyyMM");
                string infoSlug = FileNamePart(txtInfoManual.Text);
                string fileName = "Stat_Plata_" + lunaSlug;
                if (!string.IsNullOrEmpty(infoSlug)) fileName += "_" + infoSlug;
                fileName += ".csv";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "utf-8";
                Response.ContentType = "text/csv";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.SuppressContent = true;
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                Response.ContentType = "text/plain; charset=utf-8";
                Response.Write("Exportul nu a putut fi generat: " + Server.HtmlEncode(ex.Message));
            }
        }

        private static string CsvCell(object value)
        {
            string text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        private static string FileNamePart(string value)
        {
            StringBuilder result = new StringBuilder();
            foreach (char character in (value ?? string.Empty).Trim())
            {
                if (char.IsLetterOrDigit(character) || character == '-' || character == '_')
                {
                    result.Append(character);
                }
                else if (char.IsWhiteSpace(character) && result.Length > 0 && result[result.Length - 1] != '_')
                {
                    result.Append('_');
                }
            }

            return result.ToString().Trim('_');
        }
    }
}

// Declaratiile controalelor din pagina.
namespace SalariiApp
{
    public partial class StatPlata
    {
        protected global::System.Web.UI.WebControls.TextBox txtLunaCalcul;
        protected global::System.Web.UI.WebControls.TextBox txtInfoManual;
        protected global::System.Web.UI.WebControls.Button btnAplicaAntet;
        protected global::System.Web.UI.WebControls.Button btnDescarcaCsv;
        protected global::System.Web.UI.WebControls.Label lblLunaAfisata;
        protected global::System.Web.UI.WebControls.Label lblDataEmiterii;
        protected global::System.Web.UI.WebControls.Label lblInfoManualAfisat;
        protected global::System.Web.UI.WebControls.Label lblCasProcent;
        protected global::System.Web.UI.WebControls.Label lblCassProcent;
        protected global::System.Web.UI.WebControls.Label lblImpozitProcent;
        protected global::System.Web.UI.WebControls.Repeater rptStatPlata;
        protected global::System.Web.UI.WebControls.Label lblTotalBrutGeneral;
        protected global::System.Web.UI.WebControls.Label lblTotalViratGeneral;
    }
}
