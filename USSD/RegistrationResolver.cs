using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using Haketon.Models;

namespace Haketon.USSD
{
    public class RegistrationResolver
    {
        public static string INPUT_KTP_MESSAGE = "Masukkan no KTP anda";
        public static string INPUT_ADDRESS_MESSAGE = "Masukkan alamat anda sesuai KTP";
        private string userPhoneNumber = null;
        private NpgsqlConnection conn;

        public RegistrationResolver()
        {
            conn = new NpgsqlConnection(ApplicationConfig.CONNECTION_STRING);
        }

        public bool AuthorizeUser(string phoneNumber)
        {
            userPhoneNumber = phoneNumber;
            User user = conn.Query<User>(string.Format("SELECT id, name, ktpnumber, phonenumber, address, longitude, latitude FROM users WHERE phonenumber = '{0}'", phoneNumber))
                .FirstOrDefault();
                 
            if (user == null)
                return false;

            return true;
        }

        public string GetResponse(string clientRequest)
        {
            string[] requestItems = clientRequest.Split('*');
            int requestItemLength = requestItems.Length;
            string response = null;

            switch (requestItemLength)
            {
                case 1:
                    response = INPUT_KTP_MESSAGE;
                    break;
                case 2:
                    response = INPUT_ADDRESS_MESSAGE;
                    break;
                case 3:
                    Registration registration = InsertRegistration(requestItems);
                    User user = InsertUser(registration);
                    response = string.Format("Data Anda: No.HP: {0}\n No.KTP: {1}\n Alamat: {2}", user.PhoneNumber, user.KtpNumber, user.Address);
                    break;
            }

            return response;
        }

        public User GetUser(string phoneNumber)
        {
            User user = new User();
            string query = string.Format("SELECT id,name,ktpnumber,phonenumber,address,longitude,latitude FROM users WHERE phonenumber = '{0}'", phoneNumber);
            user = conn.Query<User>(query).FirstOrDefault();
            return user;
        }

        private Registration InsertRegistration(string[] requestItems)
        {
            Registration registration = new Registration();
            registration.KtpNumber = requestItems[1];
            registration.PhoneNumber = userPhoneNumber;
            registration.Address = requestItems[2];
            registration.IsVerified = true;
            string query = string.Format("INSERT INTO registrations(ktpnumber, phonenumber, address) VALUES('{0}','{1}','{2}')", registration.KtpNumber, registration.PhoneNumber, registration.Address);
            
            conn.Execute(query);
            return registration;
        }

        private User InsertUser(Registration registration)
        {
            User user = new User();
            user.KtpNumber = registration.KtpNumber;
            user.PhoneNumber = registration.PhoneNumber;
            user.Address = registration.Address;
            string query = string.Format("INSERT INTO users(ktpnumber, phonenumber, address) VALUES('{0}','{1}','{2}')", user.KtpNumber, user.PhoneNumber, user.Address);

            conn.Execute(query);
            return user;
        }
    }
}
