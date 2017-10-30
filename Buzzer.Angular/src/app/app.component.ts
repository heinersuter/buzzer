import { Component } from "@angular/core";
import { BuzzerService } from "app/shared/buzzer.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  providers: [BuzzerService]
})
export class AppComponent {
  constructor(public buzzer: BuzzerService) {}
}
