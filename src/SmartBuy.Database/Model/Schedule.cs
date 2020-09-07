using SmartBuy.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedDatabase.Model
{
    public class Schedule
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
