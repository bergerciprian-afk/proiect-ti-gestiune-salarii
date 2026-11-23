<%@ WebHandler Language="C#" Class="SalariiApp.PozaAngajat" %>

using System;
using System.Web;
using SalariiApp.Classes;

namespace SalariiApp
{
    public class PozaAngajat : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int id;
            if (int.TryParse(context.Request.QueryString["id"], out id))
            {
                var a = DbHelper.GetAngajatById(id);
                if (a != null && a.Poza != null && a.Poza.Length > 0)
                {
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite(a.Poza);
                    return;
                }
            }

            // Daca nu exista poza, returnam 404 sau imagine goala
            context.Response.StatusCode = 404;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
