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
  private gameService = inject(GameService); 
  private ui = inject(UiService); 
  
  // games() stores the main list shown in the cards
  games = signal<Game[]>([]);
  // suggestions() stores the temporary list for the search dropdown
  suggestions = signal<Game[]>([]);
  
  currentPage = signal(1);
  pageSize = signal(6);

  ngOnInit() {
    this.fetchGames();
  }

  // Gets data from backend. If search is empty, gets all games.
  fetchGames(search: string = '') {
    this.gameService.getGames(search, this.currentPage(), this.pageSize()).subscribe({
      next: (data) => this.games.set(data),
      error: (err) => console.error('Error fetching games:', err)
    });
  }
  goToPage(direction: number) {
    this.currentPage.update(val => val + direction);
    this.fetchGames();
  }

  // Triggered on every keystroke
  onType(value: string) {
    if (value.length < 2) {
      this.suggestions.set([]); 
      if (value.length === 0) this.fetchGames();
      return;
    }

    this.gameService.getGames(value).subscribe({
      next: (data) =>{
        // this.games.set(data); use it if all the data wants to be removed here and only the searched data wants to be shown but we want to show the searched data in the dropdown so we use suggestions instead of games.
         this.suggestions.set(data);
      }
    });
  }

  selectGame(game: Game) {
    this.games.set([game]);    
    this.suggestions.set([]);
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