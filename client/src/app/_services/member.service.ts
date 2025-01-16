import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, Signal, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResul } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private http = inject(HttpClient)
  baseUrl= environment.apiUrl;
  // members = signal<Member[]>([]) 
  paginatedResult = signal<PaginatedResul<Member[]>|null>(null) 

  // functions 

  getMembers(userParams: UserParams) {
    let params = this.setPaginationHeaders(userParams.pageNumber, userParams.pageSize)

    params = params.append('minAge', userParams.minAge)
    params = params.append('maxAge', userParams.maxAge)
    params = params.append('gender', userParams.gender)
    params = params.append('orderBy', userParams.orderBy)
    
    this.http.get<Member[]>(this.baseUrl +'users', {observe : 'response', params}).subscribe({
      next : response => {
        this.paginatedResult.set({
          items: response.body as Member[],
          pagination : JSON.parse(response.headers.get('Pagination')!)
        })
      }
    })
  }

  private setPaginationHeaders(pageNumber?: number, pageSize?: number){
    let params = new HttpParams()

    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber)
      params = params.append('pageSize', pageSize)
    }
    return params

  }

  getMemberByUserName( username:string) {
    // var member = this.members().find(x=> x.userName == username)
    // if (member != undefined ) return of(member)
    return this.http.get<Member>(this.baseUrl +'users/'+username)
  }

  updateUser(member : Member) {
    return this.http.put(this.baseUrl +'users', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map( m => m.userName == member.userName ? member : m))
      // })
    )
  }

  setMainPhoto(photo : Photo) {
    return this.http.put(this.baseUrl +'users/set-main-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map( m => {
      //     if (m.photos.includes(photo)) {
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    )
  }

  deletePhoto(photo : Photo) {
    return this.http.delete(this.baseUrl +'users/delete-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map( m => {
      //     if (m.photos.includes(photo)) {
      //       // Remove the photo from the photos array
      //       m.photos = m.photos.filter(p => p.id !== photo.id);
      //     }
      //     return m;
      //   }))
      // })
    )
  }
}
