import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private http = inject(HttpClient)
  baseUrl= environment.apiUrl;

  // functions 

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl +'users')
  }

  getMemberByUserName( username:string) {
    return this.http.get<Member>(this.baseUrl +'users/'+username)
  }
}
