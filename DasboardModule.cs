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
           
            Get["/dashboard"] = parameters =>
            {
                using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
                {
                    var datas = conn.Query<String>("select string_agg( commoditytype.name || '-' || order_type || ',' || price , '\\n') from orders inner join commoditytype on orders.commoditytype = commoditytype.id;").ToList();
                    var data = datas[0];
                    return View["dashboard", data];
                };

            };
        }
    }
}

