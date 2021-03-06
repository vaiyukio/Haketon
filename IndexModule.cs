﻿using Nancy;
using Npgsql;
using Dapper;
using System.Data;
using System.Linq;
using Haketon.Models;

namespace Haketon
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=root;Database=haketon;Encoding=UNICODE";

            Get["/"] = parameters =>
            {
                return View["index"];
            };


            Get["/test"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var tests = conn.Query<Registration>("SELECT Id, KtpNumber, PhoneNumber, Address FROM Registration").ToList();
                    var anu = tests.Count();
                };
                return View["index"];
            };
        }
    }
}
