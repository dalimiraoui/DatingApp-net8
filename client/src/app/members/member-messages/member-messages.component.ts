import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent{
  @ViewChild('messageForm', {static:true}) messageForm? : NgForm
  private messageService = inject(MessageService)
  username = input.required<string>();
  messages = input.required<Message[]>();
  updatedMessage = output<Message>();
  messageContent ='';

  sendMessages() {
    this.messageService.sendMessage(this.username(), this.messageContent).subscribe({
      next : message => {
      this.updatedMessage.emit(message)
      this.messageForm?.reset()
      }
    })
  }

}
