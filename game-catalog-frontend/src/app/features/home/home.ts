import { Component, signal, OnInit, inject } from '@angular/core'; 
import { HttpClient } from '@angular/common/http'; 
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

export interface GameUI extends Game {
  showDetails?: boolean; 
}
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
  imports: [RouterLink],
  templateUrl: './home.html',
})

export class Home implements OnInit {
  private http = inject(HttpClient); 
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
 deleteGame(id: string) {
  Swal.fire({
    title: 'Are you sure?',
    text: "You won't be able to revert this!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Yes, delete it!'
  }).then((result) => {
    if (result.isConfirmed) {
      this.http.delete(`https://localhost:7187/api/Games/${id}`).subscribe({
        next: () => {
          this.games.set(this.games().filter(g => g.id !== id));
          
          // Show the "Success" popup
          Swal.fire(
            'Deleted!',
            'The game has been removed from the catalog.',
            'success'
          );
        },
        error: (err) => {
          Swal.fire('Error', 'Something went wrong on the server.', 'error');
        }
      });
    }
  });
}
}