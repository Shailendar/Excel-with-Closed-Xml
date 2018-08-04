using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExcelPractice.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace ExcelPractice.Controllers
{
    public class HomeController : Controller
    {
        ExcelDBContext db = new ExcelDBContext();

        public ActionResult Index()
        {
            List<Company> company = db.Company.ToList();
            return View(company);
        }
        [HttpPost]
        public ActionResult Index(List<Company> company)
        {
            company = db.Company.ToList();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            List<int> companyType = company.Select(m => m.CompanyTypeId).OrderBy(m => m).Distinct().ToList();

            companyType.ForEach(m =>
            {
                List<Company> filtered = new List<Company>();
                filtered = company.Where(e => e.CompanyTypeId == m).ToList();

                dt = ExportData(filtered, m);
                //dt.TableName = "Shift" + m;

                ds.Tables.Add(dt);
            });

            //dt = ExportData(company);
            //dt.TableName = "Company";
            //ds.Tables.Add(dt);
            string fileName = "Company List";
            string header = "Company List";
            ExportDataToExcel(ds, fileName, header);
            return View(company);
        }

        private DataTable ExportData(List<Company> company, int cId)
        {
            DataColumn dc = new DataColumn();
            DataTable dt = new DataTable("Company Type" + cId);

            dc = dt.Columns.Add("Company Name");
            dc = dt.Columns.Add("Email Address");
            dc = dt.Columns.Add("Domain Name");

            for (var i = 0; i < company.Count; i++)
            {
                var item = company[i];
                dt.Rows.Add(item.CompanyName, item.EmailAddress, item.DomainName);
            }
            return dt;
        }

        private string ExportDataToExcel(DataSet ds, string fileName, string header)
        {
            string strError = string.Empty;

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CompanyList.xlsx");

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return strError;
        }
    }
}