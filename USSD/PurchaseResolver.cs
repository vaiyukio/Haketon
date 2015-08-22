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
        private string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=root;Database=haketon;Encoding=UNICODE";
        //private string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=haketon;Encoding=UNICODE";

        public PurchaseResolver(){}

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
                    Order order = GetData(requestItems);
                    response = string.Format("Pesanan Anda: Jenis Beras:{0}\n, Stok:{1}\n, Harga:{2}\n, Tanggal: {3}\n Terima Kasih", 0, 0, 0, 0);
                    break;
            }

            return response;
        }

        public Order GetData(string[] requestItems)
        {
            Order order = new Order();
            long commodityTypeId = long.Parse(requestItems[1]);
            long amount = long.Parse(requestItems[2]);
            long price = long.Parse(requestItems[3]);
            int dateToGo = int.Parse(requestItems[4]);

            using (var conn = new NpgsqlConnection(connectionString))
            {
                var commodityType = conn.Query<CommodityType>(string.Format("SELECT CommodityName FROM CommodityType WHERE Id = {0}", commodityTypeId))
                    .FirstOrDefault();

                order.CommodityType = commodityTypeId;
                order.Price = price;
            }

            return order;
        }

        private DateTime CalculateDate()
        {
            return new DateTime();
        }
    }
}
