using MA.CustomAction.Services;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// install Sitecore.Xdb.MarketingAutomation.Core and Sitecore.XConnect.Collection.Model nuget package

namespace MA.CustomAction.Model
{
    class SendMessageToAzureActivity : IActivity
    {
        private IAzureService _AzureQueueSendingService { get; set; }
        public IActivityServices Services { get; set; }
        public string message { get; set; }
        public SendMessageToAzureActivity(IAzureService _AzureQueueService)
        {
            _AzureQueueSendingService = _AzureQueueService;
        }
        public ActivityResult Invoke(IContactProcessingContext context)
        {
            if (!string.IsNullOrEmpty(this.message))
            {
                Guid planDefinitionId = context.ActivityEnrollment.EnrollmentKey.PlanDefinitionId;
                Contact contact = context.Contact;
                if (contact != null)
                {
                    message = GetPersonalizedMessage(message, contact);
                    _AzureQueueSendingService.SendAzureQueueAsync(message);

                }
            }
            return new SuccessMove();
        }

        private string GetPersonalizedMessage(string message, Contact contact)
        {
            //Personal Information
            string firstName = "";
            string lastName = "";
            string title = "";
            // Address
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string address4 = "";
            string stateOrProvince = "";
            string postalCode = "";

            string personalInfoFacetFacetKey = PersonalInformation.DefaultFacetKey;
            var PersonalInfofacet = contact.GetFacet<PersonalInformation>(personalInfoFacetFacetKey);
            if (PersonalInfofacet != null)
            {
                firstName = PersonalInfofacet.FirstName;
                lastName = PersonalInfofacet.LastName;
                title = PersonalInfofacet.Title;
            }

            string addressFacetFacetKey = AddressList.DefaultFacetKey;
            var addressfacet = contact.GetFacet<AddressList>(addressFacetFacetKey);
            if (addressfacet != null && addressfacet.PreferredAddress != null)
            {
                address1 = addressfacet.PreferredAddress.AddressLine1;
                address2 = addressfacet.PreferredAddress.AddressLine2;
                address3 = addressfacet.PreferredAddress.AddressLine3;
                address4 = addressfacet.PreferredAddress.City;
                stateOrProvince = addressfacet.PreferredAddress.StateOrProvince;
                postalCode = addressfacet.PreferredAddress.PostalCode;
            }

            message = message.Replace("$FirstName$", firstName);
            message = message.Replace("$LastName$", lastName);
            message = message.Replace("$Title$", title);
            message = message.Replace("$Address1$", address1);
            message = message.Replace("$Address2$", address2);
            message = message.Replace("$Address3$", address3);
            message = message.Replace("$Address4$", address4);
            message = message.Replace("$StateOrProvince$", stateOrProvince);
            message = message.Replace("$PostalCode$", postalCode);
            return message;
        }
    }
}
