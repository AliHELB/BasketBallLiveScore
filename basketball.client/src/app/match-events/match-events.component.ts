import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-match-events',
  standalone: false,

  templateUrl: './match-events.component.html',
  styleUrl: './match-events.component.css'
})
export class MatchEventsComponent implements OnInit {
  matchId!: number;
  quarter: number = 1;
  quarterDuration: number = 10;
  numberOfQuarters: number = 0;
  homeTeamId: number = 0;
  awayTeamId: number = 0;
  homePlayers: any[] = [];
  awayPlayers: any[] = [];
  selectedHomePlayerBasket: number | null = null;
  selectedAwayPlayerBasket: number | null = null;
  selectedHomePlayerFault: number | null = null;
  selectedAwayPlayerFault: number | null = null;
  playerId: number | null = null;
  eventType: string = 'basket';
  points: number | null = null;
  faultType: string | null = null;
  baskets: any[] = [];
  faults: any[] = [];
  substitutions: any[] = [];
  timer: number = 0; // Temps restant en secondes
  timerInterval: any;

  activeHomePlayers: any[] = [];
  changeOutHomePlayer: number | null = null; // Joueur qui sort pour l'équipe à domicile
  changeInHomePlayer: number | null = null;  // Joueur qui entre pour l'équipe à domicile
  availableHomePlayers: any[] = [];          // Joueurs disponibles pour entrer dans l'équipe à domicile

  activeAwayPlayers: any[] = [];
  changeOutAwayPlayer: number | null = null; // Joueur qui sort pour l'équipe adverse
  changeInAwayPlayer: number | null = null;  // Joueur qui entre pour l'équipe adverse
  availableAwayPlayers: any[] = [];          // Joueurs disponibles pour entrer dans l'équipe adverse
  availablePlayers: any[] = [];
  private hubConnection!: signalR.HubConnection;

