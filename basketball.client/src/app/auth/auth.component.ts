import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-auth',
  standalone: false,
  
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent implements OnInit {
  loginForm: FormGroup;
  registerForm: FormGroup;
  isLoginMode: boolean = true;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private authService: AuthService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(3)]],
    });

    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void { }

  switchMode() {
    this.isLoginMode = !this.isLoginMode;
    this.errorMessage = '';
    this.successMessage = ''; 
  }

  onSubmitLogin() {
    if (this.loginForm.valid) {
      this.http.post('https://localhost:7043/api/auth/login', this.loginForm.value).subscribe(
        (response: any) => {
          localStorage.setItem('token', response.token);
          this.authService.updateAuthStatus(response.token);
          this.router.navigate(['/viewer']);
        },
        (error) => {
          console.error('Login error:', error);
          this.errorMessage = 'Invalid username or password.';
        }
      );
    }
  }

  onSubmitRegister() {
    if (this.registerForm.valid) {
      const payload = { ...this.registerForm.value, role: 'viewer' };
      this.http.post('https://localhost:7043/api/user', payload).subscribe(
        (response) => {
          this.successMessage = 'Registration successful! You can now log in.';
          this.errorMessage = '';
        },
        (error) => {
          console.error('Registration error:', error);
          this.errorMessage = error.error || 'Registration failed. Username may already exist.';
          this.successMessage = ''; // Effacer les messages de succès en cas d'erreur
        }
      );
    }
  }
}
