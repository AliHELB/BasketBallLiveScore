import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  isAuthenticated: boolean = false;
  isAdmin: boolean = false;
  isEncoder: boolean = false;
  isViewer: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    // Synchronise l'état avec le service AuthService
    this.authService.isAuthenticated$.subscribe(authStatus => {
      this.isAuthenticated = authStatus;
    });

    this.authService.userRole$.subscribe(role => {
      this.isAdmin = role === 'admin';
      this.isEncoder = role === 'encoder';
      this.isViewer = role === 'viewer';
    });

    // Initialisation à partir du token local
    const token = localStorage.getItem('token');
    this.authService.updateAuthStatus(token);
  }

  logout() {
    localStorage.removeItem('token');
    this.authService.updateAuthStatus(null); // Réinitialise l'état
    this.router.navigate(['/auth']);
  }
}
