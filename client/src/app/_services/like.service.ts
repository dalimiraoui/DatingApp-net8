import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { PaginatedResul } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class LikeService {

  private http = inject(HttpClient)
  baseUrl= environment.apiUrl; 
  
 paginatedResult = signal<PaginatedResul<Member[]>|null>(null) 
  likeIds = signal<number[]>([])

  toggleLike(targetId: number) {
    return this.http.post(`${this.baseUrl}likes/${targetId}`, {})
  }

  getLikes(predicate : string, pageNumber: number, pageSize : number) {
    let params = setPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate)
    return this.http.get<Member[]>(`${this.baseUrl}likes`, {observe: 'response', params}).subscribe({
      next: response => setPaginatedResponse(response, this.paginatedResult)
    })
  }

  getLikeIds() {
    return this.http.get<number[]>(`${this.baseUrl}likes/list`).subscribe({
      next: ids => this.likeIds.set(ids)
    })
  }
}
