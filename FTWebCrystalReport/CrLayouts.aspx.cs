using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FTWebCrystalReport
{
    public partial class CrLayouts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod(EnableSession = true)]
        public static string GetData()
        {
            string msg = "";
            try
            {


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        End:
            return msg;
        }

    }
}