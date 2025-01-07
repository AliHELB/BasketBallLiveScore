import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from './auth.interceptor';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EncoderComponent } from './encoder/encoder.component';
import { ViewerComponent } from './viewer/viewer.component';
import { MatchEventsComponent } from './match-events/match-events.component';
import { ViewMatchComponent } from './view-match/view-match.component';
import { CreateTeamComponent } from './create-team/create-team.component';
import { CreateMatchComponent } from './create-match/create-match.component';
import { CreatePlayerComponent } from './create-player/create-player.component';
import { ManageRolesComponent } from './manage-roles/manage-roles.component';
import { AuthComponent } from './auth/auth.component';

@NgModule({
  declarations: [
    AppComponent,
    EncoderComponent,
    ViewerComponent,
    MatchEventsComponent,
    ViewMatchComponent,
    CreateTeamComponent,
    CreateMatchComponent,
    CreatePlayerComponent,
    ManageRolesComponent,
    AuthComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, ReactiveFormsModule,
    FormsModule, AppRoutingModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
