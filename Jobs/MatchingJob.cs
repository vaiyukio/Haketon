using Haketon.Models;
using Npgsql;
using Quartz;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.Jobs
{
    public class MatchingJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Run();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        private void Run()
        {
            using (var conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING))
            {
                var orders = conn.Query<Order>("select * from Orders where IsMatchSearched <> true or IsMatchSearched is null").ToList();
                Console.WriteLine("finding matches for " + orders.Count + " orders");
                foreach(var order in orders)
                {
                    var minAmount = 0.9 * order.Amount;
                    var maxAmount = 1.1 * order.Amount;
                    var minPrice = 0.9 * order.Price;
                    var maxPrice = 1.1 * order.Price;
                    var type = order.OrderType == "Purchase" ? "Sell" : "Purchase";
                    var matchedOrders = conn.Query<Order>("select * from Orders where IsMatchSearched = true and Price < @MaxPrice and Price > @MinPrice and Amount < @MaxAmount and Amount > @MinAmount and OrderType = @Type and CommodityType = @CommodityType and fkUserId <> @UserId",
                        new {
                            MinAmount = minAmount,
                            MaxAmount = maxAmount,
                            MinPrice = minPrice,
                            MaxPrice = maxPrice,
                            CommodityType = order.CommodityType,
                            Type = type,
                            UserId = order.fkUserId
                        }).ToList();
                    Console.WriteLine("order " + order.Id + " has "+matchedOrders.Count+" matches");

                    foreach(var matchedOrder in matchedOrders)
                    {
                        String message = "Kami menemukan order yang cocok. {0} {1} {2} sebesar {3} kg dengan harga Rp. {4}";
                        var verb = order.OrderType == "Purchase" ? "menjual" : "membeli";
                        var commodity = conn.Query<String>("select Name from CommodityType where Id = @Id", new { Id = order.CommodityType }).FirstOrDefault();
                        var user = conn.Query<User>("select Name, PhoneNumber from Users where Id = @Id", new { Id = matchedOrder.fkUserId }).FirstOrDefault();
                        message = String.Format(message, user.Name, verb, commodity, matchedOrder.Amount, matchedOrder.Price);
                        conn.Execute("insert into Outboxes(PhoneNumber, Message) values (@PhoneNumber, @Message)",
                            new
                            {
                                PhoneNumber = user.PhoneNumber,
                                Message = message
                            }
                        );
                    }
                    conn.Execute("update Orders set IsMatchSearched = true where Id = @Id", new { Id = order.Id });
                }
            };
        }
    }
}
