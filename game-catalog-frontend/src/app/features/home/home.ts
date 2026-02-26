import { Component, signal, OnInit, inject } from '@angular/core'; 
import { RouterLink } from '@angular/router';
import { GameService } from '../../core/services/game.service';
import { UiService } from '../../core/services/ui.service';
import { Game } from '../../core/models/game.model';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.html',
})
export class Home implements OnInit {
  // These services are injected basically like getting the functions of the another class. 
  private gameService = inject(GameService); 
  private ui = inject(UiService); 
  
  // State management using Signals
  games = signal<Game[]>([]);

  ngOnInit() {
    this.fetchGames();
  }

  fetchGames() {
    this.gameService.getGames().subscribe({
      next: (data) => this.games.set(data),
      error: (err) => console.error('Error fetching games:', err)
    });
  }

  deleteGame(id: string) {
    this.ui.confirmDelete().then((result) => {
      
      if (result.isConfirmed) {
        this.gameService.deleteGame(id).subscribe({
          next: () => {
            this.games.set(this.games().filter(g => g.id !== id));
            this.ui.showSuccess('Deleted!', 'The game has been removed.');
          },
          error: () => {
            this.ui.showError('Error', 'Something went wrong on the server.');
          }
        });
      }
      
    });
  }
}