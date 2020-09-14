using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class Terminal : Entity<Guid>
    {
        public string Name { get; set; }
    }
}
