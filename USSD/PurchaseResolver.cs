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
        public static string COMMODITY_TYPE_MESSAGE = "Masukkan jenis beras yang ingin anda jual\n 1. Beras\n 2. Gabah Kering Giling\n 3.Gabah Kering Panen";
        public static string STOCK_MESSAGE = "Masukkan dalam KG stok beras anda";
        public static string PRICE_MESSAGE = "Masukkan Harga Jual per KG";
        public static string NEEDED_PERIOD_MESSAGE = "dalam berapa minggu kedepan anda membutuhkan beras";
        //private string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=haketon;Encoding=UNICODE";
        private NpgsqlConnection conn;

        public PurchaseResolver()
        {
            conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING);
        }

        public string GetResponse(string clientRequest)
        {
            string[] requestItems = clientRequest.Split('*');
            int requestItemLength = requestItems.Length;
            string response = null;

            switch (requestItemLength)
            {
                case 1:
                    response = COMMODITY_TYPE_MESSAGE;
                    break;
                case 2:
                    response = STOCK_MESSAGE;
                    break;
                case 3:
                    response = PRICE_MESSAGE;
                    break;
                case 4:
                    response = NEEDED_PERIOD_MESSAGE;
                    break;
                case 5:
                    Order order = InsertOrder(requestItems);
                    response = string.Format("Pesanan Anda: Jenis:{0}\n, Stok:{1}Kg\n, Harga:Rp.{2}\n, Tanggal: {3}\n Terima Kasih", GetCommodityType(order.CommodityType).Name, order.Amount, order.Price, order.Date);
                    break;
            }

            return response;
        }

        private Order InsertOrder(string[] requestItems)
        {

            Order order = new Order();
            
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

            string query = string.Format("INSERT INTO orders(fkuserid, commoditytype, price, amount, ordertype, orderdate, fkmatchingorderid) VALUES({0},{1},{2},{3},'{4}','{5}','{6}')",
                            order.fkUserId, order.CommodityType, order.Price, order.Amount, order.OrderType, order.Date, order.fkMatchingOrderId);
           
            conn.Execute(query, order);

            return order;
        }

        private CommodityType GetCommodityType(long commodityTypeId)
        {
            CommodityType commodityType =
                       conn.Query<CommodityType>(string.Format("SELECT id, name FROM commoditytype WHERE id = {0}", commodityTypeId))
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
