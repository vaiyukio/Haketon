using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haketon.USSD
{
    public class SellResolver
    {
        public static string COMMODITY_TYPE = "Masukkan jenis beras yang ingin anda jual";
        public static string STOCK = "Masukkan dalam KG stok beras anda";
        public static string PRICE = "Masukkan Harga Jual per KG";
        public static string READY_PERIOD = "dalam berapa minggu kedepan beras anda siap";
        public static string THANKS = "Terima Kasih";

        public SellResolver()
        {
        }

        public string GetMessage(string message)
        {
            string[] messages = message.Split('*');
            int msgLength = messages.Length;
            string result = "";
            switch (msgLength)
            {
                case 1:
                    result = COMMODITY_TYPE;
                    break;
                case 2:
                    result = STOCK;
                    break;
                case 3:
                    result = PRICE;
                    break;
                case 4:
                    result = READY_PERIOD;
                    break;
                case 5:
                    result = THANKS;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        public void GetData(string message)
        {
            string[] data = message.Split('*');

        }
    }
}
