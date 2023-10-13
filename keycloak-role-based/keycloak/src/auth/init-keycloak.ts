import { KeycloakService } from 'keycloak-angular';
import { switchMap } from 'rxjs';
import devJsonData from '../config.json/config.dev.json';
// import prodJsonData from '../config.json/config.dev.json';

export function initKeycloak (keycloak: KeycloakService) {

  return () =>
    keycloak.init({
      config: {
        url: devJsonData.KEYCLOAK_URL,
        realm: devJsonData.KEYCLOAK_REALM,
        clientId: devJsonData.KEYCLOAK_CLIENT_ID,
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
