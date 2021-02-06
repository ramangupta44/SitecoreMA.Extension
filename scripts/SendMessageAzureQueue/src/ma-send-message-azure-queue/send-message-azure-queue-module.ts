import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SendMessageAzureQueueEditorComponent } from './editor/send-message-azure-queue-editor.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [SendMessageAzureQueueEditorComponent],
    entryComponents: [SendMessageAzureQueueEditorComponent]
})
export class SendMessageAzureQueueModule { }