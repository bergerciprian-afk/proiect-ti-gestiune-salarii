using System;
using System.Web.UI;

namespace SalariiApp
{
    public partial class SiteMaster : MasterPage
    {
        private static readonly string[,] Banners =
        {
            { "~/Images/banner1.jpg", "Evidența angajaților în Oracle", "Caută și actualizează datele într-un singur loc." },
            { "~/Images/banner2.jpg", "Rapoarte pregătite pentru tipărire", "Stat de plată A3 și fluturași de salariu." },
            { "~/Images/banner3.jpg", "Salarii calculate automat", "Sporuri, contribuții și suma virată pe card." }
        };

        private static readonly Random Rand = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateClock();
                RotateBanner();
            }
        }

        protected void timerClock_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        protected void timerBanner_Tick(object sender, EventArgs e)
        {
            RotateBanner();
        }

        private void UpdateClock()
        {
            DateTime now = DateTime.Now;
            lblServerTime.Text = now.ToString("HH:mm:ss");
            lblServerDate.Text = now.ToString("dddd, dd MMMM yyyy");
        }

        private void RotateBanner()
        {
            int index;
            lock (Rand)
            {
                index = Rand.Next(Banners.GetLength(0));
            }

            imgAdBanner.ImageUrl = Banners[index, 0];
            lblPromoTitle.Text = Banners[index, 1];
            lblPromoDescription.Text = Banners[index, 2];
        }
    }
}

// Declaratiile controalelor din master page.
namespace SalariiApp
{
    public partial class SiteMaster
    {
        protected global::System.Web.UI.HtmlControls.HtmlForm mainForm;
        protected global::System.Web.UI.ScriptManager ScriptManager1;
        protected global::System.Web.UI.UpdatePanel upClock;
        protected global::System.Web.UI.WebControls.Label lblServerTime;
        protected global::System.Web.UI.WebControls.Label lblServerDate;
        protected global::System.Web.UI.Timer timerClock;
        protected global::System.Web.UI.UpdatePanel upBanner;
        protected global::System.Web.UI.WebControls.Image imgAdBanner;
        protected global::System.Web.UI.WebControls.Label lblPromoTitle;
        protected global::System.Web.UI.WebControls.Label lblPromoDescription;
        protected global::System.Web.UI.Timer timerBanner;
        protected global::System.Web.UI.WebControls.Menu MainMenu;
        protected global::System.Web.UI.WebControls.Panel pnlShortcuts;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder HeadContent;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder MainContent;
    }
}
