import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  http= inject(HttpClient);

  baseUrl = environment.apiUrl;

  currentUser =signal<User|null>(null)

  login(model :any) {
    return this.http.post<User>(this.baseUrl +'account/login', model).pipe(
      map( user => {
        if(user) {
          localStorage.setItem("currentUser", JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    )
  }

  register(model :any) {
    return this.http.post<User>(this.baseUrl +'account/register', model).pipe(
      map( user => {
        console.log(user);
        
        if(user) {
          localStorage.setItem("currentUser", JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      })
    )
  }

  logout() {
    localStorage.removeItem("currentUser");
    this.currentUser.set(null);
  }
}
