using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using Haketon.Models;


namespace Haketon.USSD
{
    public class UpdateOrderResolver
    {
        private static string UPDATE_AMOUNT_MESSAGE = "Masukkan update jumlah dalam KG (0 jika tidak ada perubahan)";
        private static string UPDATE_PRICE_MESSAGE = "Masukkan update harga per KG (0 jika tidak ada perubahan)";
        private static string UPDATE_AVAILABILITY_WEEK_MESSAGE = "masukkan update minggu ketersediaan (0 jika tidak ada perubahan)";

        private NpgsqlConnection conn;

        public UpdateOrderResolver()
        {
            conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING);
        }

        public string GetResponse(string clientRequest, User user)
        {
            string response = "";
            string[] requestItems = clientRequest.Split('*');
            int requestItemLength = requestItems.Length;

            switch (requestItemLength)
            {
                case 1:
                    response = GetUpdateListResponse(clientRequest, user);
                    break;
                case 2:
                    response = GetCommodityTypeResponse(clientRequest);
                    break;
                case 3:
                    response = UPDATE_AMOUNT_MESSAGE;
                    break;
                case 4:
                    response = UPDATE_PRICE_MESSAGE;
                    break;
                case 5:
                    response = UPDATE_AVAILABILITY_WEEK_MESSAGE;
                    break;
                case 6:
                    response = UpdateOrder(clientRequest);
                    break;
            }

            return response;
        }


        private string UpdateOrder(string clientRequest)
        {
            string[] requestItems = clientRequest.Split('*');
           
            long orderId = long.Parse(requestItems[1]);
            long commodityTypeId = long.Parse(requestItems[2]);
            long amount = long.Parse(requestItems[3]);
            long price = long.Parse(requestItems[4]);
            int availabilityWeek = int.Parse(requestItems[5]);

            Dictionary<string, long> fields = new Dictionary<string, long>()
             { 
                {"commoditytype", commodityTypeId},
                {"amount", amount},
                {"price", price},
                {"orderdate", availabilityWeek}
             };
            
            string query = "UPDATE orders SET ";

            foreach (KeyValuePair<string, long> field in fields)
            {
                if (field.Value > 0)
                    query += string.Format("{0} = {1} ", field.Key, field.Value);
            }

            query += string.Format("WHERE id = {0}", orderId);
            conn.Execute(query);

            Order order = conn.Query<Order>(string.Format("SELECT commoditytype, price, amount, orderdate FROM orders WHERE id ={0}", orderId)).FirstOrDefault();

            return string.Format("Data Update Anda:\n Jenis: {0}, Stok: {1}, Harga: {2}, Ketersediaan: {3}", GetCommodityType(order.CommodityType).Name, order.Amount, order.Price, order.Date);
        }

        private string GetCommodityTypeResponse(string clientRequest)
        {
            string response = "";
            List<CommodityType> commodityTypes = conn.Query<CommodityType>("SELECT id, name FROM commoditytype").ToList<CommodityType>();
            foreach (CommodityType commodityType in commodityTypes)
            {
                response += string.Format("{0}. {1}", commodityType.Id, commodityType.Name);
            }

            return response;
        }

        private string GetUpdateListResponse(string clientRequest, User user)
        {
            string response = "";

            List<Order> orders = conn.Query<Order>(string.Format("SELECT id,fkuserid,commoditytype,amount,price,orderdate,fkmatchingorderid,ordertype FROM orders WHERE fkuserid = {0}", user.Id))
                .OrderBy(e => e.Id)
                .ToList<Order>();
   
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

    }
}
