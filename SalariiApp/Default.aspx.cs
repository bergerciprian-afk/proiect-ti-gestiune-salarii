using System;
using System.Data;
using System.Web.UI;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IncarcaStatistici();
            }
        }

        private void IncarcaStatistici()
        {
            try
            {
                DataTable dt = DbHelper.GetAngajati("");
                lblTotalAngajati.Text = dt.Rows.Count.ToString();

                TaxParams taxe = DbHelper.GetParametriTaxe();
                lblTaxeCurenteVizibile.Text = string.Format("CAS {0:0.##}% | CASS {1:0.##}% | Impozit {2:0.##}%",
                    taxe.CasProcent, taxe.CassProcent, taxe.ImpozitProcent);
                lblStatusAplicatie.Text = "Conectat la Oracle";
                lblEroareConexiune.Visible = false;
            }
            catch (Exception ex)
            {
                lblTotalAngajati.Text = "-";
                lblTaxeCurenteVizibile.Text = "-";
                lblStatusAplicatie.Text = "Oracle indisponibil";
                lblEroareConexiune.Text = "Nu s-a putut conecta la baza de date. Verificati Oracle XE si configurarea din Web.config. Detalii: " + ex.Message;
                lblEroareConexiune.Visible = true;
            }
        }
    }
}

// Declaratiile controalelor din pagina.
namespace SalariiApp
{
    public partial class Default
    {
        protected global::System.Web.UI.WebControls.Label lblTotalAngajati;
        protected global::System.Web.UI.WebControls.Label lblStatusAplicatie;
        protected global::System.Web.UI.WebControls.Label lblTaxeCurenteVizibile;
        protected global::System.Web.UI.WebControls.Label lblEroareConexiune;
    }
}
