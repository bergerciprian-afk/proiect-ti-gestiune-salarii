using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class ActualizareDate : Page
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
                string filtru = txtFiltru.Text.Trim();
                DataTable dt = DbHelper.GetAngajati(filtru);
                gvAngajati.DataSource = dt;
                gvAngajati.DataBind();
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare: " + ex.Message);
            }
        }

        protected void btnCauta_Click(object sender, EventArgs e)
        {
            gvAngajati.EditIndex = -1;
            IncarcaAngajati();
        }

        protected void btnReseteazaFiltru_Click(object sender, EventArgs e)
        {
            txtFiltru.Text = string.Empty;
            gvAngajati.EditIndex = -1;
            IncarcaAngajati();
        }

        protected void gvAngajati_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAngajati.EditIndex = e.NewEditIndex;
            IncarcaAngajati();
        }

        protected void gvAngajati_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAngajati.EditIndex = -1;
            IncarcaAngajati();
        }

        protected void gvAngajati_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvAngajati.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvAngajati.Rows[e.RowIndex];

                string nume = ((TextBox)row.FindControl("txtGvNume")).Text.Trim();
                string prenume = ((TextBox)row.FindControl("txtGvPrenume")).Text.Trim();
                string functie = ((TextBox)row.FindControl("txtGvFunctie")).Text.Trim();
                int salar = int.Parse(((TextBox)row.FindControl("txtGvSalar")).Text.Trim());
                decimal spor = DbHelper.ParseDecimal(((TextBox)row.FindControl("txtGvSpor")).Text);
                int premii = int.Parse(((TextBox)row.FindControl("txtGvPremii")).Text.Trim());
                int retineri = int.Parse(((TextBox)row.FindControl("txtGvRetineri")).Text.Trim());

                bool ok = DbHelper.UpdateAngajat(id, nume, prenume, functie, salar, spor, premii, retineri);
                if (ok)
                {
                    gvAngajati.EditIndex = -1;
                    IncarcaAngajati();
                    AfiseazaSucces("Datele angajatului " + nume + " " + prenume + " au fost actualizate.");
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare la actualizare: " + ex.Message);
            }
        }

        protected void gvAngajati_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvAngajati.SelectedDataKey.Value);
                AngajatModel a = DbHelper.GetAngajatById(id);
                if (a != null)
                {
                    hfSelectedId.Value = a.NrCrt.ToString();
                    lblNumeAngajatSelectat.Text = string.Format("{0} {1} (ID {2})", a.Nume, a.Prenume, a.NrCrt);
                    txtEditNume.Text = a.Nume;
                    txtEditPrenume.Text = a.Prenume;
                    txtEditFunctie.Text = a.Functie;
                    txtEditSalar.Text = a.SalarBaza.ToString();
                    txtEditSpor.Text = a.SporProcent.ToString();
                    txtEditPremii.Text = a.PremiiBrute.ToString();
                    txtEditRetineri.Text = a.Retineri.ToString();
                    txtEditTotalBrut.Text = a.TotalBrut.ToString("N0") + " RON";
                    txtEditCas.Text = a.Cas.ToString("N0") + " RON";
                    txtEditCass.Text = a.Cass.ToString("N0") + " RON";
                    txtEditBrutImp.Text = a.BrutImpozabil.ToString("N0") + " RON";
                    txtEditImpozit.Text = a.Impozit.ToString("N0") + " RON";
                    txtEditViratCard.Text = a.ViratCard.ToString("N0") + " RON";

                    pnlEditSingle.Visible = true;
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare(ex.Message);
            }
        }

        protected void btnSalveazaModificari_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfSelectedId.Value);
                string nume = txtEditNume.Text.Trim();
                string prenume = txtEditPrenume.Text.Trim();
                string functie = txtEditFunctie.Text.Trim();
                int salar = int.Parse(txtEditSalar.Text.Trim());
                decimal spor = DbHelper.ParseDecimal(txtEditSpor.Text);
                int premii = int.Parse(txtEditPremii.Text.Trim());
                int retineri = int.Parse(txtEditRetineri.Text.Trim());

                bool ok = DbHelper.UpdateAngajat(id, nume, prenume, functie, salar, spor, premii, retineri);
                if (ok)
                {
                    pnlEditSingle.Visible = false;
                    IncarcaAngajati();
                    AfiseazaSucces("Modificarile au fost salvate.");
                }
            }
            catch (Exception ex)
            {
                AfiseazaEroare("Eroare: " + ex.Message);
            }
        }

        protected void btnAnuleazaEditare_Click(object sender, EventArgs e)
        {
            pnlEditSingle.Visible = false;
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
    public partial class ActualizareDate
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMsg;
        protected global::System.Web.UI.WebControls.TextBox txtFiltru;
        protected global::System.Web.UI.WebControls.Button btnCauta;
        protected global::System.Web.UI.WebControls.Button btnReseteazaFiltru;
        protected global::System.Web.UI.WebControls.Panel pnlEditSingle;
        protected global::System.Web.UI.WebControls.Label lblNumeAngajatSelectat;
        protected global::System.Web.UI.WebControls.HiddenField hfSelectedId;
        protected global::System.Web.UI.WebControls.TextBox txtEditNume;
        protected global::System.Web.UI.WebControls.TextBox txtEditPrenume;
        protected global::System.Web.UI.WebControls.TextBox txtEditFunctie;
        protected global::System.Web.UI.WebControls.TextBox txtEditSalar;
        protected global::System.Web.UI.WebControls.TextBox txtEditSpor;
        protected global::System.Web.UI.WebControls.TextBox txtEditPremii;
        protected global::System.Web.UI.WebControls.TextBox txtEditRetineri;
        protected global::System.Web.UI.WebControls.TextBox txtEditTotalBrut;
        protected global::System.Web.UI.WebControls.TextBox txtEditCas;
        protected global::System.Web.UI.WebControls.TextBox txtEditCass;
        protected global::System.Web.UI.WebControls.TextBox txtEditBrutImp;
        protected global::System.Web.UI.WebControls.TextBox txtEditImpozit;
        protected global::System.Web.UI.WebControls.TextBox txtEditViratCard;
        protected global::System.Web.UI.WebControls.Button btnSalveazaModificari;
        protected global::System.Web.UI.WebControls.Button btnAnuleazaEditare;
        protected global::System.Web.UI.WebControls.GridView gvAngajati;
    }
}