  constructor(private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit(): void {
    this.matchId = +this.route.snapshot.paramMap.get('matchId')!;
    this.fetchMatchDetails();
    this.fetchBaskets();
    this.fetchFaults();
    this.fetchSubstitutions();
    this.fetchTimer();
    this.setupSignalRConnection();
  }

  fetchMatchDetails(): void {
    this.http.get<any[]>('https://localhost:7043/api/Match').subscribe(
      (matches) => {
        const match = matches.find((m: any) => m.matchId === this.matchId);
        if (match) {
          this.numberOfQuarters = match.numberOfQuarters;
          this.quarterDuration = match.quarterDuration;
          this.homeTeamId = match.homeTeamId;
          this.awayTeamId = match.awayTeamId;

          // Charger les joueurs actifs et disponibles pour l'équipe à domicile
          this.fetchPlayersByTeam(this.homeTeamId, 'home');
          this.fetchStartingFivePlayers(this.homeTeamId, 'home');
          setTimeout(() => this.fetchAvailablePlayersForTeam(this.homeTeamId, 'home'), 500);

          // Charger les joueurs actifs et disponibles pour l'équipe adverse
          this.fetchPlayersByTeam(this.awayTeamId, 'away');
          this.fetchStartingFivePlayers(this.awayTeamId, 'away');
          setTimeout(() => this.fetchAvailablePlayersForTeam(this.awayTeamId, 'away'), 500);
        } else {
          console.error('Match non trouvé');
        }
      },
      (error) => {
        console.error('Erreur lors de la récupération des matchs', error);
      }
    );
  }

  fetchPlayersByTeam(teamId: number, teamType: string): void {
    this.http.get<any[]>(`https://localhost:7043/api/Player/team/${teamId}`).subscribe(
      (players) => {
        if (teamType === 'home') {
          this.homePlayers = players.map(player => ({
            ...player,
            fullName: `${player.firstName} ${player.lastName}`
          }));
        } else if (teamType === 'away') {
          this.awayPlayers = players.map(player => ({
            ...player,
            fullName: `${player.firstName} ${player.lastName}`
          }));
        }
      },
      (error) => {
        console.error(`Erreur lors de la récupération des joueurs pour l'équipe ${teamId}`, error);
      }
    );
  }

  getPlayerName(playerId: number): string {
    const player = [...this.homePlayers, ...this.awayPlayers].find(p => p.playerId === playerId);
    return player ? `${player.firstName} ${player.lastName}` : 'Inconnu';
  }


  fetchBaskets(): void {
    this.http.get<any[]>(`https://localhost:7043/api/BasketEvents/match/${this.matchId}`).subscribe(
      (data) => this.baskets = data
    );
  }
  fetchSubstitutions(): void {
    this.http.get<any[]>(`https://localhost:7043/api/PlayerSubstitutionEvent/match/${this.matchId}`).subscribe(
      (data) => {
        this.substitutions = data;
      },
      (error) => {
        console.error('Erreur lors de la récupération des substitutions :', error);
      }
    );
  }

  fetchFaults(): void {
    this.http.get<any[]>(`https://localhost:7043/api/FaultEvents/match/${this.matchId}`).subscribe(
      (data) => this.faults = data
    );
  }
  addBasketEvent(team: 'home' | 'away'): void {
    const playerId = team === 'home' ? this.selectedHomePlayerBasket : this.selectedAwayPlayerBasket;

    if (!playerId) {
      alert('Veuillez sélectionner un joueur.');
      console.log('Aucun joueur sélectionné pour le panier.');
      return;
    }

    if (!this.points || this.points <= 0) {
      alert('Veuillez sélectionner des points valides.');
      console.log('Points non valides ou non sélectionnés.');
      return;
    }

    const basketEvent = {
      matchId: this.matchId,
      playerId,
      quarter: this.quarter,
      points: this.points,
      timer: this.timer,
    };

    this.http.post('https://localhost:7043/api/BasketEvents', basketEvent).subscribe(
      () => {
        this.fetchBaskets();
        this.resetForm();
        alert('Panier ajouté avec succès !');
      },
      (error) => {
        console.error('Erreur lors de l’ajout du panier :', error);
      }
    );
  }


  addFaultEvent(team: 'home' | 'away'): void {
    const playerId = team === 'home' ? this.selectedHomePlayerFault : this.selectedAwayPlayerFault;

    console.log('Selected Player ID:', playerId); // Debug
    console.log('Selected Fault Type:', this.faultType); // Debug

    if (!playerId || !this.faultType) {
      alert('Veuillez sélectionner un joueur et un type de faute.');
      return;
    }

    const faultEvent = {
      matchId: this.matchId,
      playerId,
      quarter: this.quarter,
      faultType: this.faultType,
      timer: this.timer,
    };

    this.http.post('https://localhost:7043/api/FaultEvents', faultEvent).subscribe(
      () => {
        this.fetchFaults();
        this.resetForm();
        alert('Faute ajoutée avec succès !');
      },
      (error) => {
        console.error('Erreur lors de l’ajout de la faute :', error);
      }
    );
  }



  fetchTimer(): void {
    this.http.get<any>(`https://localhost:7043/api/TimerMatch/${this.matchId}/${this.quarter}`).subscribe(
      (data) => {
        this.timer = data.remainingTime; // Initialise le timer avec la valeur récupérée
        console.log(`Timer initialisé : ${this.timer} secondes`);
      },
      (error) => {
        console.error('Erreur lors de la récupération du timer', error);
      }
    );
  }

  fetchStartingFivePlayers(teamId: number, teamType: 'home' | 'away'): void {
    console.log('1');
    this.http.get<any[]>(`https://localhost:7043/api/StartingFive/byMatchAndTeam/${this.matchId}/${teamId}`).subscribe(
      (startingFive) => {
        console.log(startingFive);
        // Extraire les IDs des joueurs du Starting Five
        const startingFivePlayerIds = startingFive.map(sf => sf.playerId);
        console.log('startingFivePlayerIds :'+startingFivePlayerIds);

        // Filtrer les joueurs dans l'équipe qui correspondent aux IDs du Starting Five
        if (teamType === 'home') {
          this.activeHomePlayers = this.homePlayers.filter(player => startingFivePlayerIds.includes(player.playerId));
        } else if (teamType === 'away') {
          this.activeAwayPlayers = this.awayPlayers.filter(player => startingFivePlayerIds.includes(player.playerId));
        }

        console.log(`Active players for ${teamType} team:`, teamType === 'home' ? this.activeHomePlayers : this.activeAwayPlayers);
      },
      (error) => {
        console.error('Erreur lors de la récupération des joueurs du starting five:', error);
      }
    );
  }


  fetchAvailablePlayersForTeam(teamId: number, teamType: 'home' | 'away'): void {
    console.log('2');
    this.http.get<any[]>(`https://localhost:7043/api/Player/team/${teamId}`).subscribe(
      (players) => {
        // Récupérer les IDs des joueurs actifs (dans le Starting Five)
        const activePlayerIds = teamType === 'home'
          ? this.activeHomePlayers.map(p => p.playerId)
          : this.activeAwayPlayers.map(p => p.playerId);

        // Filtrer les joueurs qui ne sont pas actifs
        const availablePlayers = players.filter(player => !activePlayerIds.includes(player.playerId));

        if (teamType === 'home') {
          this.availableHomePlayers = availablePlayers;
        } else {
          this.availableAwayPlayers = availablePlayers;
        }

        console.log(`Available players for ${teamType} team:`, availablePlayers);
      },
      (error) => {
        console.error('Erreur lors de la récupération des joueurs disponibles:', error);
      }
    );
  }



  submitPlayerChange(team: 'home' | 'away'): void {
    const teamId = team === 'home' ? this.homeTeamId : this.awayTeamId;
    const changeOutPlayer = team === 'home' ? this.changeOutHomePlayer : this.changeOutAwayPlayer;
    const changeInPlayer = team === 'home' ? this.changeInHomePlayer : this.changeInAwayPlayer;

    if (!changeOutPlayer || !changeInPlayer) {
      alert('Veuillez sélectionner un joueur qui sort et un joueur qui entre.');
      return;
    }

    const changePayload = {
      matchId: this.matchId,
      teamId: teamId,
      playerId: changeOutPlayer,
      playerIds: [changeInPlayer]
    };

    this.http.put(`https://localhost:7043/api/StartingFive/byMatchTeamAndPlayer/${this.matchId}/${teamId}/${changeOutPlayer}`, changePayload, { responseType: 'text' }).subscribe(
      (response: string) => {
        alert(response);

        // Enregistrement de l'événement de substitution
        const substitutionEventPayload = {
          matchId: this.matchId,
          playerOutId: changeOutPlayer,
          playerInId: changeInPlayer,
          quarter: this.quarter,
          timer: this.timer
        };

        this.addPlayerSubstitutionEvent(substitutionEventPayload);

        this.loadTeamData(team, teamId); // Rafraîchit les données
        this.fetchSubstitutions();
        this.resetSelection(team);
      },
      (error) => {
        console.error('Erreur lors du changement de joueur :', error);
        alert('Erreur lors du changement de joueur.');
      }
    );
  }
  loadTeamData(team: 'home' | 'away', teamId: number): void {
    this.fetchPlayersByTeam(teamId, team); // Charger tous les joueurs de l'équipe
    this.fetchStartingFivePlayers(teamId, team); // Charger les joueurs actifs (Starting Five)
    this.fetchAvailablePlayersForTeam(teamId, team); // Charger les joueurs disponibles
  }

  addPlayerSubstitutionEvent(eventPayload: any): void {
    this.http.post('https://localhost:7043/api/PlayerSubstitutionEvent', eventPayload).subscribe(
      () => {
        console.log('Substitution enregistrée avec succès !');
      },
      (error) => {
        console.error('Erreur lors de l’enregistrement de la substitution :', error);
      }
    );
  }
  resetSelection(team: 'home' | 'away'): void {
    if (team === 'home') {
      this.changeOutHomePlayer = null;
      this.changeInHomePlayer = null;
    } else if (team === 'away') {
      this.changeOutAwayPlayer = null;
      this.changeInAwayPlayer = null;
    }
  }

  resetForm(): void {
    this.selectedHomePlayerBasket = null;
    this.selectedAwayPlayerBasket = null;
    this.selectedHomePlayerFault = null;
    this.selectedAwayPlayerFault = null;
    this.points = null;
    this.faultType = null;
  }



  startTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }

