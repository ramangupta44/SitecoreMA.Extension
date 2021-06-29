using MA.CustomMarketingApps.Model;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Abstractions;
using Sitecore.ListManagement.Operations;
using Sitecore.ListManagement.Providers;
using Sitecore.ListManagement.XConnect;
using Sitecore.ListManagement.XConnect.Segmentation;
using Sitecore.ListManagement.XConnect.Web;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using Sitecore.ListManagement.DependencyInjection;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Segments;
using Sitecore.Marketing.Definitions.ContactLists ;
using Sitecore.Data.Managers;

namespace MA.CustomMarketingApps.Controllers
{
    [System.Web.Http.RoutePrefix("sitecore/api/customlists/{listId}/contacts")]
    public class CustomContactsController: ServicesApiController
    {
        private readonly Model.IListSubscriptionsStore<Model.ContactDataModel> _listSubscriptionsStore;
        private readonly BaseLog _log;
        
        public CustomContactsController()
        {
             
            //IContactListProvider clp = new CustomContactListProvider(Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IDefinitionManager<IContactListDefinition>>(), 
            //                                                            Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IDefinitionManager<ISegmentDefinition>>(),
            //                                                            LanguageManager.DefaultLanguage.CultureInfo);
            IContactListProvider clp = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IContactListProvider>();
            var sp = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<ISubscriptionService>();
            var csf = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IContactSourceFactory>();
            var sss = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<ISegmentationService>();
            var cp = new MA.CustomMarketingApps.Model.CustomContactProvider(sss, csf); // create an object of the custom ContactProvider class
            var or = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IListOperationRepository>();
            var lss = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<Model.IListSubscriptionsStore<Model.ContactDataModel>>();
            int batchSize = Sitecore.Configuration.Settings.GetIntSetting("ListManagement.BatchSize", 250);
            this._listSubscriptionsStore = new ListSubscriptionsStore(clp, sp, cp, or, batchSize, csf, sss); // create your custom store
        }

        [System.Web.Http.Route]
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("customaction")]
        public virtual IEnumerable<Model.ContactDataModel> GetEntities(Guid listId, string filter = " ", int pageIndex = 0, int pageSize = 20)
        {
            return this._listSubscriptionsStore.GetSubscribers(listId, filter, pageIndex, pageSize); // your custom store must be used here
        }
    }
}
