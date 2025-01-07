import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-create-match',
  standalone: false,
  
  templateUrl: './create-match.component.html',
  styleUrl: './create-match.component.css'
})
export class CreateMatchComponent implements OnInit {
  matchForm: FormGroup;
  teams: any[] = [];
  playersHomeTeam: any[] = [];
  playersAwayTeam: any[] = [];
  selectedHomePlayers: number[] = [];
  selectedAwayPlayers: number[] = [];
  isSubmitting = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.matchForm = this.fb.group({
      quarters: [4, [Validators.required, Validators.min(1)]],
      quarterDuration: [10, [Validators.required, Validators.min(1)]],
      timeoutDuration: [60, [Validators.required, Validators.min(1)]],
      homeTeamId: [null, Validators.required],
      awayTeamId: [null, Validators.required],
    });
  }

  ngOnInit() {
    this.fetchTeams();
    this.onTeamSelectionChange();
  }

  fetchTeams() {
    this.http.get('https://localhost:7043/api/Team').subscribe({
      next: (data: any) => this.teams = data,
      error: (err) => console.error('Error fetching teams:', err)
    });
  }

  onTeamSelectionChange() {
    this.matchForm.get('homeTeamId')?.valueChanges.subscribe((teamId) => {
      if (teamId) this.fetchPlayersForTeam(teamId, true);
    });

    this.matchForm.get('awayTeamId')?.valueChanges.subscribe((teamId) => {
      if (teamId) this.fetchPlayersForTeam(teamId, false);
    });
  }

  fetchPlayersForTeam(teamId: number, isHomeTeam: boolean) {
    this.http.get(`https://localhost:7043/api/Player/team/${teamId}`).subscribe({
      next: (data: any) => {
        if (isHomeTeam) {
          this.playersHomeTeam = data;
          this.selectedHomePlayers = [];
        } else {
          this.playersAwayTeam = data;
          this.selectedAwayPlayers = [];
        }
      },
      error: (err) => console.error(`Error fetching players for team ${teamId}:`, err)
    });
  }

  togglePlayerSelection(playerId: number, isHomeTeam: boolean) {
    const selectedPlayers = isHomeTeam ? this.selectedHomePlayers : this.selectedAwayPlayers;
    if (selectedPlayers.includes(playerId)) {
      selectedPlayers.splice(selectedPlayers.indexOf(playerId), 1); // Supprime le joueur s'il est déjà sélectionné
    } else {
      selectedPlayers.push(playerId); // Ajoute le joueur
    }
    console.log(isHomeTeam ? 'Home Players:' : 'Away Players:', selectedPlayers); // Debug
  }



  isSameTeam() {
    const { homeTeamId, awayTeamId } = this.matchForm.value;
    return homeTeamId && awayTeamId && homeTeamId === awayTeamId;
  }

  onSubmit() {
    if (this.matchForm.invalid || this.selectedHomePlayers.length !== 5 || this.selectedAwayPlayers.length !== 5) {
      alert('Veuillez sélectionner 5 joueurs pour chaque équipe.');
      return;
    }

    const formValue = this.matchForm.value;
    console.log('Selected Home Players:', this.selectedHomePlayers);
    console.log('Selected Away Players:', this.selectedAwayPlayers);

    // Création du match
    const matchPayload = {
      quarters: parseInt(formValue.quarters, 10), // Conversion explicite
      quarterDuration: parseInt(formValue.quarterDuration, 10), // Conversion + transformation
      timeoutDuration: parseInt(formValue.timeoutDuration, 10), // Conversion explicite
      homeTeamId: parseInt(formValue.homeTeamId, 10), // Conversion explicite
      awayTeamId: parseInt(formValue.awayTeamId, 10), // Conversion explicite
      createdByUserId: 1, // Fixed ID
      encoderUserIds: [1] // Fixed list
    };

    this.isSubmitting = true;
    this.http.post('https://localhost:7043/api/Match', matchPayload).subscribe({
      next: (matchResponse: any) => {
        const matchId = matchResponse.matchId; // Assurez-vous que l'API retourne un matchId

        // Requête pour l'équipe à domicile
        const homeTeamPayload = {
          matchId: matchId,
          teamId: formValue.homeTeamId,
          playerIds: this.selectedHomePlayers,
        };

        this.http.post('https://localhost:7043/api/StartingFive', homeTeamPayload).subscribe({
          next: () => {
            console.log('Home team Starting Five added.');

            // Requête pour l'équipe à l'extérieur
            const awayTeamPayload = {
              matchId: matchId,
              teamId: formValue.awayTeamId,
              playerIds: this.selectedAwayPlayers,
            };

            this.http.post('https://localhost:7043/api/StartingFive', awayTeamPayload).subscribe({
              next: () => {
                console.log('Away team Starting Five added.');
                alert('Match et Starting Fives créés avec succès');
                this.matchForm.reset();
                this.selectedHomePlayers = [];
                this.selectedAwayPlayers = [];
                this.isSubmitting = false;
              },
              error: (err) => {
                console.error('Error adding away team Starting Five:', err);
                alert('Erreur lors de la création des Starting Fives pour l\'équipe à l\'extérieur.');
                this.isSubmitting = false;
              }
            });
          },
          error: (err) => {
            console.error('Error adding home team Starting Five:', err);
            alert('Erreur lors de la création des Starting Fives pour l\'équipe à domicile.');
            this.isSubmitting = false;
          }
        });
      },
      error: (err) => {
        console.error('Error creating match:', err);
        alert('Erreur lors de la création du match.');
        this.isSubmitting = false;
      }
    });
  }
}
