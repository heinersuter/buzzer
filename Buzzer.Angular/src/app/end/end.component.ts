import { Component, AfterViewInit, ViewChild, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BuzzerService } from "app/shared/buzzer.service";

@Component({
  selector: "app-end",
  templateUrl: "./end.component.html",
  styleUrls: ["./end.component.css"]
})
export class EndComponent implements OnInit, AfterViewInit {
  constructor(private router: Router, public buzzer: BuzzerService) {}

  @ViewChild("inputToFocus") viewChild;

  ngOnInit(): void {
    this.buzzer.getNotification(game => {
      if (game.winner === null) {
        this.router.navigateByUrl("/button");
      }
    });
  }

  ngAfterViewInit() {
    if (this.viewChild) {
      this.viewChild.nativeElement.focus();
    }
  }

  reset() {
    this.buzzer.reset().then(game => this.router.navigateByUrl("/button"));
  }
}
