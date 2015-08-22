using System;
using System.Collections.Generic;
using Nancy;
using Haketon.Models;
using Haketon.USSD;

namespace Haketon
{
    public class MainModule : NancyModule
    {
        private bool isRegistered = false;
        private string phoneNumber = null;
        private RegistrationResolver registrationResolver;
        private PurchaseResolver purchaseResolver;
        private SellResolver sellResolver;
        private UpdateOrderResolver updateOrderResolver;

        public MainModule()
        {
            Get["/ussd/"] = parameters =>
            {
                registrationResolver = new RegistrationResolver();
               
                string response = null;
                phoneNumber = this.Request.Query["n"];
                string clientRequest = this.Request.Query["m"];
                isRegistered = registrationResolver.AuthorizeUser(phoneNumber);

                if (!isRegistered && (!clientRequest.StartsWith("0") || string.IsNullOrEmpty(clientRequest)))
                    return ApplicationConfig.REGISTER_MESSAGE;

                else if (isRegistered && string.IsNullOrEmpty(clientRequest))
                    return ApplicationConfig.MAIN_MESSAGE;

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
                    result = UpdateOrderResolver(clientRequest);
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
            sellResolver = new SellResolver();
            return sellResolver.GetResponse(clientRequest);
        }

        private string PurchaseResolver(string clientRequest)
        {
            purchaseResolver = new PurchaseResolver();
            return purchaseResolver.GetResponse(clientRequest);
        }

        private string UpdateOrderResolver(string clientRequest)
        {
            updateOrderResolver = new UpdateOrderResolver();
            User user = registrationResolver.GetUser(phoneNumber);
            return updateOrderResolver.GetResponse(clientRequest, user);
        }

        private string UpdatePurchaseResolver(string clientRequest)
        {
            return "";
        }
    }
}
