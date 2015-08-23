using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Npgsql;
using Dapper;
using Haketon.Models;
using Newtonsoft.Json;

namespace Haketon
{
    public class DashboardModule : NancyModule
    {
        public DashboardModule()
        {
            const string sunburstQuery = "select  string_agg ( commoditytype.name || '-' || ordertype || ',' || amount , '\\n' )  from orders inner join commoditytype on orders.commoditytype = commoditytype.id";
            Get["/dashboard"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<String>(sunburstQuery).ToList();
                    var data = datas[0];
                    return View["dashboard",data];
                }
            };
            Get["/sunburst"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<String>(sunburstQuery).ToList();
                    var data = datas[0];
                    return View["sunburst", data];
                };
            };

            Get["/verify"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<Registration>("select * from Registrations where IsVerified <> true or IsVerified is null").ToList();
                    return View["verifylist", datas];
                };
            };

            Get["/verify/{id}"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var data = conn.Query<Registration>("select * from Registrations where (IsVerified <> true or IsVerified is null) and Id = @Id", new { Id = parameters.id }).FirstOrDefault();
                    return View["verify", data];
                };
            };


            Post["/verify"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    dynamic form = Request.Form;
                    conn.Execute("insert into Users(Name, KtpNumber, PhoneNumber, Address, Longitude, Latitude) values (@Name, @KtpNumber, @PhoneNumber, @Address, @Longitude, @Latitude)",
                        new
                        {
                            Name = form.Name,
                            KtpNumber = form.KtpNumber,
                            PhoneNumber = form.PhoneNumber,
                            Address = form.Address,
                            Longitude = form.Longitude,
                            Latitude = form.Latitude,
                        }
                    );
                    conn.Execute("update Registrations set IsVerified = true where Id = @Id", new { Id = form.id });
                    return Response.AsRedirect("/verify");
                };
            };

            Get["/orders/{start}"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var data = conn.Query<OrderUser>("select * from Orders_Users where Id > @Start", new { Start = parameters.start }).ToList();
                    return JsonConvert.SerializeObject(data);
                };
            };

            Get["/sunburstdata"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<String>(sunburstQuery).ToList();
                    return datas[0];
                };
            };

        }
    }
}
