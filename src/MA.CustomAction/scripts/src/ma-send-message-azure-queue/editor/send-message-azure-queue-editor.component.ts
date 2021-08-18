import { Component, OnInit, Injector } from '@angular/core';
import { EditorBase } from '@sitecore/ma-core';
 
@Component({
    selector: 'send-message-azure-queue-editor',
            template: `
        <section class="content">
            <div class="form-group">
                <div class="row send-message-azure-queue-editor">
                    <label class="col-12 title">Message to send</label>
                    <div class="col-12">
                        <textarea rows="6" type="text" class="form-control" [(ngModel)]="message"></textarea>
                   </div>
                </div>
            </div>

        </section>
    `,
    //CSS Styles are ommitted for brevity
    styles: ['']
})
 
export class SendMessageAzureQueueEditorComponent extends EditorBase implements OnInit {

    constructor(private injector: Injector) {
        super();
    }
 
    message: string;
 
    ngOnInit(): void {
        this.message = this.model ? this.model.message || "" : "";
     }
 
    serialize(): any {
        return {  
            message: this.message,
        };
    }
}