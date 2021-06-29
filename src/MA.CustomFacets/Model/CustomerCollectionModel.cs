using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// install Sitecore.XConnect.Collection.Model nuget package.

namespace MA.CustomFacets.Model
{
    public static class CustomerCollectionModel
    {
        public static XdbModel Model { get; } = CreateModel();

        private static XdbModel CreateModel()
        {

            XdbModelBuilder builder = new XdbModelBuilder("CustomFacets.Xconnect.CustomerCollectionModel", new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Contact, CustomerStatus>(CustomerStatus.DefaultFacetKey);
            return builder.BuildModel();
        }
    }
}
