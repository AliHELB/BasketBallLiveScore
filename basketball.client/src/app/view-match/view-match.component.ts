import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-view-match',
  standalone: false,
  
  templateUrl: './view-match.component.html',
  styleUrl: './view-match.component.css'
})
export class ViewMatchComponent implements OnInit {
  matchId!: number; // ID du match récupéré depuis l'URL
  events: any[] = []; // Liste des événements pour le match
  homePlayers: number[] = []; // IDs des joueurs de l'équipe à domicile
  awayPlayers: number[] = []; // IDs des joueurs de l'équipe adverse
  homeBaskets: any[] = [];
  awayBaskets: any[] = [];
  homeFaults: any[] = [];
  awayFaults: any[] = [];
  homeTeamId!: number;
  awayTeamId!: number;
  activeHomePlayers: any[] = [];
  activeAwayPlayers: any[] = [];
  timer: number = 0; // Temps restant en secondes
  private hubConnection!: signalR.HubConnection;
  private timerInterval: any;
  quarter: number = 1; 
  players: any[] = []; // Liste des joueurs
  homeSubstitutions: any[] = [];
  awaySubstitutions: any[] = [];
  homeScore: number = 0;
  awayScore: number = 0;

  constructor(
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.matchId = +this.route.snapshot.paramMap.get('matchId')!;
    this.http.get<any>(`https://localhost:7043/api/Match/${this.matchId}`).subscribe(
      (match) => {
        this.homeTeamId = match.homeTeamId;
        this.awayTeamId = match.awayTeamId;
        this.fetchPlayers(); // Récupère les joueurs des deux équipes
        this.fetchActivePlayers();

        setTimeout(() => { // Assurez-vous que les joueurs sont chargés avant de récupérer les événements
          this.fetchBaskets();
          this.fetchFaults();
          this.fetchPlayerSubstitutions(); // Récupérer les substitutions de joueurs
        }, 500);

        this.fetchTimer();
        this.setupSignalRConnection();
      },
      (error) => console.error('Erreur lors de la récupération des détails du match', error)
    );
  }

  fetchBaskets(): void {
    this.http.get<any[]>(`https://localhost:7043/api/BasketEvents/match/${this.matchId}`).subscribe(
      (data) => {
        console.log('Paniers reçus :', data);
        this.homeBaskets = data
          .filter(event => this.homePlayers.includes(event.playerId)) // Filtrer par joueurs de l'équipe à domicile
          .reverse(); // Inverser l'ordre des paniers
        this.awayBaskets = data
          .filter(event => this.awayPlayers.includes(event.playerId)) // Filtrer par joueurs de l'équipe adverse
          .reverse(); // Inverser l'ordre des paniers
        this.calculateScores();
        console.log('Paniers domicile :', this.homeBaskets);
        console.log('Paniers adversaire :', this.awayBaskets);
      },
      (error) => console.error('Erreur lors de la récupération des paniers', error)
    );
  }

  calculateScores(): void {
    this.homeScore = this.homeBaskets.reduce((total, basket) => total + basket.points, 0);
    this.awayScore = this.awayBaskets.reduce((total, basket) => total + basket.points, 0);
  }


  fetchFaults(): void {
    this.http.get<any[]>(`https://localhost:7043/api/FaultEvents/match/${this.matchId}`).subscribe(
      (data) => {
        console.log('Fautes reçues :', data);
        this.homeFaults = data
          .filter(event => this.homePlayers.includes(event.playerId)) // Filtrer par joueurs de l'équipe à domicile
          .reverse(); // Inverser l'ordre des fautes
        this.awayFaults = data
          .filter(event => this.awayPlayers.includes(event.playerId)) // Filtrer par joueurs de l'équipe adverse
          .reverse(); // Inverser l'ordre des fautes
        console.log('Fautes domicile :', this.homeFaults);
        console.log('Fautes adversaire :', this.awayFaults);
      },
      (error) => console.error('Erreur lors de la récupération des fautes', error)
    );
  }

  fetchPlayers(): void {
    this.http.get<any[]>(`https://localhost:7043/api/Player/team/${this.homeTeamId}`).subscribe(
      (data) => {
        this.homePlayers = data.map(player => player.playerId); // IDs des joueurs de l'équipe à domicile
        console.log('Joueurs domicile :', this.homePlayers);
      },
      (error) => console.error('Erreur lors de la récupération des joueurs de l\'équipe à domicile', error)
    );

    this.http.get<any[]>(`https://localhost:7043/api/Player/team/${this.awayTeamId}`).subscribe(
      (data) => {
        this.awayPlayers = data.map(player => player.playerId); // IDs des joueurs de l'équipe adverse
        console.log('Joueurs adversaire :', this.awayPlayers);
      },
      (error) => console.error('Erreur lors de la récupération des joueurs de l\'équipe adverse', error)
    );
  }

  fetchActivePlayers(): void {
    this.http.get<any[]>(`https://localhost:7043/api/StartingFive/byMatch/${this.matchId}`).subscribe(
      (data) => {
        // Filtrer les joueurs par équipe
        this.activeHomePlayers = data.filter(player => player.teamId === this.homeTeamId)
          .map(player => player.playerId);
        this.activeAwayPlayers = data.filter(player => player.teamId === this.awayTeamId)
          .map(player => player.playerId);

        console.log('Joueurs actifs domicile :', this.activeHomePlayers);
        console.log('Joueurs actifs adverse :', this.activeAwayPlayers);
      },
      (error) => console.error('Erreur lors de la récupération des joueurs actifs', error)
    );
  }


