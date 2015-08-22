using System;
using System.Collections.Generic;
using Nancy;
using Haketon.Models;
using Haketon.USSD;

namespace Haketon
{
    public class MainModule : NancyModule
    {
        private bool isRegistered = true;

        public MainModule()
        {
            Get["/ussd"] = parameters =>
            {
                string response = null;
                string phoneNumber = this.Request.Query["n"];
                string clientRequest = this.Request.Query["m"];

                if (!isRegistered)
                    return ApplicationConfig.REGISTER_MESSAGE;

                if (string.IsNullOrEmpty(clientRequest))
                    return ApplicationConfig.MAIN_MESSAGE ;

                response = USSDResolver(clientRequest);
                return response;
            };
        }

        private string USSDResolver(string clientRequest)
        {
            string[] requestItems = clientRequest.Split('*');
            string type = requestItems[0];
            string result = null;

            switch (type)
            {
                case "1":
                    result = this.PurchaseResolver(clientRequest);
                    break;
                case "2":
                    result = this.SellResolver(clientRequest);
                    break;
                case "3":
                    result = this.UpdateSellResolver(clientRequest);
                    break;
                case "4":
                    result = this.UpdatePurchaseResolver(clientRequest);
                    break;
            }

            return result;
        }

        private string SellResolver(string clientRequest)
        {
            SellResolver sellResolver = new USSD.SellResolver();
            return sellResolver.GetMessage(clientRequest);
        }

        private string PurchaseResolver(string clientRequest)
        {
            PurchaseResolver purchaseResolver = new USSD.PurchaseResolver();
            return purchaseResolver.GetMessage(clientRequest);
        }

        private string UpdateSellResolver(string clientRequest)
        {
            return "";
        }

        private string UpdatePurchaseResolver(string clientRequest)
        {
            return "";
        }
    }
}
