using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName From dbo.Department";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand mycommand = new SqlCommand(query, myCon))
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
        public JsonResult Post(Department dept)
        {
            string query = @"Insert into dbo.Department values (@DepartmentName)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentName", dept.DepartmentName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult("Added Successfully");

        }

        [HttpPut]
        public JsonResult Put(Department dept)
        {
            string query = @"Update dbo.Department set DepartmentName = @DepartmentName where DepartmentId = @DepartmentId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentId", dept.DepartmentId);
                    mycommand.Parameters.AddWithValue("@DepartmentName", dept.DepartmentName);
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
            string query = @"Delete from dbo.Department where DepartmentId = @DepartmentId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Connstring");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }


            }

            return new JsonResult("Deleted Successfully");

        }

    }
}
