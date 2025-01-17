import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { LikeService } from './like.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  http= inject(HttpClient);
  private likeService= inject(LikeService);

  baseUrl = environment.apiUrl;

  currentUser =signal<User|null>(null)

  login(model :any) {
    return this.http.post<User>(this.baseUrl +'account/login', model).pipe(
      map( user => {
        if(user) {
          this.setCurrentUser(user)
        }
      })
    )
  }

  register(model :any) {
    return this.http.post<User>(this.baseUrl +'account/register', model).pipe(
      map( user => {
        console.log(user);
        
        if(user) {
          this.setCurrentUser(user)
        }
        return user;
      })
    )
  }

  setCurrentUser(user : User) {
    localStorage.setItem("currentUser", JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds()
  }

  logout() {
    localStorage.removeItem("currentUser");
    this.currentUser.set(null);
  }
}
