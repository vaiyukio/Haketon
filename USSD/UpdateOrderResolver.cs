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
            
            string query = "UPDATE orders SET ";

            if (commodityTypeId == 0 && amount == 0 && price == 0 && availabilityWeek == 0)
                return "Anda tidak melakukan update order";
            else if (amount == 0 && price == 0 && availabilityWeek == 0)
                query += string.Format("commoditytype = {0} ", commodityTypeId);
            else if(price == 0 && availabilityWeek == 0)
                query += string.Format("commoditytype = {0},  amount = {1} ", commodityTypeId, amount);
            else if(availabilityWeek == 0)
                query += string.Format("commoditytype = {0},  amount = {1},  price = {2} ", commodityTypeId, amount, price);
            else
                query += string.Format("commoditytype = {0},  amount = {1},  price = {2},  orderdate = '{3}' ", commodityTypeId, amount, price, DateTime.Now);

            query += string.Format("WHERE id = {0}", orderId);
            conn.Execute(query);
            
            return string.Format("Data Update Anda:\n Jenis: {0}, Stok: {1}, Harga: {2}, Ketersediaan: {3}", GetCommodityType(commodityTypeId).Name, amount, price, DateTime.Now);
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
