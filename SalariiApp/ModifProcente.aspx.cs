using System;
using System.Web.UI;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class ModifProcente : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool authenticated = IsAuthenticated();
            if (authenticated)
            {
                if (!IsPostBack)
                {
                    ArataFormularModificare();
                }
                return;
            }

            pnlAuth.Visible = true;
            pnlModificare.Visible = false;
        }

        private bool IsAuthenticated()
        {
            return Session["AdminAutentificat"] is bool && (bool)Session["AdminAutentificat"];
        }

        protected void btnAutentifica_Click(object sender, EventArgs e)
        {
            string parola = txtParolaAuth.Text.Trim();
            if (DbHelper.VerificaParola(parola))
            {
                Session["AdminAutentificat"] = true;
                pnlAlert.Visible = false;
                ArataFormularModificare();
            }
            else
            {
                AfiseazaEroare("Parola incorecta.");
            }
        }

        private void ArataFormularModificare()
        {
            pnlAuth.Visible = false;
            pnlModificare.Visible = true;

            var taxe = DbHelper.GetParametriTaxe();
            txtCas.Text = taxe.CasProcent.ToString("0.##");
            txtCass.Text = taxe.CassProcent.ToString("0.##");
            txtImpozit.Text = taxe.ImpozitProcent.ToString("0.##");
        }

        protected void btnSalveazaTaxe_Click(object sender, EventArgs e)
        {
            if (!RequireAuthentication()) return;
            if (!Page.IsValid) return;

            try
            {
                decimal cas = DbHelper.ParseDecimal(txtCas.Text);
                decimal cass = DbHelper.ParseDecimal(txtCass.Text);
                decimal imp = DbHelper.ParseDecimal(txtImpozit.Text);

                bool ok = DbHelper.UpdateParametriTaxe(cas, cass, imp);
                if (ok)
                {
                    AfiseazaSucces("Procentele au fost actualizate si salariile recalculate.");
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare: " + ex.Message);
            }
        }

        protected void btnReseteazaImplicite_Click(object sender, EventArgs e)
        {
            txtCas.Text = "25";
            txtCass.Text = "10";
            txtImpozit.Text = "10";
            Page.Validate();
            btnSalveazaTaxe_Click(sender, e);
        }

        protected void btnSchimbaParola_Click(object sender, EventArgs e)
        {
            if (!RequireAuthentication()) return;
            string noua = txtNouaParola.Text.Trim();
            string conf = txtConfirmaParola.Text.Trim();

            if (string.IsNullOrEmpty(noua))
            {
                AfiseazaEroare("Parola noua nu poate fi vida.");
                return;
            }

            if (noua != conf)
            {
                AfiseazaEroare("Parolele introduse nu coincid.");
                return;
            }

            try
            {
                bool ok = DbHelper.UpdateParola(noua);
                if (ok)
                {
                    txtNouaParola.Text = string.Empty;
                    txtConfirmaParola.Text = string.Empty;
                    AfiseazaSucces("Parola a fost modificata si criptata cu succes.");
                }
                else
                {
                    AfiseazaEroare("Nu s-a putut actualiza parola.");
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare la schimbarea parolei: " + ex.Message);
            }
        }

        protected void btnBlocheaza_Click(object sender, EventArgs e)
        {
            Session["AdminAutentificat"] = false;
            pnlModificare.Visible = false;
            pnlAuth.Visible = true;
            txtParolaAuth.Text = string.Empty;
            pnlAlert.Visible = false;
        }

        private bool RequireAuthentication()
        {
            if (IsAuthenticated()) return true;

            Session.Remove("AdminAutentificat");
            pnlModificare.Visible = false;
            pnlAuth.Visible = true;
            AfiseazaEroare("Sesiunea a expirat. Autentificati-va din nou.");
            return false;
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
    public partial class ModifProcente
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMsg;
        protected global::System.Web.UI.WebControls.Panel pnlAuth;
        protected global::System.Web.UI.WebControls.TextBox txtParolaAuth;
        protected global::System.Web.UI.WebControls.Button btnAutentifica;
        protected global::System.Web.UI.WebControls.Panel pnlModificare;
        protected global::System.Web.UI.WebControls.Button btnBlocheaza;
        protected global::System.Web.UI.WebControls.TextBox txtCas;
        protected global::System.Web.UI.WebControls.RangeValidator rvCas;
        protected global::System.Web.UI.WebControls.TextBox txtCass;
        protected global::System.Web.UI.WebControls.RangeValidator rvCass;
        protected global::System.Web.UI.WebControls.TextBox txtImpozit;
        protected global::System.Web.UI.WebControls.RangeValidator rvImp;
        protected global::System.Web.UI.WebControls.Button btnSalveazaTaxe;
        protected global::System.Web.UI.WebControls.Button btnReseteazaImplicite;
        protected global::System.Web.UI.WebControls.TextBox txtNouaParola;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmaParola;
        protected global::System.Web.UI.WebControls.Button btnSchimbaParola;
    }
}
