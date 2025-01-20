import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { Gallery, GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-member-details',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-details.component.html',
  styleUrl: './member-details.component.css'
})
export class MemberDetailsComponent implements OnInit {
 
 @ViewChild('memberTabs', {static:true}) memberTabs? : TabsetComponent
 private messageService = inject(MessageService);
 private memberService = inject(MemberService);
 private route = inject(ActivatedRoute);

 member: Member = {} as Member;

 images : GalleryItem[] =[];

 activeTab?: TabDirective
 messages: Message[] =[]

 ngOnInit(): void {
  this.route.data.subscribe({
    next : data => {
      this.member = data['member'];
      this.member && this.member.photos.map( p => {
        this.images.push( new ImageItem({src: p.url, thumb: p.url}))
      })
    }
  })
  
  this.route.queryParams.subscribe({
    next : params => {
      params['tab']  && this.selectTab(params['tab'] )
    }
  })
 }

 onActiveTab(data: TabDirective) {
  this.activeTab = data;
  if (this.messages.length ===0 && this.activeTab.heading=='Messages' && this.member) {
    this.messageService.getMessageThread(this.member.userName).subscribe({
      next : messages => this.messages = messages
    })
  }
 }
 
 onUpdateMessages(event : Message) {
  this.messages.push(event);
 }

 selectTab(heading : string) {
   if (this.memberTabs) {
    const messageTab= this.memberTabs.tabs.find( x => x.heading === heading)
    if (messageTab) messageTab.active = true
   }
 }

//  loadMember() {
//   const username = this.route.snapshot.paramMap.get('username');
//   if (!username) return;
//   this.memberService.getMemberByUserName(username).subscribe({
//     next : data => {
//       this.member = data;
//       this.member.photos.map( p => {
//         this.images.push( new ImageItem({src: p.url, thumb: p.url}))
//       })
//     }
//   })
//  }
}
