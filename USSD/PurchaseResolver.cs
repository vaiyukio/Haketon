using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using Haketon.Models;

namespace Haketon.USSD
{
    public class PurchaseResolver
    {
        public static string COMMODITY_TYPE = "Masukkan jenis beras yang ingin anda jual";
        public static string STOCK = "Masukkan dalam KG stok beras anda";
        public static string PRICE = "Masukkan Harga Jual per KG";
        public static string READY_PERIOD = "dalam berapa minggu kedepan anda membutuhkan beras";
        public static string FINISH = "Terima Kasih";
        
        //private string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=haketon;Encoding=UNICODE";
        private NpgsqlConnection conn;

        public PurchaseResolver()
        {
            conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING);
        }

        public string GetMessage(string clientRequest)
        {
            string[] requestItems = clientRequest.Split('*');
            int requestItemLength = requestItems.Length;
            string response = null;

            switch (requestItemLength)
            {
                case 1:
                    response = COMMODITY_TYPE;
                    break;
                case 2:
                    response = STOCK;
                    break;
                case 3:
                    response = PRICE;
                    break;
                case 4:
                    response = READY_PERIOD;
                    break;
                case 5:
                    Order order = InsertOrder(requestItems);
                    response = string.Format("Pesanan Anda: Jenis Beras:{0}\n, Stok:{1}\n, Harga:{2}\n, Tanggal: {3}\n Terima Kasih", GetCommodityType(order.CommodityType).Name, order.Amount, order.Price, order.Date);
                    break;
            }

            return response;
        }

        private Order InsertOrder(string[] requestItems)
        {

            Order order = new Order();
            string query = "INSERT INTO Order(fkUserId, CommodityType, Price,Amount, OrderType, Date, fkMatchingOrderId) VALUES(@fkUserId, @CommodityType, @Price, @Amount, @OrderType, @Date, @fkMatchingOrderId)";
           
            long commodityTypeId = long.Parse(requestItems[1]);
            long amount = long.Parse(requestItems[2]);
            long price = long.Parse(requestItems[3]);
            int dateToGo = int.Parse(requestItems[4]);

            order.fkUserId = 1;
            order.CommodityType = commodityTypeId;
            order.Price = price;
            order.Amount = amount;
            order.OrderType = "Purchase";
            order.Date = CalculateDate(dateToGo);
            order.fkMatchingOrderId = 0;
           
            conn.Execute(query, order);

            return order;
        }

        private CommodityType GetCommodityType(long commodityTypeId)
        {
            CommodityType commodityType =
                       conn.Query<CommodityType>(string.Format("SELECT Id, Name FROM CommodityType WHERE Id = {0}", commodityTypeId))
                       .FirstOrDefault();

            return commodityType;
        }

        private DateTime CalculateDate(int dateToGo)
        {
            DateTime currentDate = DateTime.Now;

            return currentDate;
        }
    }
}
