using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project
{
    public class DatabaseSetting
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CustomersCollectionName { get; set; } = null!;
    }
}
