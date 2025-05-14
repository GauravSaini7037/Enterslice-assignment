import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TaskModel } from '../Models/UserModel';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserDetailsServiceService {
  private Url:string = environment.baseUrl;
  lastUrl:string="/Tasks";
  constructor(private _http:HttpClient) { }
  SaveTask(data : TaskModel):Observable<TaskModel>{
    return this._http.post<TaskModel>(`${this.Url}${this.lastUrl}/SaveTask`,data);
  }
  GetAllTasks():Observable<TaskModel>{
    return this._http.get<TaskModel>(`${this.Url}${this.lastUrl}/GetAllTasks`);
  }
  DeleteTask(id:number):Observable<TaskModel>{
    return this._http.delete<TaskModel>(`${this.Url}${this.lastUrl}/DeleteTask/${id}`);
  }
}
