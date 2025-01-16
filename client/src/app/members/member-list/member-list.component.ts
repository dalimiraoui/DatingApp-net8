import { Component, inject, OnInit } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberCardComponent } from '../member-card/member-card.component';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons'

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
  memberService = inject(MemberService);
  private accountService = inject(AccountService)
  userParams = new UserParams(this.accountService.currentUser())


  genderList = [{value:'male', display:'Males'}, {value:'female', display:'Females'}]

  ngOnInit(): void {
    if (!this.memberService.paginatedResult()) this.getAllMembers();
  }

  getAllMembers() {
    this.memberService.getMembers(this.userParams)
  }

  resetFilters() {
    this.userParams = new UserParams(this.accountService.currentUser())
    this.getAllMembers()
  }

  pageChanged(event : any) {
    if (this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page
      this.getAllMembers()

    }
  }

}
