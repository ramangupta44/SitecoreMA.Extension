using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.CustomMarketingApps.Model
{
    public class MarketingPreference
    {
        public Guid MarketingCategoryId { get; set; }

        public Guid ManagerRootId { get; set; }

        public bool? Preference { get; set; }

        public DateTime DateTime { get; set; }
    }
}
