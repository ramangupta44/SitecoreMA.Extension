using MA.CustomFacets.Model;
using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Segmentation.Predicates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
// install Sitecore.XConnect, Sitecore.Framework.Rules.Abstractions,Sitecore.XConnect.Segmentation.Predicates nuget packages
// add reference to MA.CustomFacets project

namespace MA.CustomRules
{
    public class CheckCustomerStatus : ICondition, IMappableRuleEntity, IContactSearchQueryFactory
    {
        public StringOperationType Comparison { get; set; }
        public string CustomerStatus { get; set; }
        public Expression<Func<Contact, bool>> CreateContactSearchQuery(IContactSearchQueryContext context)
        {
            return (Expression<Func<Contact, bool>>)(contact => this.Comparison.Evaluate(contact.GetFacet<CustomerFacets>(CustomerFacets.DefaultFacetKey).CustomerStatus, this.CustomerStatus));
        }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var customerStatus = context.Fact<Contact>((string)null).GetFacet<CustomerFacets>(CustomerFacets.DefaultFacetKey).CustomerStatus;
            if (this.Comparison.Evaluate(customerStatus, this.CustomerStatus))
            {
                return true;
            }
            else
            { return false; }
        }
    }
}
