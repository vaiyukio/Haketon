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
        private RegistrationResolver registrationResolver;
        private PurchaseResolver purchaseResolver;
        private SellResolver sellResolver;
        private UpdateOrderResolver updateOrderResolver;
        private User user;

        public MainModule()
        {
            Get["/ussd/"] = parameters =>
            {
                registrationResolver = new RegistrationResolver();
               
                string response = null;
                string phoneNumber = this.Request.Query["n"];
                string clientRequest = this.Request.Query["m"];
                isRegistered = registrationResolver.AuthorizeUser(phoneNumber);

                if (!isRegistered && (!clientRequest.StartsWith("0") || string.IsNullOrEmpty(clientRequest)))
                    return ApplicationConfig.REGISTER_MESSAGE;

                else if (isRegistered && string.IsNullOrEmpty(clientRequest))
                    return ApplicationConfig.MAIN_MESSAGE;

                user = registrationResolver.GetUser(phoneNumber);
                response = USSDResolver(clientRequest, user);
                return response;
            };
        }

        private string USSDResolver(string clientRequest, User user)
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
                    result = PurchaseResolver(clientRequest, user);
                    break;
                case "2":
                    result = SellResolver(clientRequest, user);
                    break;
                case "3":
                    result = UpdateOrderResolver(clientRequest, user);
                    break;
                case "4":
                    result = "";
                    break;
            }

            return result;
        }

        private string RegistrationResolver(string clientRequest)
        {
            return registrationResolver.GetResponse(clientRequest);
        }

        private string SellResolver(string clientRequest, User user)
        {
            sellResolver = new SellResolver();
            return sellResolver.GetResponse(clientRequest, user);
        }

        private string PurchaseResolver(string clientRequest, User user)
        {
            purchaseResolver = new PurchaseResolver();
            return purchaseResolver.GetResponse(clientRequest, user);
        }

        private string UpdateOrderResolver(string clientRequest, User user)
        {
            updateOrderResolver = new UpdateOrderResolver();
            return updateOrderResolver.GetResponse(clientRequest, user);
        }
    }
}
