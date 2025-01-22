import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikeService } from './like.service';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  http= inject(HttpClient);
  private likeService= inject(LikeService);
  private presenceService= inject(PresenceService);

  baseUrl = environment.apiUrl;

  currentUser =signal<User|null>(null)

  roles = computed( () => {
    const user = this.currentUser();
    if (user && user.token) {
      const role = JSON.parse(atob(user.token.split('.')[1])).role
      return Array.isArray(role) ? role : [role]
    }
    return []
  })

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
    this.presenceService.createHubConnection(user)
  }

  logout() {
    localStorage.removeItem("currentUser");
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();
  }
}
