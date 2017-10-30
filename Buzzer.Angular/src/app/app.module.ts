import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from "./app.component";
import { GameComponent } from "./game/game.component";
import { UserComponent } from './user/user.component';
import { ButtonComponent } from './button/button.component';
import { EndComponent } from './end/end.component';

@NgModule({
  declarations: [AppComponent, GameComponent, UserComponent, ButtonComponent, EndComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      {
        path: "",
        redirectTo: "/game",
        pathMatch: "full"
      },
      {
        path: "game",
        component: GameComponent
      },
      {
        path: "user",
        component: UserComponent
      },
      {
        path: "button",
        component: ButtonComponent
      },
      {
        path: "end",
        component: EndComponent
      }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
