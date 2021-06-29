using MA.CustomFacets.Model;
using Sitecore.ListManagement.XConnect.Web.Export;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.CustomMarketingApps.Mappers
{
    class CustomerStatusContactDataReader: IContactDataReader
    {
        public string FacetName
        {
            get
            {
                return "CustomerStatus";
            }
        }

        public string Map(Contact contact)
        {
            string CustomerFacetKey = CustomerStatus.DefaultFacetKey;
            return contact.GetFacet<CustomerStatus>(CustomerFacetKey)?.Status ?? "";
        }
    }
}
