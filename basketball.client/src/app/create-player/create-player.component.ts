import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Player {
  firstName: string;
  lastName: string;
  playerNumber: number | null;
  teamId: number | null;
}

@Component({
  selector: 'app-create-player',
  standalone: false,
  
  templateUrl: './create-player.component.html',
  styleUrl: './create-player.component.css'
})
export class CreatePlayerComponent implements OnInit {
  player: Player = {
    firstName: '',
    lastName: '',
    playerNumber: null,
    teamId: null
  };

  teams: { teamId: number; teamName: string }[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchTeams();
  }

  fetchTeams(): void {
    this.http.get<{ teamId: number; teamName: string }[]>('https://localhost:7043/api/Team').subscribe((data) => {
      this.teams = data;
    });
  }

  onSubmit(): void {
    this.http.post('https://localhost:7043/api/Player', this.player).subscribe({
      next: () => alert('Joueur créé avec succès !'),
      error: () => alert('Une erreur s\'est produite.')
    });
  }
}
