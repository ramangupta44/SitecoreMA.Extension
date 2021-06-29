using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// install Sitecore.XConnect nuget package.

namespace MA.CustomFacets.Model
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class CustomerStatus : Facet
    {
        public string Status { get; set; }
        public const string DefaultFacetKey = "CustomerStatus";
    }
}
