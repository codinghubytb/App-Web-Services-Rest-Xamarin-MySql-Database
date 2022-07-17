using APIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebApp.Services
{
    public class FunctionsDatabase
    {
        private readonly IConfiguration _configuration;


        public FunctionsDatabase(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public DataTable Request_SqlServer(string query, List<List<object>> listParameters)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("SqlServerDatabase");
            SqlDataReader sqlDataReader;
            using (SqlConnection MyCon = new SqlConnection(sqlDataSource))
            {
                MyCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, MyCon))
                {
                    AddParamaters_SqlServer(myCommand, listParameters);
                    sqlDataReader = myCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    MyCon.Close();
                }
            }

            return table;
        }


        public DataTable Request_MySql(string query, List<List<object>> listParameters)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MySqlDatabase");
            MySqlDataReader sqlDataReader;
            using (MySqlConnection MyCon = new MySqlConnection(sqlDataSource))
            {
                MyCon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, MyCon))
                {
                    AddParamaters_MySql(myCommand, listParameters);
                    sqlDataReader = myCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    MyCon.Close();
                }
            }

            return table;
        }



        public void AddParamaters_SqlServer(SqlCommand command, List<List<object>> listParameters)
        {
            for (int i = 0; i < listParameters.Count; i++)
            {
                command.Parameters.AddWithValue(listParameters[i][0].ToString(), listParameters[i][1]);
            }
        }


        public void AddParamaters_MySql(MySqlCommand command, List<List<object>> listParameters)
        {
            for (int i = 0; i < listParameters.Count; i++)
            {
                command.Parameters.AddWithValue(listParameters[i][0].ToString(), listParameters[i][1]);
            }
        }

    }
}
