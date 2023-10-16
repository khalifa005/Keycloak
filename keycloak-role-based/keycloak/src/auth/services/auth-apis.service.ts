import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NgxPermissionsService } from 'ngx-permissions';
import { environment } from 'src/env/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthApisService {

  private baseUrl : string = environment.apiBaseUrl + '/user';

  constructor(private http : HttpClient ,
    private router: Router,
    private permissionsService: NgxPermissionsService
    ) {

 }

 getGroups(){
   return this.http.post<any>(`${this.baseUrl}/login`,loginObj);
 }



}
