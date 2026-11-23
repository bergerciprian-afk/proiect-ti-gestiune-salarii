using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using SalariiApp.Classes;

namespace SalariiApp
{
    public partial class AdaugareAngajat : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlAlert.Visible = false;
                pnlRezultatCalcul.Visible = false;
            }
        }

        protected void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string nume = txtNume.Text.Trim();
                string prenume = txtPrenume.Text.Trim();
                string functie = txtFunctie.Text.Trim();
                int salarBaza = int.Parse(txtSalarBaza.Text.Trim());
                decimal spor = string.IsNullOrWhiteSpace(txtSpor.Text) ? 0 : DbHelper.ParseDecimal(txtSpor.Text);
                int premii = string.IsNullOrWhiteSpace(txtPremii.Text) ? 0 : int.Parse(txtPremii.Text.Trim());
                int retineri = string.IsNullOrWhiteSpace(txtRetineri.Text) ? 0 : int.Parse(txtRetineri.Text.Trim());

                byte[] pozaBytes = null;
                if (fuPoza.HasFile)
                {
                    if (fuPoza.PostedFile.ContentLength > 5 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Imaginea trebuie sa aiba cel mult 5 MB.");
                    }

                    using (Image image = Image.FromStream(fuPoza.PostedFile.InputStream, false, true))
                    using (MemoryStream jpeg = new MemoryStream())
                    {
                        image.Save(jpeg, ImageFormat.Jpeg);
                        pozaBytes = jpeg.ToArray();
                    }
                }

                bool succes = DbHelper.InsertAngajat(nume, prenume, functie, salarBaza, spor, premii, retineri, pozaBytes);

                if (succes)
                {
                    TaxParams taxe = DbHelper.GetParametriTaxe();
                    int totalBrut, cas, cass, brutImp, impozit, viratCard;
                    DbHelper.CalculFormule(salarBaza, spor, premii, retineri, taxe.CasProcent, taxe.CassProcent, taxe.ImpozitProcent,
                        out totalBrut, out cas, out cass, out brutImp, out impozit, out viratCard);

                    pnlRezultatCalcul.Visible = true;
                    lblResTotalBrut.Text = totalBrut.ToString("N0");
                    lblResCasProc.Text = taxe.CasProcent.ToString("0.#");
                    lblResCas.Text = cas.ToString("N0");
                    lblResCassProc.Text = taxe.CassProcent.ToString("0.#");
                    lblResCass.Text = cass.ToString("N0");
                    lblResBrutImp.Text = brutImp.ToString("N0");
                    lblResImpProc.Text = taxe.ImpozitProcent.ToString("0.#");
                    lblResImpozit.Text = impozit.ToString("N0");
                    lblResViratCard.Text = viratCard.ToString("N0");

                    divAlert.Attributes["class"] = "alert alert-success";
                    lblAlertMsg.Text = "Angajatul " + nume + " " + prenume + " a fost salvat.";
                    pnlAlert.Visible = true;

                    txtNume.Text = string.Empty;
                    txtPrenume.Text = string.Empty;
                    txtFunctie.Text = string.Empty;
                    txtSalarBaza.Text = string.Empty;
                    txtSpor.Text = "0";
                    txtPremii.Text = "0";
                    txtRetineri.Text = "0";
                    txtNume.Focus();
                }
            }
            catch (Exception ex)
            {
                divAlert.Attributes["class"] = "alert alert-danger";
                lblAlertMsg.Text = "Eroare: " + ex.Message;
                pnlAlert.Visible = true;
            }
        }

        protected void btnReseteaza_Click(object sender, EventArgs e)
        {
            txtNume.Text = string.Empty;
            txtPrenume.Text = string.Empty;
            txtFunctie.Text = string.Empty;
            txtSalarBaza.Text = string.Empty;
            txtSpor.Text = "0";
            txtPremii.Text = "0";
            txtRetineri.Text = "0";
            pnlAlert.Visible = false;
            pnlRezultatCalcul.Visible = false;
        }
    }
}

// Declaratiile controalelor din pagina.
namespace SalariiApp
{
    public partial class AdaugareAngajat
    {
        protected global::System.Web.UI.WebControls.Panel pnlAlert;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divAlert;
        protected global::System.Web.UI.WebControls.Label lblAlertMsg;
        protected global::System.Web.UI.WebControls.TextBox txtNume;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvNume;
        protected global::System.Web.UI.WebControls.TextBox txtPrenume;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvPrenume;
        protected global::System.Web.UI.WebControls.TextBox txtFunctie;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvFunctie;
        protected global::System.Web.UI.WebControls.TextBox txtSalarBaza;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvSalar;
        protected global::System.Web.UI.WebControls.RangeValidator rvSalar;
        protected global::System.Web.UI.WebControls.TextBox txtSpor;
        protected global::System.Web.UI.WebControls.RangeValidator rvSpor;
        protected global::System.Web.UI.WebControls.TextBox txtPremii;
        protected global::System.Web.UI.WebControls.RangeValidator rvPremii;
        protected global::System.Web.UI.WebControls.TextBox txtRetineri;
        protected global::System.Web.UI.WebControls.RangeValidator rvRetineri;
        protected global::System.Web.UI.WebControls.FileUpload fuPoza;
        protected global::System.Web.UI.WebControls.Button btnSalveaza;
        protected global::System.Web.UI.WebControls.Button btnReseteaza;
        protected global::System.Web.UI.WebControls.Panel pnlRezultatCalcul;
        protected global::System.Web.UI.WebControls.Label lblResTotalBrut;
        protected global::System.Web.UI.WebControls.Label lblResCasProc;
        protected global::System.Web.UI.WebControls.Label lblResCas;
        protected global::System.Web.UI.WebControls.Label lblResCassProc;
        protected global::System.Web.UI.WebControls.Label lblResCass;
        protected global::System.Web.UI.WebControls.Label lblResBrutImp;
        protected global::System.Web.UI.WebControls.Label lblResImpProc;
        protected global::System.Web.UI.WebControls.Label lblResImpozit;
        protected global::System.Web.UI.WebControls.Label lblResViratCard;
    }
}
