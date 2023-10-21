import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NgxPermissionsService } from 'ngx-permissions';
import { environment } from 'src/env/environment';
import { UserGroupsDto } from '../dtos/group-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthApisService {

  private baseUrl : string = environment.apiBaseUrl + '/Auth';

  constructor(private http : HttpClient ,
    private router: Router,
    private permissionsService: NgxPermissionsService
    ) {

 }

 getGroups(){
   return this.http.get<any>(`${this.baseUrl}/GetGroups`);
 }

 getUserGroupsByUserId(id:string){
  let url = "GetUserGroupsById?Id="+id;
   return this.http.get<any>(`${this.baseUrl}/`+url);
 }

 getRepresentationUserGroupsById(id:string){
  const params = new HttpParams()
  .set('id', id);

  let url = "GetRepresentationUserGroupsById";
   return this.http.get<any>(`${this.baseUrl}/GetRepresentationUserGroupsById`,  { params });
 }

 getUsers(){
   return this.http.get<any>(`${this.baseUrl}/GetUsers`);
 }

 saveRoleFunctions(userGroups: UserGroupsDto): Observable<any> {
  return this.http.put<any>(`${this.baseUrl}/UpdateUserGroups`,userGroups);
}



}
