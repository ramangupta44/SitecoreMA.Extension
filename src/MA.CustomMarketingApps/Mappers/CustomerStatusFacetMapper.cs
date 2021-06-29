using MA.CustomFacets.Model;
using Sitecore.Diagnostics;
using Sitecore.ListManagement.Import;
using Sitecore.ListManagement.XConnect.Web.Import;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Import Sitecore.ListManagement.XConnect.Web
namespace MA.CustomMarketingApps.Mappers
{
    class CustomerStatusFacetMapper: IFacetMapper
    {
        private const string FacetMapperPrefix = "CustomerStatus_";

        public CustomerStatusFacetMapper() : this("CustomerStatus")
        {
        }
        public CustomerStatusFacetMapper(string facetName)
        {
            Assert.ArgumentNotNull(facetName, nameof(facetName));

            this.FacetName = facetName;
        }
        public string FacetName { get; }

        public MappingResult Map(string facetKey, Facet facet, ContactMappingInfo mappings, string[] data)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry("CustomerFacetMapperCalled facetKey:" + facetKey + " data" + string.Join("; ", data));
            }

            if (facetKey != this.FacetName)
            {
                return new NoMatch(facetKey);
            }

            CustomerStatus customeruser = new CustomerStatus();
            string customerStatus = mappings.GetValue(FacetMapperPrefix+nameof(customeruser.Status), data);
            Log.Info("Facet Mapper: " + FacetMapperPrefix + nameof(customeruser.Status), this);
            if (!string.IsNullOrEmpty(customerStatus)) { customeruser.Status = customerStatus; }
            //var _facet = facet as CustomerStatus ?? new CustomerStatus(preferredAddress, "Preferred");
            return (MappingResult)new FacetMapped(facetKey, (Facet)customeruser);

        }
    }
}
