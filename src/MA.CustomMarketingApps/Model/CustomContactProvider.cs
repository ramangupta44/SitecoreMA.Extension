using Sitecore.Diagnostics;
using Sitecore.ListManagement;
using Sitecore.ListManagement.XConnect;
using Sitecore.ListManagement.XConnect.Segmentation;
using Sitecore.ListManagement.XConnect.Sources;
using Sitecore.ListManagement.XConnect.Web;
using Sitecore.XConnect;
using Sitecore.XConnect.Client.Synchronous;
using System.Collections.Generic;


namespace MA.CustomMarketingApps.Model
{
    public class CustomContactProvider : IContactProvider
    {
        private readonly IContactSourceFactory _contactSourceFactory;

        private readonly ISegmentationService _segmentationService;

        public CustomContactProvider(ISegmentationService segmentationService, IContactSourceFactory contactSourceFactory)
        {
            Assert.ArgumentNotNull((object)segmentationService, "segmentationService");
            Assert.ArgumentNotNull((object)contactSourceFactory, "contactSourceFactory");
            _segmentationService = segmentationService;
            _contactSourceFactory = contactSourceFactory;
        }
        public ContactSearchResult GetFilteredContacts(ContactList contactList, string searchFilter, int pageIndex, int pageSize)
        {
            IContactSource source = _contactSourceFactory.GetSource(contactList.ContactListDefinition);
            FilteringSource contactSource = new FilteringSource(source, searchFilter);
            IEnumerable<Contact> contacts = _segmentationService.GetContacts(contactSource, pageSize * pageIndex, pageSize, "ListSubscriptions", "Personal", "Emails", "CustomerStatus");
            long count = _segmentationService.GetCount(source);
            return new ContactSearchResult(contacts, (int)count);
        }
        public IEntityBatchEnumerator<Contact> GetContactBatchEnumerator(ContactList contactList, int batchSize, params string[] facets)
        {
            IContactSource source = _contactSourceFactory.GetSource(contactList.ContactListDefinition);
            return _segmentationService.GetContactBatchEnumerator(source, batchSize, facets);
        }
        public long GetCount(ContactList contactList)
        {
            IContactSource source = _contactSourceFactory.GetSource(contactList.ContactListDefinition);
            return _segmentationService.GetCount(source);
        }

    }
}
