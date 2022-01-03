using MA.CustomFacets.Model;
//using MA.CustomMarketingApps.Interfaces;
using Sitecore.EmailCampaign.Cd.Controllers;
using Sitecore.EmailCampaign.Cd.Services;
using Sitecore.Framework.Conditions;
using Sitecore.Modules.EmailCampaign.Core;
using Sitecore.Modules.EmailCampaign.Core.Contacts;
using Sitecore.Modules.EmailCampaign.Core.Crypto;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.EmailCampaign.Model.XConnect.Facets;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Http;

namespace MA.CustomMarketingApps.Controllers
{
    public class EXMPreferencesController: Controller
    {
        private ContactIdentifier _contactIdentifier;
        //private readonly IStringCipher _cipher;
        private readonly IMarketingPreferencesService _marketingPreferencesService;
        private readonly IContactService _contactService;
        private readonly IStringCipher _cipher;
        protected readonly ILogger Logger;
        private Contact _contact;

        public EXMPreferencesController()
        {
            IContactService contactService = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IContactService>();
            IStringCipher cipher = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IStringCipher>();
            ILogger logger = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<ILogger>();
            Condition.Requires<IContactService>(contactService, nameof(contactService)).IsNotNull<IContactService>();
            Condition.Requires<IStringCipher>(cipher, nameof(cipher)).IsNotNull<IStringCipher>();
            Condition.Requires<ILogger>(logger, nameof(logger)).IsNotNull<ILogger>();
            this._contactService = contactService;
            this._cipher = cipher;
            this.Logger = logger;
        }

        [System.Web.Http.HttpPost]
        public void getcontact(List<MarketingPreference> Preferences, string EncryptedContactIdentifier)
        {
            ContactIdentifier contactIdentifier = this.GetContactIdentifier(EncryptedContactIdentifier);
            
            Contact contact = this.GetContact(contactIdentifier);
            CustomerStatus custstatus = new CustomerStatus();
            foreach (var item in Preferences)
            {
                if( item.MarketingCategoryId.ToString() == "67edf04b-d0cb-46a4-8d62-b06a9269613f")
                {
                    custstatus.Status = item.Preference.ToString();
                }
                // write logic
            }
            //Contact con = ExmContext.Contact;
            //Guid contactid = GetGuid(EncryptedContactIdentifier);
            string xConnectUrlBase = "https://demo93.xconnect";
            string thumbPrint = "ADA0D856C029312BF904CC008E06C4ED4C712590";
            CertificateHttpClientHandlerModifierOptions options = CertificateHttpClientHandlerModifierOptions.Parse($"StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue={thumbPrint}");

            var certificateModifier = new CertificateHttpClientHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // Ensure configuration pointing to appropriate instance of xconnect
            var collectionClient = new CollectionWebApiClient(new Uri($"{xConnectUrlBase}/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri($"{xConnectUrlBase}/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri($"{xConnectUrlBase}/configuration"), clientModifiers, new[] { certificateModifier });

            //For exisitng facets
           //XdbModel[] models = { CollectionModel.Model };
            //For custom Facets
           XdbModel[] models = { CollectionModel.Model, CustomerCollectionModel.Model };

            //var cfg = new XConnectClientConfiguration(new XdbRuntimeModel(models), collectionClient, searchClient, configurationClient);
            var cfg = new XConnectClientConfiguration(new XdbRuntimeModel(models), collectionClient, searchClient, configurationClient, true);
            cfg.Initialize();
            using (var client = new XConnectClient(cfg))
            {
                //var reference = new Sitecore.XConnect.ContactReference(Guid.Parse("CA107901-6CE7-0000-0000-06388D85276A"));
                //Contact contact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] {
                //                PersonalInformation.DefaultFacetKey,
                //                EmailAddressList.DefaultFacetKey,
                //                PhoneNumberList.DefaultFacetKey,
                //                CustomerStatus.DefaultFacetKey,
                //                CustomerOptIn.DefaultFacetKey
                //            }));
                if (contact != null)
                {
                    

                    CustomerStatus customeroptinFacet = contact.GetFacet<CustomerStatus>(CustomerStatus.DefaultFacetKey);
                    if (customeroptinFacet != null)
                    {
                        // write logic
                        customeroptinFacet.Status = custstatus.Status;
                        client.SetFacet(contact, CustomerStatus.DefaultFacetKey, customeroptinFacet);
                    }
                    else
                    {
                        client.SetFacet(contact, CustomerStatus.DefaultFacetKey, custstatus);
                    }
                }
                client.Submit();
            }
            //return Json("ok");
        }



        protected ContactIdentifier GetContactIdentifier(string encryptedValue)
        {
            if (this._contactIdentifier != null)
                return this._contactIdentifier;
            try
            {
                string str = this._cipher.TryDecrypt(encryptedValue);
                if (string.IsNullOrEmpty(str))
                {

                    return (ContactIdentifier)null;
                }
                this._contactIdentifier = str.ToContactIdentifier();
                return this._contactIdentifier;
            }
            catch (Exception ex)
            {
                return (ContactIdentifier)null;
            }
        }

        protected Contact GetContact(ContactIdentifier contactIdentifier)
        {
            return this._contact ?? (this._contact = this._contactService.GetContactWithRetry(contactIdentifier, 100.0, 3, this.FacetKeys));
        }

        protected virtual string[] FacetKeys { get; } = new string[]{ PersonalInformation.DefaultFacetKey,
                                EmailAddressList.DefaultFacetKey,
                                PhoneNumberList.DefaultFacetKey,
                                CustomerStatus.DefaultFacetKey};


        //protected Guid GetGuid(string encryptedValue)
        //{
        //    try
        //    {
        //        string input = this. .TryDecrypt(encryptedValue);
        //        if (!string.IsNullOrEmpty(input))
        //        {
        //            Guid result;
        //            return Guid.TryParse(input, out result) ? result : Guid.Empty;
        //        }
        //        return Guid.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Guid.Empty;
        //    }
        //}
    }
}
