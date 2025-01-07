import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-create-team',
  standalone: false,
  
  templateUrl: './create-team.component.html',
  styleUrl: './create-team.component.css'
})
export class CreateTeamComponent {
  teamName: string = '';
  coachName: string = '';

  constructor(private http: HttpClient) { }

  createTeam() {
    const teamData = {
      teamName: this.teamName,
      coachName: this.coachName
    };

    this.http.post('https://localhost:7043/api/Team', teamData).subscribe({
      next: () => console.log('Équipe créée avec succès'),
      error: (err) => console.error('Erreur lors de la création de l\'équipe:', err)
    });
  }
}
