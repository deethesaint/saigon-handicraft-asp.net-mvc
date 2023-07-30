using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VuThanhDuong_DA.Models
{
    public static class UserRanking
    {
        private static string bronze = "Đồng";
        private static string silver = "Bạc";
        private static string gold = "Vàng";
        private static string platinum = "Bạch Kim";
        private static string diamond = "Kim Cương";

        public static string BRONZE { get { return bronze; } }
        public static string SILVER { get { return silver; } }
        public static string GOLD { get { return gold; } }
        public static string PLATINUM { get { return platinum; } }
        public static string DIAMOND { get { return diamond; } }
    }
}