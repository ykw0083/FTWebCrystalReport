using FTWebCrystalReport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace FTWebCrystalReport.Controllers
{
    public class GetReportController : ApiController
    {
        [HttpGet]
        [Route("api/getreportparams/single/{value}")]
        public ft_ORPT GetDocumentsById(string value)
        {
            ft_ORPT h = new ft_ORPT();
            h = ft_ORPT.LoadById(int.Parse(value));
            return h;
        }
        [HttpGet]
        [Route("api/getreportparams/query/{value}")]
        public DataTable GetQuery(string value)
        {
            DataTable dt = new DataTable();
            ft_RPT1 line = new ft_RPT1();
            line = ft_RPT1.LoadLinesById(int.Parse(value));

            if (line.ParamSQL != "" && line.ParamSQL != null)
                dt = LoadQuery(line.ParamSQL);

            return dt;
        }

        private DataTable LoadQuery(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

    }
}
