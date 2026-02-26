import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Game, CreateGameDto, UpdateGameDto } from '../models/game.model'; // Import our new types
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7187/api/Games';

  getGames(search?: string): Observable<Game[]> {
    const url = search ? `${this.apiUrl}?search=${search}` : this.apiUrl;
    return this.http.get<Game[]>(url);
  }

  getGameById(id: string): Observable<Game> {
    return this.http.get<Game>(`${this.apiUrl}/${id}`);
  }

  // Type-safe Create method
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