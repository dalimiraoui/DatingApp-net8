import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikeService } from '../_services/like.service';
import { Member } from '../_models/member';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [MemberCardComponent, FormsModule, ButtonsModule, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit, OnDestroy{

  likeService = inject(LikeService)

  predicate : string ='liked'

  pageNumber: number =1;
  pageSize: number =5;

  
  ngOnInit(): void {
    this.getTitle()
    this.loadLikes()
  }

  getTitle() {
    switch(this.predicate) {
      case 'liked':  return 'Members you like'
      case 'likedBy' : return 'Members Who like you'
      default: return 'Mutual'

    }
  }
  loadLikes() {
    this.likeService.getLikes(this.predicate, this.pageNumber, this.pageSize);
  }
  pageChanged(event : any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page
      this.loadLikes()

    }
  }

  ngOnDestroy(): void {
    this.likeService.paginatedResult.set(null);
  }




}
