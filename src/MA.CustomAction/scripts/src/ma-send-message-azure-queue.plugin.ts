import { Plugin } from '@sitecore/ma-core';
import { SendMessageAzureQueueActivity } from './ma-send-message-azure-queue/send-message-azure-queue-activity';
import { SendMessageAzureQueueModuleNgFactory } from '../codegen/ma-send-message-azure-queue/send-message-azure-queue-module.ngfactory';
import { SendMessageAzureQueueEditorComponent } from '../codegen/ma-send-message-azure-queue/editor/send-message-azure-queue-editor.component';
 
// Use the @Plugin decorator to define all the activities the module contains.
@Plugin({
    activityDefinitions: [
        {
            // The ID must match the ID of the activity type description definition item in the CMS.
            id: 'C7C9F433-0D96-4791-BE74-4D942C16EC11', 
            activity: SendMessageAzureQueueActivity,
            editorComponenet: SendMessageAzureQueueEditorComponent,
            editorModuleFactory: SendMessageAzureQueueModuleNgFactory
        }
    ]
})
export default class SendMessageAzureQueuePlugin {}