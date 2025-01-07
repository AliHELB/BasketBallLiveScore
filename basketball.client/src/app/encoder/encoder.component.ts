import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-encoder',
  standalone: false,
  
  templateUrl: './encoder.component.html',
  styleUrl: './encoder.component.css'
})
export class EncoderComponent implements OnInit {
  matches: any[] = [];
  teams: { [key: number]: string } = {}; // Associe les IDs d'équipe à leurs noms

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchMatches();
  }

  fetchMatches(): void {
    this.http.get<any[]>('https://localhost:7043/api/Match').subscribe(
      (matches) => {
        this.matches = matches;
        this.fetchTeamNames();
      },
      (error) => {
        console.error('Erreur lors de la récupération des matchs', error);
      }
    );
  }

  fetchTeamNames(): void {
    const teamIds = new Set<number>();
    this.matches.forEach(match => {
      teamIds.add(match.homeTeamId);
      teamIds.add(match.awayTeamId);
    });

    teamIds.forEach(id => {
      this.http.get<any>(`https://localhost:7043/api/Team/${id}`).subscribe(
        (team) => {
          this.teams[id] = team.teamName;
        },
        (error) => {
          console.error(`Erreur lors de la récupération de l'équipe avec l'ID ${id}`, error);
        }
      );
    });
  }

  getTeamName(id: number): string {
    return this.teams[id] || 'Chargement...';
  }
}
