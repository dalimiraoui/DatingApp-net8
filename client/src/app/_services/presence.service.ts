import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl
  private hubConnection?: HubConnection
  private toastr = inject(ToastrService)
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
      this.toastr.info(username + ' has connected')
    })

    this.hubConnection.on("UserIsOffOnline", username => {
      this.toastr.warning(username + ' has disconnected')
    })

    this.hubConnection.on("GetOnlineUsers", username => {
      this.onlineUsers.set(username)
      
    })
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
