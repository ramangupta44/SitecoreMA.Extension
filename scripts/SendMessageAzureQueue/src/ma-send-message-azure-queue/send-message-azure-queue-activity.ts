import { SingleItem} from '@sitecore/ma-core';
 
export class SendMessageAzureQueueActivity extends SingleItem {
    getVisual(): string {
        const title= 'Send Message to Azure Queue';
        const subTitle = 'Send Message to Azure Queue';
        const cssClass = this.isDefined ? '' : 'undefined';
        
        return `
            <div class="viewport-send-message-azure-queue-editor marketing-action ${cssClass}">
                <span class="icon">
                    <img src="/~/icon/OfficeWhite/32x32/cubes.png" />
              </span>
                <p class="text with-subtitle" title="${title}">
                    ${title}
                    <small class="subtitle" title="${subTitle}">${subTitle}</small>
                </p>
            </div>
        `;
    }
 
    get isDefined(): boolean {
            return true;
    }
}