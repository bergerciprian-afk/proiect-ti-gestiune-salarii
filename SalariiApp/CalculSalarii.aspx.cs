using System;
using System.Data;
using System.Web.UI;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class CalculSalarii : Page
    {
        private long _totalBrutSum = 0;
        private long _totalViratSum = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IncarcaDate();
            }
        }

        private void IncarcaDate()
        {
            try
            {
                TaxParams taxe = DbHelper.GetParametriTaxe();
                lblBadgeCas.Text = taxe.CasProcent.ToString("0.#");
                lblBadgeCass.Text = taxe.CassProcent.ToString("0.#");
                lblBadgeImp.Text = taxe.ImpozitProcent.ToString("0.#");

                string filtru = txtFiltru.Text.Trim();
                DataTable dt = DbHelper.GetAngajati(filtru);

                _totalBrutSum = 0;
                _totalViratSum = 0;
                foreach (DataRow r in dt.Rows)
                {
                    _totalBrutSum += Convert.ToInt64(r["TOTAL_BRUT"]);
                    _totalViratSum += Convert.ToInt64(r["VIRAT_CARD"]);
                }

                gvCalcul.DataSource = dt;
                gvCalcul.DataBind();

                if (gvCalcul.FooterRow != null)
                {
                    gvCalcul.FooterRow.Cells[1].Text = "TOTALURI:";
                    gvCalcul.FooterRow.Cells[7].Text = _totalBrutSum.ToString("N0") + " RON";
                    gvCalcul.FooterRow.Cells[13].Text = _totalViratSum.ToString("N0") + " RON";
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare(ex.Message);
            }
        }

        protected void btnFiltreaza_Click(object sender, EventArgs e)
        {
            IncarcaDate();
        }

        protected void btnRecalculeazaTot_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.RecalculeazaToateSalariile();
                IncarcaDate();
                AfiseazaSucces("Recalcularea salariilor a fost efectuata.");
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare la recalculare: " + ex.Message);
            }
        }

        private void AfiseazaSucces(string msg)
        {
            divAlert.Attributes["class"] = "alert alert-success";
            lblAlertMsg.Text = msg;
            pnlAlert.Visible = true;
        }

        private void AfiseazaEroare(string msg)
        {
            divAlert.Attributes["class"] = "alert alert-danger";
            lblAlertMsg.Text = msg;
            pnlAlert.Visible = true;
        }
    }
}

// Declaratiile controalelor din pagina.
namespace SalariiApp
{
    public partial class CalculSalarii
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMsg;
        protected global::System.Web.UI.WebControls.Label lblBadgeCas;
        protected global::System.Web.UI.WebControls.Label lblBadgeCass;
        protected global::System.Web.UI.WebControls.Label lblBadgeImp;
        protected global::System.Web.UI.WebControls.TextBox txtFiltru;
        protected global::System.Web.UI.WebControls.Button btnFiltreaza;
        protected global::System.Web.UI.WebControls.Button btnRecalculeazaTot;
        protected global::System.Web.UI.WebControls.GridView gvCalcul;
    }
}
