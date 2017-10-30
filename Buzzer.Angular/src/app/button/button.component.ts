import { Component, AfterViewInit, ViewChild, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BuzzerService } from "app/shared/buzzer.service";

@Component({
  selector: "app-button",
  templateUrl: "./button.component.html",
  styleUrls: ["./button.component.css"]
})
export class ButtonComponent implements OnInit, AfterViewInit {
  constructor(private router: Router, private buzzer: BuzzerService) {}

  @ViewChild("inputToFocus") viewChild;

  ngOnInit(): void {
    this.buzzer.getNotification(game => {
      if (game.winner !== null) {
        this.router.navigateByUrl("/end");
      }
    });
  }

  ngAfterViewInit() {
    this.viewChild.nativeElement.focus();
  }

  tryToWin() {
    this.buzzer.tryToWin().then(game => this.router.navigateByUrl("/end"));
  }
}