  getPlayerName(playerId: number): string {
    const player = this.players.find(p => p.playerId === playerId);
    if (player) {
      return `${player.firstName} ${player.lastName}`;
    }

    // Si le joueur n'est pas encore dans la liste, on le récupère
    this.http.get<any>(`https://localhost:7043/api/Player/${playerId}`).subscribe(
      playerData => {
        this.players.push(playerData); // Ajouter le joueur à la liste
      },
      error => {
        console.error(`Erreur lors de la récupération du joueur avec l'ID ${playerId}`, error);
      }
    );

    return 'Chargement...'; // Message temporaire pendant le chargement
  }


  fetchPlayerSubstitutions(): void {
    this.http.get<any[]>(`https://localhost:7043/api/PlayerSubstitutionEvent/match/${this.matchId}`).subscribe(
      (data) => {
        console.log('Substitutions reçues :', data);

        // Filtrer les substitutions pour chaque équipe et les inverser
        this.homeSubstitutions = data
          .filter(sub =>
            this.homePlayers.includes(sub.playerOutId) || this.homePlayers.includes(sub.playerInId)
          )
          .reverse(); // Inverser l'ordre des substitutions
        this.awaySubstitutions = data
          .filter(sub =>
            this.awayPlayers.includes(sub.playerOutId) || this.awayPlayers.includes(sub.playerInId)
          )
          .reverse(); // Inverser l'ordre des substitutions

        console.log('Substitutions domicile :', this.homeSubstitutions);
        console.log('Substitutions adversaire :', this.awaySubstitutions);
      },
      (error) => console.error('Erreur lors de la récupération des substitutions', error)
    );
  }



  startLocalTimer(): void {
    clearInterval(this.timerInterval);
    this.timerInterval = setInterval(() => {
      if (this.timer > 0) {
        this.timer--;
        this.cdr.detectChanges(); // Met à jour l'affichage
      } else {
        clearInterval(this.timerInterval);
      }
    }, 1000);
  }

  pauseLocalTimer(): void {
    clearInterval(this.timerInterval); // Arrête le timer local
  }



  fetchTimer(): void {
    this.http.get<{ remainingTime: number }>(`https://localhost:7043/api/TimerMatch/${this.matchId}/${this.quarter}`).subscribe(
      (data: { remainingTime: number }) => {
        this.timer = data.remainingTime; // Initialise le timer avec la valeur récupérée
        console.log(`Timer pour le Viewer : ${this.timer} secondes`);
      },
      (error: any) => {
        console.error('Erreur lors de la récupération du timer', error);
      }
    );
  }

  formatTimer(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${remainingSeconds.toString().padStart(2, '0')}`;
  }



  setupSignalRConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7043/hubs/match-events')
      .build();

    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection established for View Match.');
        this.hubConnection.invoke('JoinMatchGroup', this.matchId); // Rejoindre un groupe spécifique au match
      })
      .catch(err => console.error('SignalR connection error: ', err));

    // Événements spécifiques
    this.hubConnection.on('BasketEventAdded', (basketEvent: any) => {
      console.log('BasketEventAdded received:', basketEvent);
      if (this.homePlayers.includes(basketEvent.playerId)) {
        this.homeBaskets.push(basketEvent);
      } else if (this.awayPlayers.includes(basketEvent.playerId)) {
        this.awayBaskets.push(basketEvent);
      }
      this.calculateScores();
      this.cdr.detectChanges();
    });

    this.hubConnection.on('FaultEventAdded', (faultEvent: any) => {
      console.log('FaultEventAdded received:', faultEvent);
      if (this.homePlayers.includes(faultEvent.playerId)) {
        this.homeFaults.push(faultEvent);
      } else if (this.awayPlayers.includes(faultEvent.playerId)) {
        this.awayFaults.push(faultEvent);
      }
      this.cdr.detectChanges();
    });

    this.hubConnection.on('SubstitutionEventAdded', (substitutionEvent: any) => {
      if (this.homePlayers.includes(substitutionEvent.playerOutId) || this.homePlayers.includes(substitutionEvent.playerInId)) {
        this.homeSubstitutions.push(substitutionEvent);

        // Mettre à jour les joueurs actifs pour l'équipe à domicile
        this.activeHomePlayers = this.activeHomePlayers.map(playerId =>
          playerId === substitutionEvent.playerOutId ? substitutionEvent.playerInId : playerId
        );
      } else if (this.awayPlayers.includes(substitutionEvent.playerOutId) || this.awayPlayers.includes(substitutionEvent.playerInId)) {
        this.awaySubstitutions.push(substitutionEvent);

        // Mettre à jour les joueurs actifs pour l'équipe adverse
        this.activeAwayPlayers = this.activeAwayPlayers.map(playerId =>
          playerId === substitutionEvent.playerOutId ? substitutionEvent.playerInId : playerId
        );
      }
      this.cdr.detectChanges(); // Met à jour l'affichage
    });


    this.hubConnection.on('TimerStarted', (matchId: number) => {
      if (this.matchId === matchId) {
        this.startLocalTimer(); // Démarre le timer localement
      }
    });

    this.hubConnection.on('TimerUpdated', (matchId: number, remainingTime: number) => {
      if (this.matchId === matchId) {
        this.timer = remainingTime;
        this.cdr.detectChanges(); // Met à jour l'affichage
      }
    });

    this.hubConnection.on('TimerPaused', (matchId: number) => {
      if (this.matchId === matchId) {
        this.pauseLocalTimer(); // Arrête le timer localement
      }
    });

    this.hubConnection.on('QuarterChanged', (matchId: number, quarter: number) => {
      if (this.matchId === matchId) {
        this.quarter = quarter; // Met à jour le quarter
        this.fetchTimer(); // Récupère le nouveau timer
        console.log(`Passage au Quarter ${this.quarter}`);
      }
    });


  }

}
