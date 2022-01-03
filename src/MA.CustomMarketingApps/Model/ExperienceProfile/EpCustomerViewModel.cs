using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPExpressTab.Data;
using EPExpressTab.Repositories;
using MA.CustomFacets.Model;

namespace MA.CustomMarketingApps.Model.ExperienceProfile
{
    public class EpCustomerViewModel: EpExpressViewModel
    {
        public override string Heading => "To see custom facets values related to Customer";
        public override string TabLabel => "Customer Details";
        public override object GetModel(Guid customerstatus)
        {
            try
            {
                Sitecore.XConnect.Contact contact = EPRepository.GetContact(customerstatus, CustomerStatus.DefaultFacetKey);
                var custoStatus = contact.GetFacet<CustomerStatus>(CustomerStatus.DefaultFacetKey);
                return new EpCustomerModel
                {
                    customer_Status = custoStatus,
                };
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public override string GetFullViewPath(object model)
        {
            return "/Views/ExperienceProfile/EpBennettsCustomer.cshtml";
        }
    }
}
