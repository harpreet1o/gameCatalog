import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Game, CreateGameDto, UpdateGameDto } from '../models/game.model'; // Import our new types
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  // we dont use the let or const here we use the readonly to not make it change and private cause these are injectable but dont need to be accessed outside of this class.
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7187/api/Games';

  // we create routes here and don't use async cause we get observable not the promise. 

  // we receive an array of games here  
  getGames(search?: string): Observable<Game[]> {
    const url = search ? `${this.apiUrl}?search=${search}` : this.apiUrl;
    return this.http.get<Game[]>(url);
  }

  getGameById(id: string): Observable<Game> {
    return this.http.get<Game>(`${this.apiUrl}/${id}`);
  }

  
  createGame(game: CreateGameDto): Observable<Game> {
    return this.http.post<Game>(this.apiUrl, game);
  }

  // Type-safe Update method
  updateGame(game: UpdateGameDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${game.id}`, game);
  }

  deleteGame(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}