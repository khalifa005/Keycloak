import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'my-app';

  authenticated = false;
  isUser = false;
  isAdmin = false;

    // Http Options
    httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
  })};

  constructor(private readonly keycloak: KeycloakService, private http: HttpClient) {
    this.keycloak.isLoggedIn().then((authenticated) => {
      let token = this.keycloak.getToken().then((tokenValue)=>{
        console.log("token");
        console.log(tokenValue);
      });

      this.authenticated = authenticated;
      if (authenticated) {
        const roles = this.keycloak.getUserRoles();
        this.isUser = roles.includes('USER');
        this.isAdmin = roles.includes('ADMIN');
      }
    });
  }

  ngOnInit() {
    this.http.get('https://localhost:44384/WeatherForecast', this.httpOptions).subscribe({
      next: (response) =>{
        console.log(response);
      },
      error: (err) => {
        console.log(err);
      }
     });
  }

  login() {
    this.keycloak.login();
  }

  logout() {
    this.keycloak.logout();
  }
}
