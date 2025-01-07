import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-manage-roles',
  standalone: false,
  
  templateUrl: './manage-roles.component.html',
  styleUrl: './manage-roles.component.css'
})
export class ManageRolesComponent implements OnInit {
  users: any[] = []; // Liste des utilisateurs

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.fetchUsers();
  }

  fetchUsers() {
    // Endpoint pour récupérer tous les utilisateurs
    this.http.get('https://localhost:7043/api/user/all').subscribe(
      (response: any) => {
        this.users = response;
      },
      (error) => {
        console.error('Erreur lors de la récupération des utilisateurs', error);
      }
    );
  }

  updateRole(user: any) {
    const payload = { NewRole: user.role }; // Crée un objet avec la clé attendue par le backend
    this.http.put(`https://localhost:7043/api/user/${user.id}/role`, payload).subscribe(
      (response: any) => {
        alert('Rôle mis à jour avec succès');
      },
      (error) => {
        console.error('Erreur lors de la mise à jour du rôle', error);
      }
    );
  }
}
