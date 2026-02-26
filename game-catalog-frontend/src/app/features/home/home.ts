import { Component, signal, OnInit, inject } from '@angular/core'; 
import { RouterLink } from '@angular/router';
import { GameService } from '../../core/services/game.services';
import { Game } from '../../core/models/game.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.html',
})
export class Home implements OnInit {
  // Inject the Service instead of HttpClient
  private gameService = inject(GameService); 
  
  // State management using Signals
  games = signal<Game[]>([]);

  ngOnInit() {
    this.fetchGames();
  }

  fetchGames() {
    // Logic moved to service, component just subscribes to the stream
    this.gameService.getGames().subscribe({
      next: (data) => this.games.set(data),
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
        // Use service for the HTTP call
        this.gameService.deleteGame(id).subscribe({
          next: () => {
            // Update local state instantly for a snappy UI
            this.games.set(this.games().filter(g => g.id !== id));
            
            Swal.fire('Deleted!', 'The game has been removed.', 'success');
          },
          error: (err) => {
            Swal.fire('Error', 'Something went wrong on the server.', 'error');
          }
        });
      }
    });
  }
}