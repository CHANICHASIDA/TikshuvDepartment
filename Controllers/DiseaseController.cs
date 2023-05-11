using TikshuvProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace TikshuvProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController:ControllerBase
    {
        public readonly IConfiguration _configuration;
        List<Disease> diseaseList = new List<Disease>();
        public DiseaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("NewDisease")]
        public string newDisease(Disease disease)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter cmd1 = new SqlDataAdapter("SELECT * FROM Customer WHERE id='" + disease.id + "'", con);
            DataTable db = new DataTable();
            con.Open();
            cmd1.Fill(db);
            while (db.Rows.Count == 0)
                return "id not exsist in the DB";
            cmd1 = new SqlDataAdapter("SELECT * FROM Disease WHERE id='" + disease.id + "'", con);
            db = new DataTable();
            cmd1.Fill(db);
            while (db.Rows.Count > 0)
                return "you are be positive already";
            while (disease.positiveDate.Date > disease.recoveryDate.Date)
                return "Your recovery Date cannot be before you tested positive";
            while (disease.positiveDate > DateTime.Now)
                return "Error - entering a future date in a positiveDate";
            con.Close();
            int yearp = disease.positiveDate.Year;
            int monthp =disease.positiveDate.Month+100;
            int dayp = disease.positiveDate.Day+100;
            int yearr = disease.recoveryDate.Year;
            int monthr = disease.recoveryDate.Month + 100;
            int dayr = disease.recoveryDate.Day + 100;
            string datep = yearp.ToString() + '-' + monthp.ToString().Substring(1, 2) + '-' + dayp.ToString().Substring(1, 2);
            string dater = yearr.ToString() + '-' + monthr.ToString().Substring(1, 2) + '-' + dayr.ToString().Substring(1, 2);
            SqlCommand cmd = new SqlCommand("INSERT INTO Disease(id,positiveDate,recoveryDate)VALUES('" + disease.id + "','" + datep + "','" + dater + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
                return "Data-Insert";
            else
                return "Failed";
        }
        [HttpGet]
        [Route("GetDisease")]
        public string GetDisease()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Disease", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Disease disease = new Disease();
                    disease.id = Convert.ToString(dt.Rows[i]["id"]);
                    disease.positiveDate = Convert.ToDateTime(dt.Rows[i]["positiveDate"]);
                    disease.recoveryDate = Convert.ToDateTime(dt.Rows[i]["recoveryDate"]);
                    diseaseList.Add(disease);

                }
            }
            if (diseaseList.Count > 0)
            {
                return JsonConvert.SerializeObject(diseaseList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);

            }
        }
        [HttpPost]
        [Route("log-in")]
        public string LogIn(string id)
        {
            Disease disease = new Disease();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Disease WHERE id='" + id + "'", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            Response response = new Response();
            con.Close();
            if (dt.Rows.Count > 0)
            {
                disease.id = Convert.ToString(dt.Rows[0]["id"]);
                disease.positiveDate = Convert.ToDateTime(dt.Rows[0]["positiveDate"]);
                disease.recoveryDate = Convert.ToDateTime(dt.Rows[0]["recoveryDate"]);
            }
            if (disease.id != null)
                return JsonConvert.SerializeObject(disease);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);

            }

        }
    }
}
