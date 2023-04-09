﻿using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FTWebCrystalReport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;
namespace FTWebCrystalReport.Controllers
{
    public class GenReportController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GenerateReport([FromBody] ft_ORPT t)
        {
            HttpStatusCode stsCode = HttpStatusCode.OK;

            string test = "";
            string folder = "", path = "", sql = "", keyColumn = "", key = "", filename = "", message = "", sapdocentry = "";
            try
            {
                ft_ORPT h = new ft_ORPT();
                h = ft_ORPT.LoadById(t.Id);

                folder = HostingEnvironment.MapPath("~/Pdf/" + t.Id + "/");
                if (!File.Exists(folder)) Directory.CreateDirectory(folder);
                string[] filePaths = Directory.GetFiles(HostingEnvironment.MapPath("~/Pdf/" + t.Id + "/"), "*.pdf");
                string root = "";
                if (filePaths.Length > 0)
                {
                    foreach (string item in filePaths)
                    {
                        root = item;
                        FileInfo info = new FileInfo(root);
                        filename = Path.GetFileName(info.FullName);
                        path = HostingEnvironment.MapPath("~/Pdf/" + h.Id + "/") + filename;
                        if (File.Exists(root))
                        {
                            if (File.Exists(path))
                                File.Delete(path);
                        }
                    }
                }
                ReportDocument a = new ReportDocument();
                //string sapdb = WebConfigurationManager.AppSettings["sapdb"].ToString();
                string rpt = HostingEnvironment.MapPath("~/" + h.RptPath);
                a.Load(rpt);

                string conStr = WebConfigurationManager.ConnectionStrings["WebConnectionString"].ToString();

                using (SqlConnection con = new SqlConnection(conStr))
                {

                    ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                    TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                    Tables CrTables;
                    SqlCommand cmd = new SqlCommand("exec LoadCrConn_sp " + t.Id.ToString(), con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    crConnectionInfo.ServerName = dt.Rows[0]["CrServer"].ToString();
                    crConnectionInfo.DatabaseName = dt.Rows[0]["CrDatabase"].ToString();// con.Database.ToString();// dt.Rows[0]["sapcompany"].ToString();
                    crConnectionInfo.UserID = dt.Rows[0]["CrDBUser"].ToString();
                    crConnectionInfo.Password = dt.Rows[0]["CrDBPwd"].ToString();
                    CrTables = a.Database.Tables;
                    foreach (Table CrTable in CrTables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo;
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                        CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    }
                    foreach (ft_RPT1 d in t.Lines)
                    {
                        if (d.DataType.ToUpper() == "DATE")
                        {
                            a.SetParameterValue(d.ParamName, DateTime.Parse(d.ParamValue));
                        }
                        else if (d.DataType.ToUpper() == "NUMERIC")
                        {

                            a.SetParameterValue(d.ParamName, int.Parse(d.ParamValue));
                        }
                        else
                        {
                            a.SetParameterValue(d.ParamName, d.ParamValue);
                        }
                    }
                }

                folder = HostingEnvironment.MapPath("~/Pdf/" + t.Id + "/");
                if (!File.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);
                filename = key.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                path = folder + filename;
                a.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                a.Close();
                a.Dispose();
                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //var stream = new FileStream(path, FileMode.Open);
                //result.Content = new StreamContent(stream);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = filename;
                //return result;
                //}
                message = filename;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                stsCode = HttpStatusCode.BadRequest;
            }
            return Request.CreateResponse<string>(stsCode, message);
        }
    }
}