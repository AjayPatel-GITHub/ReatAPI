using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, convert(varchar(10),DateOfJoining,120) as  DateOfJoining, 
                            PhotoFileName  From dbo.Employee";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"Insert into dbo.Employee 
                            (EmployeeName,Department,DateOfJoining,PhotoFileName)
                            values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    mycommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult("Added Successfully");

        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"Update dbo.Employee set 
                             EmployeeName = @EmployeeName,
                             Department = @Department,
                             DateOfJoining = @DateOfJoining,
                             PhotoFileName = @PhotoFileName
                            where EmployeeId = @EmployeeId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    mycommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult("Updated Successfully");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"Delete from dbo.Employee where EmployeeId = @EmployeeId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult("Deleted Successfully");

        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                //Save Profile
                var httpRequest = Request.Form;
                var postFile = httpRequest.Files[0];
                string filename = postFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath,FileMode.Create))
                {
                    postFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

    }
}
