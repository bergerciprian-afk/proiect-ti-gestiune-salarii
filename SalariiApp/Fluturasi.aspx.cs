using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class Fluturasi : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IncarcaDropdownAngajati();
                IncarcaFluturasi();
            }
        }

        private void IncarcaDropdownAngajati()
        {
            try
            {
                DataTable dt = DbHelper.GetAngajati("");
                ddlAngajati.Items.Clear();
                ddlAngajati.Items.Add(new ListItem("-- Toti Angajatii --", "0"));

                foreach (DataRow r in dt.Rows)
                {
                    string text = string.Format("{0} {1} ({2})", r["NUME"], r["PRENUME"], r["FUNCTIE"]);
                    string val = r["NR_CRT"].ToString();
                    ddlAngajati.Items.Add(new ListItem(text, val));
                }
            }
            catch { }
        }

        private void IncarcaFluturasi()
        {
            try
            {
                DataTable dt = DbHelper.GetAngajati("");
                TaxParams taxe = DbHelper.GetParametriTaxe();

                if (!dt.Columns.Contains("DATA_CURENTA")) dt.Columns.Add("DATA_CURENTA", typeof(string));
                if (!dt.Columns.Contains("CAS_PROC")) dt.Columns.Add("CAS_PROC", typeof(string));
                if (!dt.Columns.Contains("CASS_PROC")) dt.Columns.Add("CASS_PROC", typeof(string));
                if (!dt.Columns.Contains("IMP_PROC")) dt.Columns.Add("IMP_PROC", typeof(string));

                string dataAzi = DateTime.Now.ToString("dd.MM.yyyy");

                foreach (DataRow r in dt.Rows)
                {
                    r["DATA_CURENTA"] = dataAzi;
                    r["CAS_PROC"] = taxe.CasProcent.ToString("0.#");
                    r["CASS_PROC"] = taxe.CassProcent.ToString("0.#");
                    r["IMP_PROC"] = taxe.ImpozitProcent.ToString("0.#");
                }

                int selectedId = int.Parse(ddlAngajati.SelectedValue);
                if (selectedId > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "NR_CRT = " + selectedId;
                    rptFluturasi.DataSource = dv;
                }
                else
                {
                    rptFluturasi.DataSource = dt;
                }

                rptFluturasi.DataBind();
            }
            catch (Exception ex)
            {
                string message = HttpUtility.JavaScriptStringEncode("Eroare la incarcarea fluturasilor: " + ex.Message);
                ClientScript.RegisterStartupScript(GetType(), "fluturasiError", "alert('" + message + "');", true);
            }
        }

        protected void ddlAngajati_SelectedIndexChanged(object sender, EventArgs e)
        {
            IncarcaFluturasi();
        }

        protected void btnArataToti_Click(object sender, EventArgs e)
        {
            ddlAngajati.SelectedValue = "0";
            IncarcaFluturasi();
        }
    }
}

// Declaratiile controalelor din pagina.
namespace SalariiApp
{
    public partial class Fluturasi
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlAngajati;
        protected global::System.Web.UI.WebControls.Button btnArataToti;
        protected global::System.Web.UI.WebControls.Repeater rptFluturasi;
    }
}
