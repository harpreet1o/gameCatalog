import { Component, signal, OnInit, inject } from '@angular/core'; 
import { HttpClient } from '@angular/common/http'; 


export interface Game {
  id: string;            
  name: string;          
  description: string;
  price: number;
  genre: string;
  gameImageURL: string;  
}
@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.html',
})

export class Home implements OnInit {
  private http = inject(HttpClient); // "Inject" the tool to make web calls
  games = signal<Game[]>([]);

  ngOnInit() {
    this.fetchGames();
  }

  fetchGames() {
    // Make the actual call to your .NET/Server API
    this.http.get<Game[]>('https://localhost:7187/api/Games')
      .subscribe({
        next: (data) => {
          console.log("data received", data);
          this.games.set(data)},
        error: (err) => console.error('Error fetching games:', err)
      });
  }
}