import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl
  private hubConnection?: HubConnection
  private toastr = inject(ToastrService)
  private router = inject(Router)
  onlineUsers = signal<string[]>([])

  createHubConnection(user : User) {
    this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.hubUrl +'presence', {
          accessTokenFactory : () => user.token
        })
        .withAutomaticReconnect()
        .build();
    this.hubConnection.start().catch( error => console.log(error))

    this.hubConnection.on("UserIsInOnline", username => {
      this.onlineUsers.update(users => [...users, username])
    })

    this.hubConnection.on("UserIsOffOnline", username => {
      this.onlineUsers.update(users => users.filter(x => x != username))
      //this.toastr.warning(username + ' has disconnected')
    })

    this.hubConnection.on("GetOnlineUsers", username => {
      this.onlineUsers.set(username)
      
    })

    this.hubConnection.on("NewMessageReceived", ({username, knownAs}) => {
      this.toastr.info(knownAs + ' has sent you a message! Click me to see it')
        .onTap
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/' + username + '?tab=messages'))
    })
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
