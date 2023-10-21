import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';
import { takeUntil, Subject, Subscription } from 'rxjs';
import { ApiResponse, GroupDto } from 'src/auth/dtos/group-dto';
import { AuthApisService } from 'src/auth/services/auth-apis.service';
import { environment } from 'src/env/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit , OnDestroy {
  title = 'my-app';

  authenticated = false;
  isUser = false;
  isAdmin = false;

    // Http Options
    httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
  })};

  constructor(private readonly keycloak: KeycloakService,
     private router: Router,
     private authApisService: AuthApisService,
     private http: HttpClient) {
    this.keycloak.isLoggedIn().then((authenticated) => {
      let token = this.keycloak.getToken().then((tokenValue)=>{
        console.log("token");
        console.log(tokenValue);
      });



      this.authenticated = authenticated;
      if (authenticated) {
        // const roles = this.keycloak.getUserRoles();
        // this.isUser = roles.includes('USER');
        // this.isAdmin = roles.includes('ADMIN');
        let Profile = this.keycloak.loadUserProfile().then((value)=>{
          console.log("profile");
          console.log(value);
          this.getUserGroups(value.id as string);
        });

      }
    });
  }

  ngOnInit() {

    if (environment.production) {
      console.log("environment.production");
      console.log(environment.apiBaseUrl);
      }else{
        console.log("server local");
        console.log(environment.apiBaseUrl);

      }

    this.http.get('https://localhost:44384/WeatherForecast', this.httpOptions).subscribe({
      next: (response) =>{
        console.log(response);
      },
      error: (err) => {
        console.log(err);
      }
     });
  }

  goToCamunda() {
    this.router.navigateByUrl("/camunda");
  }

  goToUsers() {
    this.router.navigateByUrl("/users");
  }

  goToComplain() {
    this.router.navigateByUrl("/products");
  }


  login() {
    this.keycloak.login();
  }

  logout() {
    this.keycloak.logout();
  }

  userGroupsPaths!: string[];

  private getUserGroups(userId:string): void {

    this.authApisService
    .getUserGroupsByUserId(userId)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next:(response:ApiResponse<GroupDto[]>) => {

       if(response.statusCode == 200 && response.data){
        this.userGroupsPaths = response.data.map(x=> x.path);

        //sid
      if(this.userGroupsPaths.length > 0){
        this.authApisService.loadPermissions(this.userGroupsPaths);
      }
       }else{
         console.log(response);
       }
         },
        error:(error:any)=> {
          console.log(error);
        }
   });

  }
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private subs: Subscription[] = [];
  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
