using System;
using System.Data;
using System.Web.UI;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class StergereAngajat : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IncarcaAngajati();
            }
        }

        private void IncarcaAngajati()
        {
            try
            {
                string nume = txtCautaNume.Text.Trim();
                string prenume = txtCautaPrenume.Text.Trim();

                DataTable dt = DbHelper.GetAngajati(nume, prenume);
                gvStergere.DataSource = dt;
                gvStergere.DataBind();

                if (dt.Rows.Count == 0 && (!string.IsNullOrEmpty(nume) || !string.IsNullOrEmpty(prenume)))
                {
                    AfiseazaEroare("Nu a fost gasit niciun angajat conform criteriilor de cautare.");
                }
                else
                {
                    pnlAlert.Visible = false;
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare: " + ex.Message);
            }
        }

        protected void btnCauta_Click(object sender, EventArgs e)
        {
            pnlConfirmare.Visible = false;
            IncarcaAngajati();
        }

        protected void btnArataToti_Click(object sender, EventArgs e)
        {
            txtCautaNume.Text = string.Empty;
            txtCautaPrenume.Text = string.Empty;
            pnlConfirmare.Visible = false;
            IncarcaAngajati();
        }

        protected void gvStergere_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvStergere.SelectedDataKey.Value);
                AngajatModel a = DbHelper.GetAngajatById(id);
                if (a != null)
                {
                    hfDeleteId.Value = a.NrCrt.ToString();
                    lblConfirmNume.Text = string.Format("{0} {1} (ID: {2})", a.Nume, a.Prenume, a.NrCrt);
                    lblConfirmFunctie.Text = a.Functie;
                    lblConfirmSalar.Text = a.SalarBaza.ToString("N0");
                    pnlConfirmare.Visible = true;
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare(ex.Message);
            }
        }

        protected void btnExecutaStergere_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfDeleteId.Value);
                string infoAngajat = lblConfirmNume.Text;

                bool succes = DbHelper.DeleteAngajat(id);
                if (succes)
                {
                    pnlConfirmare.Visible = false;
                    IncarcaAngajati();
                    AfiseazaSucces("Angajatul " + infoAngajat + " a fost sters.");
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare la stergere: " + ex.Message);
            }
        }

        protected void btnAnuleazaStergere_Click(object sender, EventArgs e)
        {
            pnlConfirmare.Visible = false;
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
    public partial class StergereAngajat
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMsg;
        protected global::System.Web.UI.WebControls.TextBox txtCautaNume;
        protected global::System.Web.UI.WebControls.TextBox txtCautaPrenume;
        protected global::System.Web.UI.WebControls.Button btnCauta;
        protected global::System.Web.UI.WebControls.Button btnArataToti;
        protected global::System.Web.UI.WebControls.Panel pnlConfirmare;
        protected global::System.Web.UI.WebControls.Label lblConfirmNume;
        protected global::System.Web.UI.WebControls.Label lblConfirmFunctie;
        protected global::System.Web.UI.WebControls.Label lblConfirmSalar;
        protected global::System.Web.UI.WebControls.HiddenField hfDeleteId;
        protected global::System.Web.UI.WebControls.Button btnExecutaStergere;
        protected global::System.Web.UI.WebControls.Button btnAnuleazaStergere;
        protected global::System.Web.UI.WebControls.GridView gvStergere;
    }
}
