using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Nancy;
using Haketon.Models;
using Haketon.USSD;
using Npgsql;

namespace Haketon
{
    public class DasboardModule : NancyModule
    {
        public DasboardModule()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=root;Database=haketon;Encoding=UNICODE";
            Get["/sunburst"] = parameters => 
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var datas = conn.Query<String>("select string_agg( commoditytype.name || '-' || order_type || ',' || price , '\\n') from orders inner join commoditytype on orders.commoditytype = commoditytype.id;").ToList();
                    var data = datas[0];
                    return View["sunburst",data];
                };
                
            };

            Get["/dashboard"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    var datas = conn.Query<String>("select string_agg( commoditytype.name || '-' || order_type || ',' || price , '\\n') from orders inner join commoditytype on orders.commoditytype = commoditytype.id;").ToList();
                    var data = datas[0];
                    return View["dashboard", data];
                };

            };
        }
    }
}
