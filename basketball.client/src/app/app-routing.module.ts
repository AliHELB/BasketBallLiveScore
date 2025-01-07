import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EncoderComponent } from './encoder/encoder.component';
import { ViewerComponent } from './viewer/viewer.component';
import { MatchEventsComponent } from './match-events/match-events.component';
import { ViewMatchComponent } from './view-match/view-match.component';
import { CreateTeamComponent } from './create-team/create-team.component';
import { CreateMatchComponent } from './create-match/create-match.component';
import { CreatePlayerComponent } from './create-player/create-player.component';
import { AuthGuard } from './auth.guard';
import { ManageRolesComponent } from './manage-roles/manage-roles.component';
import { AuthComponent } from './auth/auth.component';

const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  { path: 'auth', component: AuthComponent },
  { path: 'encoder', component: EncoderComponent, canActivate: [AuthGuard], data: { roles: ['admin', 'encoder'] } },
  { path: 'viewer', component: ViewerComponent, canActivate: [AuthGuard], data: { roles: ['admin', 'encoder', 'viewer'] } },
  { path: 'events/:matchId', component: MatchEventsComponent, canActivate: [AuthGuard], data: { roles: ['admin', 'encoder'] } },
  { path: 'view-match/:matchId', component: ViewMatchComponent, canActivate: [AuthGuard], data: { roles: ['admin', 'encoder', 'viewer'] } },
  { path: 'create-team', component: CreateTeamComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
  { path: 'create-match', component: CreateMatchComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
  { path: 'create-player', component: CreatePlayerComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
  { path: 'manage-roles', component: ManageRolesComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
