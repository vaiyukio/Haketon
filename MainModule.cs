using System;
using System.Collections.Generic;
using Nancy;
using Haketon.Models;

namespace Haketon
{
    public class MainModule : NancyModule
    {
        private bool isRegistered = true;

        public MainModule()
        {
            Get["/ussd/"] = x =>
            {
                var phoneNumber = this.Request.Query["n"];
                var message = this.Request.Query["m"];
                
                //TODO Check phone number
                if (!isRegistered)
                {
                    return "0. Register";
                }

                if (string.IsNullOrEmpty(message))
                {
                    return "1. Input Pemebelian Beras\n 2. Input Penjualan Beras\n 3. Update Pembelian Beras\n 4.Update Penjualan Beras";
                }

                return USSDResolver(message);
            };
        }

        private string USSDResolver(string message)
        {
            string[] messages = message.Split('*');
            string type = messages[0];
            string result = null;

            switch (type)
            {
                case "1":
                    result = this.PurchaseResolver(message);
                    break;
                case "2":
                    result = this.SellResolver(message);
                    break;
                case "3":
                    result = null;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private string SellResolver(string message)
        {
            USSD.SellResolver sellResolver = new USSD.SellResolver();
            return sellResolver.GetMessage(message);
        }

        private string PurchaseResolver(string message)
        {
            USSD.PurchaseResolver purchaseResolver = new USSD.PurchaseResolver();
            return purchaseResolver.GetMessage(message);
        }
    }
}
