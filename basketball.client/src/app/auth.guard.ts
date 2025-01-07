import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token = localStorage.getItem('token');
    if (token && this.isTokenValid(token)) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const userRole = payload.role; // Récupération du rôle utilisateur
      const allowedRoles = route.data['roles'] as Array<string>;

      // Vérifie si le rôle de l'utilisateur est autorisé
      if (allowedRoles.includes(userRole)) {
        return true;
      } else {
        this.router.navigate(['/auth']);
        return false;
      }
    } else {
      this.router.navigate(['/auth']);
      return false;
    }
  }

  private isTokenValid(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload && payload.exp > Date.now() / 1000; // Vérifie que le token n'est pas expiré
    } catch (e) {
      return false;
    }
  }
}
