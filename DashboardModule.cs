using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Npgsql;
using Dapper;

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
                return View["verifylist"];
            };

        }
    }
}
