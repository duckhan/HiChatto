using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Server
{
    public static class Statistics
    {
        static Statistics()
        {
            totalOnlineUser = 0;
            newUserID = 10000;
            newGroupID = 10000;
        }
        private static object obj = new object();
        private static int totalOnlineUser;
        public static int TotalOnlineUser
        {
            get { return totalOnlineUser; }
            set
            {
                lock (obj)
                {
                    totalOnlineUser = value;
                }
            }
        }
        private static int newUserID;
        public static int NewUserID
        {
            get
            {
                lock (obj)
                {
                    return newUserID++;
                }
            }
        }
        private static int newGroupID;
        public static int NewGroupID
        {
            get
            {
                lock (obj)
                {
                    return newGroupID++;
                }
            }
        }

    }
}
