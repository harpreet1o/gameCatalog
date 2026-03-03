import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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
  getGames(search?: string, pageNumber?: number, pageSize?: number): Observable<Game[]> {
    console.log('Fetching games with search:', search, 'page:', pageNumber, 'pageSize:', pageSize);
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    if (pageNumber !== undefined && pageSize !== undefined) {
      params = params.set('pageNumber', pageNumber);
      params = params.set('pageSize', pageSize);
    }

    return this.http.get<Game[]>(this.apiUrl, { params });
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