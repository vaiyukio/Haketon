using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Npgsql;
using Dapper;
using Haketon.Models;

namespace Haketon
{
    public class DashboardModule : NancyModule
    {
        public DashboardModule()
        {
            Get["/dashboard"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<String>("select string_agg( commoditytype.name || '-' || order_type || ',' || price , '\\n') from orders inner join commoditytype on orders.commoditytype = commoditytype.id;").ToList();
                    var data = datas[0];
                    return View["dashboard", data];
                };

            };
            Get["/sunburst"] = parameters =>
            {
                return View["sunburst", "bro"];
            };

            Get["/verify"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<Registration>("select * from Registrations where IsVerified <> true").ToList();
                    return View["verifylist", datas];
                };
            };

            Get["/verify/{id}"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var data = conn.Query<Registration>("select * from Registrations where IsVerified <> true and Id = @Id", new { Id = parameters.id }).FirstOrDefault();
                    return View["verify", data];
                };
            };

        }
    }
}
