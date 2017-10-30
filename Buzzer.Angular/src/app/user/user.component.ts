import { Component, AfterViewInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { BuzzerService } from "app/shared/buzzer.service";

@Component({
  selector: "app-user",
  templateUrl: "./user.component.html",
  styleUrls: ["./user.component.css"]
})
export class UserComponent implements AfterViewInit {
  constructor(private router: Router, private buzzer: BuzzerService) {}
  @ViewChild("inputToFocus") viewChild;

  userName: string;

  ngAfterViewInit() {
    this.viewChild.nativeElement.focus();
  }

  continue() {
    this.buzzer
      .setUserName(this.userName)
      .then(game => this.router.navigateByUrl("/button"));
  }
}
