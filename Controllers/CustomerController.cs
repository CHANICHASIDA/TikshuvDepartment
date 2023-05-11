using TikshuvProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Linq;
using System.ComponentModel;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Net;
using System.Web.Helpers;
using System.Web;
using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Drawing;
using System.Drawing.Imaging;

namespace TikshuvProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Customer")]
        public string newCustomer(Customer customer)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter cmd1 = new SqlDataAdapter("SELECT * FROM Customer WHERE id='" + customer.id + "'", con);
            DataTable db = new DataTable();
            con.Open();
            cmd1.Fill(db);
            while (db.Rows.Count > 0)
                return "id already exist in the DB press again";
            while (string.IsNullOrWhiteSpace(customer.id) || customer.id.Length != 9 || !(Regex.IsMatch(customer.id, "^[0-9]+$")))
                return "invaild id-please press again";
            int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
            int count = 0, j;
            customer.id = customer.id.PadLeft(9, '0');
            for (j = 0; j < 9; j++)
            {
                int num = Int32.Parse(customer.id.Substring(j, 1)) * id_12_digits[j];
                if (num > 9)
                    num = (num / 10) + (num % 10);
                count += num;
            }
            if (!(count % 10 == 0))
                return "invaild id-please press again";
            while (!(Regex.IsMatch(customer.firstName, "^[a-zA-Z]+$")))
                return "your first name worng";
            while (!(Regex.IsMatch(customer.lastName, "^[a-zA-Z]+$")))
                return "your last name worng";
            while (!(Regex.IsMatch(customer.address, "^[a-zA-Z]+$")))
                return "your address worng";
            while (!(Regex.IsMatch(customer.city, "^[a-zA-Z]+$")))
                return "your city worng";
            while (!(Regex.IsMatch(customer.phone, "^[0-9]+$")))
                return "invaild phone-please press again";
            while (!(Regex.IsMatch(customer.mobile, "^[0-9]+$")))
                return "invaild mobile-please press again";
            int yearp = customer.birthday.Year;
            int monthp = customer.birthday.Month + 100;
            int dayp = customer.birthday.Day + 100;
            string datep = yearp.ToString() + '-' + monthp.ToString().Substring(1, 2) + '-' + dayp.ToString().Substring(1, 2);
            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO Customer(id,firstName,lastName,phone,mobile,city,numOfStreet,Address,birthday)VALUES('" + customer.id + "','" + customer.firstName + "','" + customer.lastName + "','" + customer.phone + "','" + customer.mobile + "','" + customer.city + "','" + customer.numOfStreet + "','" + customer.address + "','" + datep + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
                return "Data-Insert";
            else
                return "Failed";
        }

        [HttpGet]
        [Route("Customer")]
        public string GetCustomer()
        {
            List<Customer> customerList = new List<Customer>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customer", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Customer customer = new Customer();
                    customer.id = Convert.ToString(dt.Rows[i]["id"]);
                    customer.firstName = Convert.ToString(dt.Rows[i]["firstName"]);
                    customer.lastName = Convert.ToString(dt.Rows[i]["lastName"]);
                    customer.phone = Convert.ToString(dt.Rows[i]["phone"]);
                    customer.mobile = Convert.ToString(dt.Rows[i]["mobile"]);
                    customer.city = Convert.ToString(dt.Rows[i]["city"]);
                    customer.numOfStreet = Convert.ToInt32(dt.Rows[i]["numOfStreet"]);
                    customer.address = Convert.ToString(dt.Rows[i]["address"]);
                    customer.birthday = Convert.ToDateTime(dt.Rows[i]["birthday"]);
                    customerList.Add(customer);

                }
            }
            if (customerList.Count > 0)
                return JsonConvert.SerializeObject(customerList);
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
            Customer customer = new Customer();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customer WHERE id='"+ id +"'", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            Response response = new Response();
            con.Close();    
            if (dt.Rows.Count > 0)
            {
                    customer.id = Convert.ToString(dt.Rows[0]["id"]);
                    customer.firstName = Convert.ToString(dt.Rows[0]["firstName"]);
                    customer.lastName = Convert.ToString(dt.Rows[0]["lastName"]);
                    customer.phone = Convert.ToString(dt.Rows[0]["phone"]);
                    customer.mobile = Convert.ToString(dt.Rows[0]["mobile"]);
                    customer.city = Convert.ToString(dt.Rows[0]["city"]);
                    customer.numOfStreet = Convert.ToInt32(dt.Rows[0]["numOfStreet"]);
                    customer.address = Convert.ToString(dt.Rows[0]["address"]);
                    customer.birthday = Convert.ToDateTime(dt.Rows[0]["birthday"]);
            }
            if (customer.id!=null)
                return JsonConvert.SerializeObject(customer);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);

            }

        }

        [HttpGet]
        [Route("not vaccinated")]
        public string NotVaccinated()
        {
            List<Customer> customerList = new List<Customer>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customer WHERE id NOT IN ( SELECT id FROM Vaccination)", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Customer customer = new Customer();
                    customer.id = Convert.ToString(dt.Rows[i]["id"]);
                    customer.firstName = Convert.ToString(dt.Rows[i]["firstName"]);
                    customer.lastName = Convert.ToString(dt.Rows[i]["lastName"]);
                    customer.phone = Convert.ToString(dt.Rows[i]["phone"]);
                    customer.mobile = Convert.ToString(dt.Rows[i]["mobile"]);
                    customer.city = Convert.ToString(dt.Rows[i]["city"]);
                    customer.numOfStreet = Convert.ToInt32(dt.Rows[i]["numOfStreet"]);
                    customer.address = Convert.ToString(dt.Rows[i]["address"]);
                    customer.birthday = Convert.ToDateTime(dt.Rows[i]["birthday"]);
                    customerList.Add(customer);

                }
            }
            if (customerList.Count > 0)
                return "The number of unvaccinated customers:" + JsonConvert.SerializeObject(dt.Rows.Count) + "\n" + JsonConvert.SerializeObject(customerList);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return "The number of unvaccinated customers:" + JsonConvert.SerializeObject(dt.Rows.Count) + JsonConvert.SerializeObject(response);

            }

        }

        [HttpGet]
        [Route("Summary of active patients this month")]
        public IActionResult Calander()
        {
            int temp = 1;
            int s, e, j;
            int[] calander = new int[32];
            string[,] toprint = new string[32, 2];
            for (int i = 0; i < 32; i++) { calander[i] = 0; }
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProjetDb").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT CASE WHEN MONTH(positiveDate)!=MONTH(GETDATE())THEN CONVERT(DATETIME,CONCAT(YEAR(GETDATE()), '-',RIGHT(CAST(100+ MONTH(GETDATE()) AS VARCHAR(3)),2) , '-','01')) ELSE positiveDate END positiveDate, CASE WHEN MONTH(recoveryDate)!=MONTH(GETDATE()) THEN CONVERT(DATETIME,CONCAT(YEAR(GETDATE()), '-',RIGHT(CAST(100+ MONTH(GETDATE()) AS VARCHAR(3)),2) , '-',CASE WHEN MONTH(GETDATE()) in (1,3,5,7,8,10,12) THEN '31' ELSE CASE WHEN MONTH(GETDATE())=2 THEN '28' ELSE '30'END END )) ELSE recoveryDate END recoveryDate FROM Disease where MONTH(positiveDate)<=MONTH(GETDATE()) AND MONTH(recoveryDate)>=MONTH(GETDATE())", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s = Convert.ToDateTime(dt.Rows[i]["positiveDate"]).Day;
                e = Convert.ToDateTime(dt.Rows[i]["recoveryDate"]).Day;
                for (j = s; j <= e; j++)
                {
                    calander[j]++;
                }

            }
            for (int i = 1; i <= 31; i++)
            {
                toprint[i, 0] = "Patients Number:"+calander[i].ToString();
                toprint[i, 1] = temp.ToString() + "-" + DateTime.Now.Month+"-"+DateTime.Now.Year;
                temp++;
            }
            string json = JsonConvert.SerializeObject(toprint);
            return Content(json, "application/json");


        }
    }
}
