using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Haketon
{
    public static class ApplicationConfig
    {
        public static string MAIN_MESSAGE = "1. Input Pemebelian Beras\n 2. Input Penjualan Beras\n 3. Update Order Sebelumnya\n 4.Pilih Transaksi";
        public static string REGISTER_MESSAGE = "0. Register";
        public static string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
