using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using Haketon.Models;

namespace Haketon.USSD
{
    public class TransactionResolver
    {
        public static string USER_ORDER = "Pilih Order yang Dimaksud:";
        public static string USER_MATCHING_ORDER = "Pilih Order yang Match:";
        public static string STOCK = "Terima Kasih. Order anda telah selesai";
        private NpgsqlConnection conn;

        public TransactionResolver()
        {
            conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING);
        }

        public string GetResponse(string clientRequest, User user)
        {
            string[] requestItems = clientRequest.Split('*');
            int requestItemLength = requestItems.Length;
            string response = null;
            switch (requestItemLength)
            {
                case 1:
                    String userOrderString = GetUserOrder(user);
                    response = USER_ORDER + userOrderString;
                    break;
                case 2:
                    String orderMatching = GetMatcingOder(requestItems[1]);
                    response = USER_MATCHING_ORDER + orderMatching;
                    break;
                case 3:
                    updateMatchingOrderId(int.Parse(requestItems[1]), int.Parse(requestItems[2]));
                    response = STOCK;
                    break;
                case 5:
                    //Order order = InsertOrder(requestItems, user);
                   // response = string.Format("Data Jualan Anda: Jenis:{0}\n, Stok:{1}Kg\n, Harga:Rp.{2}\n, Tanggal Tersedia: {3}\n Terima Kasih", GetCommodityType(order.CommodityType).Name, order.Amount, order.Price, order.Date);
                    break;
            }

            return response;
        }

        public String GetUserOrder(User user)
        {
            String response = "";
            var orders = conn.Query<Order>("select * from Orders where fkuserid = @UserId and id in (select first_id from orders_matches)", new{ UserId = user.Id}).ToList();
            foreach (Order orderItem in orders)
            {
                response += string.Format("{0}. {1}, {2}, {3}Kg Rp.{4}, {5} Minggu\n", orderItem.Id, orderItem.OrderType,
                    GetCommodityType(orderItem.CommodityType).Name, orderItem.Amount, orderItem.Price, orderItem.Date);
            }

            return response;
        }

        private CommodityType GetCommodityType(long commodityTypeId)
        {
            CommodityType commodityType =
                       conn.Query<CommodityType>(string.Format("SELECT id, name FROM commoditytype WHERE id = {0}", commodityTypeId))
                       .FirstOrDefault();

            return commodityType;
        }

        public String GetMatcingOder(String orderId)
        {
            int orderIdInt = Int32.Parse(orderId);
            String response = "";
            var orders = conn.Query<Order>("select * from Orders inner join orders_matches on orders.id = orders_matches.second_id where first_id = @OrderId", new { OrderId = orderIdInt }).ToList();
            foreach (Order orderItem in orders)
            {
                response += string.Format("{0}. {1}, {2}, {3}Kg Rp.{4}, {5} Minggu\n", orderItem.Id, orderItem.OrderType,
                    GetCommodityType(orderItem.CommodityType).Name, orderItem.Amount, orderItem.Price, orderItem.Date);
            }

            return response;
        }

        public void updateMatchingOrderId(int firstId, int secondId)
        {
            conn.Execute("update orders set fkmatchingorderid = @FirstId where id = @SecondId", new { FirstId = firstId, SecondId = secondId });
            conn.Execute("update orders set fkmatchingorderid = @FirstId where id = @SecondId", new { FirstId = secondId, SecondId = firstId });
        }
    }
}
