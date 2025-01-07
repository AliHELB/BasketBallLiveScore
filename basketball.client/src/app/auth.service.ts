import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private authState = new BehaviorSubject<boolean>(false);
  private roleState = new BehaviorSubject<string | null>(null);

  isAuthenticated$ = this.authState.asObservable();
  userRole$ = this.roleState.asObservable();

  updateAuthStatus(token: string | null) {
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      this.authState.next(true); // Met à jour l'état d'authentification
      this.roleState.next(payload.role); // Met à jour le rôle de l'utilisateur
    } else {
      this.authState.next(false);
      this.roleState.next(null);
    }
  }
}
