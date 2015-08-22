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
        public static string KTP = "Masukkan no KTP anda";
        public static string ADDRESS = "Masukkan alamat anda sesuai KTP";
        private string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=root;Database=haketon;Encoding=UNICODE";
        private NpgsqlConnection conn;

        public RegistrationResolver()
        {

        }
    }
}
