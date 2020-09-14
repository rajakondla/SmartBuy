using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class Dispatcher:Entity<Guid>
    {
        public string Name { get; set; }

        public Guid DispatcherGroupId { get; set; }
    }
}
