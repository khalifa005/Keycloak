import { KeycloakService } from 'keycloak-angular';

export function initKeycloak (keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: 'http://localhost:8088',
        realm: 'amana',
        clientId: 'angular-app-client',
      },
      initOptions: {
        onLoad: 'check-sso',
        checkLoginIframe: false
      },
      //token will be added to all api's requests
      enableBearerInterceptor: true,
      bearerPrefix: 'Bearer',
    });
}
