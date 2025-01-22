import { AfterViewChecked, Component, inject, input, ViewChild } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements AfterViewChecked{
  
  @ViewChild('messageForm', {static:true}) messageForm? : NgForm
  @ViewChild('scrollMe') scrollContainer? : any
  messageService = inject(MessageService)
  username = input.required<string>();
  messageContent ='';

  sendMessages() {
    this.messageService.sendMessage(this.username(), this.messageContent).then(() => {
      this.messageForm?.reset();
      this.scrollToButton();
    })
  }

  ngAfterViewChecked(): void {
    this.scrollToButton();
  }

  private scrollToButton() {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight
    }
  }

}
