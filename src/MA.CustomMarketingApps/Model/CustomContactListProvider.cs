using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.ListManagement;
using Sitecore.ListManagement.Providers;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.ContactLists;
using Sitecore.Marketing.Definitions.Segments;

namespace MA.CustomMarketingApps.Model
{
    class CustomContactListProvider : IContactListProvider
	{

		private readonly IDefinitionManager<IContactListDefinition> _contactListDefinitionManager;

		private readonly IDefinitionManager<ISegmentDefinition> _segmentDefinitionManager;

		private readonly CultureInfo _defaultCulture;

		private readonly string _emptyGuidCannotBeUsedAsListId = "Empty Guid cannot be used as list Id";

		public CustomContactListProvider(IDefinitionManager<IContactListDefinition> contactListDefinitionManager, IDefinitionManager<ISegmentDefinition> segmentDefinitionManager, CultureInfo defaultCulture)
		{
			Assert.ArgumentNotNull((object)contactListDefinitionManager, "contactListDefinitionManager");
			Assert.ArgumentNotNull((object)segmentDefinitionManager, "segmentDefinitionManager");
			Assert.ArgumentNotNull((object)defaultCulture, "defaultCulture");
			_contactListDefinitionManager = contactListDefinitionManager;
			_segmentDefinitionManager = segmentDefinitionManager;
			_defaultCulture = defaultCulture;
		}

		public ContactList Get(Guid listId, CultureInfo culture)
		{
			Assert.ArgumentCondition(listId != Guid.Empty, "listId", _emptyGuidCannotBeUsedAsListId);
			Assert.ArgumentNotNull((object)culture, "culture");
			IContactListDefinition definition = this.GetDefinition<IContactListDefinition>(_contactListDefinitionManager, listId, culture);
			if (definition == null)
			{
				return null;
			}
			return GetContactList(definition);
		}

		public ResultSet<ContactList> Search(SearchParameters<IContactListDefinition, DateTime> searchParameters)
		{
			Assert.ArgumentNotNull((object)searchParameters, "searchParameters");
			ResultSet<DefinitionResult<IContactListDefinition>> val = _contactListDefinitionManager.Search<DateTime>(searchParameters);
			return new ResultSet<ContactList>(val.PageNumber, (IReadOnlyCollection<ContactList>)(from x in val.DataPage
																									   select GetContactList(x.Data)).ToList(), val.Total);
		}

		public void Save(ContactList contactList)
		{
			Save(contactList, activate: false);
		}

		public void Save(ContactList contactList, bool activate)
		{
			Assert.ArgumentNotNull((object)contactList, "contactList");
			Task.Run(async delegate
			{
				await _contactListDefinitionManager.SaveAsync(contactList.ContactListDefinition, activate).ConfigureAwait(continueOnCapturedContext: false);
			}).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}

		public void Delete(Guid listId)
		{
			Assert.ArgumentCondition(listId != Guid.Empty, "listId", _emptyGuidCannotBeUsedAsListId);
			_contactListDefinitionManager.Delete(listId, (CultureInfo)null);
		}

		private ContactList GetContactList(IContactListDefinition contactListDefinition)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			if ((int)contactListDefinition.Type == 1)
			{
				IEnumerable<ISegmentDefinition> segmentDefinitions = from s in contactListDefinition.SegmentDefinitionIds ?? Enumerable.Empty<Guid>()
																	 select this.GetDefinition<ISegmentDefinition>(_segmentDefinitionManager, s, ((IDefinition)contactListDefinition).Culture) into item
																	 where item != null
																	 select item;
				return new ContactList(contactListDefinition, segmentDefinitions);
			}
			return new ContactList(contactListDefinition);
		}

		private T GetDefinition<T>(IDefinitionManager<T> definitionaManager, Guid id, CultureInfo culture) where T : class, IDefinition
		{
			IReadOnlyCollection<CultureInfo> availableCultures = definitionaManager.GetAvailableCultures(id);
            if (availableCultures == null || !availableCultures.Any())
            {
                return null;
            }
            CultureInfo cultureInfo = availableCultures.FirstOrDefault((CultureInfo c) => c == culture) ?? availableCultures.FirstOrDefault((CultureInfo c) => c == _defaultCulture) ?? availableCultures.First();
            //CultureInfo cultureInfo = new CultureInfo("en", false);
			return definitionaManager.Get(id, cultureInfo);
		}
	}
}
