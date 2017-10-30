import { Injectable } from "@angular/core";
import { IGame } from "app/shared/game.model";
import { HttpClient } from "@angular/common/http";
import { HubConnection } from "@aspnet/signalr-client";
import { Observable } from "rxjs/Observable";
import { environment } from "environments/environment";

@Injectable()
export class BuzzerService {
  private readonly apiUrl: string = `${environment.baseUrl}/api`;
  private readonly notifyUrl: string = `${environment.baseUrl}/signalR`;
  private game: IGame;
  private currentUserName: string;

  constructor(private http: HttpClient) {}

  get gameName(): string {
    return this.game ? this.game.name : null;
  }

  get userName(): string {
    return this.currentUserName;
  }

  get allUsers(): string[] {
    return this.game ? this.game.users : null;
  }

  get winner(): string {
    return this.game ? this.game.winner : null;
  }

  get isWinner(): boolean {
    return this.game.winner === this.currentUserName;
  }

  setGameName(gameName: string): Promise<IGame> {
    let promise = this.http
      .put<IGame>(`${this.apiUrl}/games/${gameName}`, null)
      .toPromise();
    promise.then(game => (this.game = game)).catch(error => console.log(error));
    this.connectToSignalRHub(gameName);
    return promise;
  }

  setUserName(userName: string): Promise<IGame> {
    let promise = this.http
      .post<IGame>(
        `${this.apiUrl}/games/${this.game.name}/users/${userName}`,
        null
      )
      .toPromise();
    promise
      .then(game => {
        this.game = game;
        this.currentUserName = userName;
      })
      .catch(error => console.log(error));
    return promise;
  }

  tryToWin(): Promise<IGame> {
    let promise = this.http
      .post<IGame>(
        `${this.apiUrl}/games/${this.game.name}/winner/${this.currentUserName}`,
        null
      )
      .toPromise();
    promise.then(game => (this.game = game)).catch(error => console.log(error));
    return promise;
  }

  reset(): Promise<IGame> {
    let promise = this.http
      .delete<IGame>(
        `${this.apiUrl}/games/${this.game.name}/winner/${this.currentUserName}`
      )
      .toPromise();
    promise.then(game => (this.game = game)).catch(error => console.log(error));
    return promise;
  }

  getNotification(handler: (game: IGame) => void) {
    let connection = new HubConnection(`${this.notifyUrl}/games`);
    connection.on(this.game.name, handler);
    connection.start();
  }

  private connectToSignalRHub(gameName: string) {
    let connection = new HubConnection(`${this.notifyUrl}/games`);
    connection.on(gameName, game => {
      this.game = game;
    });
    connection.start().then(() =>
      connection.onclose(() => {
        setTimeout(() => connection.start(), 500);
      })
    );
  }
}
