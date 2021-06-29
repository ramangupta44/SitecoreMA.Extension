using Sitecore;
using Sitecore.ListManagement;
using Sitecore.ListManagement.Providers;
using Sitecore.ListManagement.XConnect.Web;
using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ListManagement.Exceptions;
using Sitecore.XConnect.Collection.Model;
using MA.CustomFacets.Model;
using Sitecore.ListManagement.XConnect.Sources;
using Sitecore.ListManagement.XConnect;
using Sitecore.ListManagement.XConnect.Segmentation;
using Sitecore.ListManagement.Operations;
using Sitecore.Diagnostics;

namespace MA.CustomMarketingApps.Model
{
    class ListSubscriptionsStore: IListSubscriptionsStore<ContactDataModel>
    {
        private readonly IContactProvider _contactProvider;
        private readonly IContactListProvider _contactListProvider;
        private readonly IContactSourceFactory _contactSourceFactory;
        private readonly ISegmentationService _segmentationService;
        private readonly IListOperationRepository _operationRepository;
        private readonly ISubscriptionService _subscriptionService;
        private readonly int _batchSize;

        public ListSubscriptionsStore(IContactListProvider contactListProvider, ISubscriptionService subscriptionService, IContactProvider contactProvider, IListOperationRepository operationRepository, int BatchSize, IContactSourceFactory csf, ISegmentationService sss)
        {
            Assert.ArgumentNotNull((object)contactListProvider, "contactListProvider");
            Assert.ArgumentNotNull((object)subscriptionService, "subscriptionService");
            Assert.ArgumentNotNull((object)contactProvider, "contactProvider");
            Assert.ArgumentNotNull((object)operationRepository, "operationRepository");
            Assert.ArgumentNotNull((object)BatchSize, "settings");
            Assert.ArgumentNotNull((object)csf, "contactSourceFactory");
            Assert.ArgumentNotNull((object)sss, "segmentationService");
            _contactListProvider = contactListProvider;
            _subscriptionService = subscriptionService;
            _contactProvider = contactProvider;
            _operationRepository = operationRepository;
            _batchSize = BatchSize;
            _contactSourceFactory = csf;
            _segmentationService = sss;
        }

        public IEnumerable<ContactDataModel> GetSubscribers(Guid listId, string searchFilter, int pageIndex, int pageSize)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                ContactList contactList = this.EnsureList(listId, null);
                ContactSearchResult result = GetFilteredContacts(contactList, searchFilter, pageIndex, pageSize);
                List<ContactDataModel> source = result.Contacts.Select<Contact, ContactDataModel>(new Func<Contact, ContactDataModel>(this.MapEntity)).ToList<ContactDataModel>();
                if (source.Any<ContactDataModel>())
                {
                    source[0].Count = result.Count;
                }
                return source;
            }
        }
        private ContactList EnsureList(Guid listId, string message = null)
        {
            //IL_0020: Unknown result type (might be due to invalid IL or missing references)
            //IL_0027: Unknown result type (might be due to invalid IL or missing references)
            ContactList val = _contactListProvider.Get(listId, Context.Language.CultureInfo);
            if (val != null)
            {
                return val;
            }
            if (message != null)
            {
                throw new ContactListNotFoundException(message);
            }
            throw new ContactListNotFoundException(listId);
        }
        
        private ContactDataModel MapEntity(Contact entity)
        {
            ContactDataModel model1 = new ContactDataModel();
            Guid? id = entity.Id;
            model1.Id = (id.HasValue ? id.GetValueOrDefault() : Guid.Empty).ToString();
            model1.Email = (entity.Emails()?.PreferredEmail == null) ? null : ((entity.Emails() == null) ? null : (entity.Emails().PreferredEmail)).SmtpAddress;
            model1.FirstName = entity.Personal()?.FirstName;
            model1.LastName = entity.Personal()?.LastName;
            model1.Status = entity.GetFacet<CustomerStatus>().Status;
            ContactIdentifier identifier = entity.Identifiers.FirstOrDefault<ContactIdentifier>(x => (x.Source == "ListManager")) ?? entity.Identifiers.FirstOrDefault<ContactIdentifier>();
            model1.Identifier = identifier?.Identifier;
            model1.IdentifierSource = identifier?.Source;
            return model1;
        }
        public ContactSearchResult GetFilteredContacts(ContactList contactList, string searchFilter, int pageIndex, int pageSize)
        {
            IContactSource contactSource = this._contactSourceFactory.GetSource(contactList.ContactListDefinition);
            FilteringSource source2 = new FilteringSource(contactSource, searchFilter);
            string[] facets = new string[] { "ListSubscriptions", "Personal", "Emails", "CustomerStatus" };
            return new ContactSearchResult(this._segmentationService.GetContacts(source2, pageSize * pageIndex, pageSize, facets), (int)this._segmentationService.GetCount(contactSource));
        }
    }
}
