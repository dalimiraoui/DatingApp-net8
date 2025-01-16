import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { MemberService } from '../../_services/member.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, PhotoEditorComponent, TimeagoModule, DatePipe],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit{
 
  @ViewChild("editForm") editForm? : NgForm
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue =true;
    }
  }
  member?:Member
  //inject services
  private accountService = inject(AccountService)
  private memberService = inject(MemberService)
  private toastr = inject(ToastrService)

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.accountService.currentUser()?.username;
    if (!username) return;
    this.memberService.getMemberByUserName(username).subscribe({
      next : data => {this.member = data}
    })
  }

  updateMember() {
    this.memberService.updateUser(this.editForm?.value).subscribe({
      next : _ => {
        this.toastr.success("Profile updated successfully")
        this.editForm?.reset(this.member)
      },
      error : error => this.toastr.error(error)
    })
    
  }

  onMemberChange(event :Member) {
    this.member =event
  }

}
