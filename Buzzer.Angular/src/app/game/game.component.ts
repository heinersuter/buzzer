import { Component, AfterViewInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { BuzzerService } from "app/shared/buzzer.service";

@Component({
  selector: "app-game",
  templateUrl: "./game.component.html",
  styleUrls: ["./game.component.css"]
})
export class GameComponent implements AfterViewInit {
  constructor(private router: Router, private buzzer: BuzzerService) {}
  @ViewChild("inputToFocus") viewChild;

  gameName: string = "";

  ngAfterViewInit() {
    this.viewChild.nativeElement.focus();
  }

  continue() {
    this.buzzer
      .setGameName(this.gameName)
      .then(game => this.router.navigateByUrl("/user"));
  }
}
