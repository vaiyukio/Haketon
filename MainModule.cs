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
        private RegistrationResolver registrationResolver;
        private PurchaseResolver purchaseResolver;
        private SellResolver sellResolver;

        public MainModule()
        {
            Get["/ussd"] = parameters =>
            {
                registrationResolver = new RegistrationResolver();
                
                string response = null;
                string phoneNumber = this.Request.Query["n"];
                string clientRequest = this.Request.Query["m"];

                isRegistered = false; //registrationResolver.AuthorizeUser(phoneNumber);

                if (!isRegistered && clientRequest == null)
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
                case "0":
                    result = RegistrationResolver(clientRequest);
                    break;
                case "1":
                    result = PurchaseResolver(clientRequest);
                    break;
                case "2":
                    result = SellResolver(clientRequest);
                    break;
                case "3":
                    result = UpdateSellResolver(clientRequest);
                    break;
                case "4":
                    result = UpdatePurchaseResolver(clientRequest);
                    break;
            }

            return result;
        }

        private string RegistrationResolver(string clientRequest)
        {
            return registrationResolver.GetResponse(clientRequest);
        }

        private string SellResolver(string clientRequest)
        {
            sellResolver = new USSD.SellResolver();
            return sellResolver.GetMessage(clientRequest);
        }

        private string PurchaseResolver(string clientRequest)
        {
            purchaseResolver = new USSD.PurchaseResolver();
            return purchaseResolver.GetResponse(clientRequest);
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
