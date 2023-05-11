using TikshuvProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace TikshuvProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationController:ControllerBase
    {
        public readonly IConfiguration _configuration;
        List<Vaccination> vaccinationsList = new List<Vaccination>();

        public VaccinationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("NewVaccination")]
        public string newVaccination(Vaccination vaccination)
        {
            int yearp = vaccination.vaccinationDate.Year;
            int monthp = vaccination.vaccinationDate.Month + 100;
            int dayp = vaccination.vaccinationDate.Day + 100;
            string datep = yearp.ToString() + '-' + monthp.ToString().Substring(1, 2) + '-' + dayp.ToString().Substring(1, 2);
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter cmd1 = new SqlDataAdapter("SELECT * FROM Customer WHERE id='" + vaccination.id + "'", con);
            DataTable db = new DataTable();
            con.Open();
            cmd1.Fill(db);
            while (db.Rows.Count == 0)
                return "id not exsist in the DB";
            cmd1 = new SqlDataAdapter("SELECT * FROM Vaccination WHERE id='" + vaccination.id + "'", con);
            db=new DataTable();
            cmd1.Fill(db);
            while (db.Rows.Count >= 4)
                return "cant enter more vaccination";
            cmd1 = new SqlDataAdapter("SELECT * FROM Vaccination WHERE vaccinationDate='" + vaccination.vaccinationDate + "' AND id='"+datep+"'", con);
            db = new DataTable();
            cmd1.Fill(db);
            while (db.Rows.Count > 0)
                return "An error has already been inoculated on this day";
            while (vaccination.manufacturer != "moderna" && vaccination.manufacturer != "fizer")
                return "only fizer ot modena in israel";
            SqlCommand cmd = new SqlCommand("INSERT INTO Vaccination(id,vaccinationDate,manufacturer)VALUES('" + vaccination.id + "','" + datep + "','" + vaccination.manufacturer + "')", con);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Data-Insert";
            }
            else
            {
                return "Failed";
            }
        }
        [HttpGet]
        [Route("GetVaccination")]
        public string GetVaccination()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Vaccination", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Vaccination vaccination = new Vaccination();
                    vaccination.num = Convert.ToInt32(dt.Rows[i]["num"]);
                    vaccination.id = Convert.ToString(dt.Rows[i]["id"]);
                    vaccination.vaccinationDate = Convert.ToDateTime(dt.Rows[i]["vaccinationDate"]);
                    vaccination.manufacturer = dt.Rows[i]["manufacturer"].ToString();
                    vaccinationsList.Add(vaccination);

                }
            }
            if (vaccinationsList.Count > 0)
            {
                return JsonConvert.SerializeObject(vaccinationsList);
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
            string date="";
            Vaccination v = new Vaccination();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT vaccinationDate FROM Vaccination WHERE id='"+ id +"'ORDER BY vaccinationDate", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            Response response = new Response();
            con.Close();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    date += " the vaccination:" + (i + 1) + " inDate:" + dt.Rows[i]["vaccinationDate"].ToString() + ",";
                }
                return date;
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);

            }
        }
    }
}