    this.timerInterval = setInterval(() => {
      if (this.timer > 0) {
        this.timer--;

        // Mettre à jour la base de données
        this.http.post('https://localhost:7043/api/TimerMatch', {
          matchId: this.matchId,
          quarter: this.quarter,
          remainingTime: this.timer
        }).subscribe();

        // Envoyer la mise à jour aux Viewers
        this.hubConnection.invoke('UpdateTimer', this.matchId, this.timer)
          .catch(err => console.error('Erreur lors de l’envoi de la mise à jour du timer', err));
      } else {
        clearInterval(this.timerInterval);
        this.transitionToNextQuarter();
      }
    }, 1000);

    this.hubConnection.invoke('StartTimer', this.matchId)
      .catch(err => console.error('Erreur lors de l’envoi du démarrage du timer', err));
  }

  resumeTimer(): void {
    this.startTimer();

    this.hubConnection.invoke('ResumeTimer', this.matchId)
      .catch(err => console.error('Erreur lors de la reprise du timer', err));
  }

  pauseTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }

    // Mettre à jour la base de données
    this.http.post('https://localhost:7043/api/TimerMatch', {
      matchId: this.matchId,
      quarter: this.quarter,
      remainingTime: this.timer
    }).subscribe();

    // Notifier les Viewers de la pause
    this.hubConnection.invoke('PauseTimer', this.matchId)
      .catch(err => console.error('Erreur lors de l’envoi de la pause du timer', err));
  }

  formatTimer(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${remainingSeconds.toString().padStart(2, '0')}`;
  }

  transitionToNextQuarter(): void {
    if (this.quarter < this.numberOfQuarters) {
      this.quarter++; // Passer au quarter suivant

      // Récupérer le timer pour le nouveau quarter
      this.fetchTimer();

      // Notifier les Viewers du changement de quarter
      this.hubConnection.invoke('QuarterChanged', this.matchId, this.quarter)
        .catch(err => console.error('Erreur lors de la notification du changement de quarter', err));
    } else {
      console.log('Le match est terminé.');
    }
  }

  setupSignalRConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7043/hubs/match-events')
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connection established for Match Events.'))
      .catch(err => console.error('SignalR connection error: ', err));
  }
}
