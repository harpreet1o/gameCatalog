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
  
  games = signal<Game[]>([]);
  suggestions = signal<Game[]>([]);
  
  // State Signals
  currentPage = signal(1);
  pageSize = signal(6);
  isDescending = signal(false);
  sortBy = signal('name'); 
  searchQuery = signal(''); // Added to keep search persistent across pages

  ngOnInit() {
    this.fetchGames();
  }

  
  fetchGames(search: string = '') {
    this.gameService.getGames(search, this.currentPage(), this.pageSize(), this.sortBy(),this.isDescending()).
    subscribe({
     
      next: (data) =>{this.games.set(data); 
        console.log('Fetched games:', data)

      }, error: (err) => console.error('Error fetching games:', err)
    });
  }
  onType(value: string) {
    this.searchQuery.set(value); // Update the global search state

    if (value.length < 2) {
      this.suggestions.set([]); 
      if (value.length === 0) {
        this.currentPage.set(1); // Reset page on clear
        this.fetchGames();
      }
      return;
    }

    // Suggestions should respect the same sort/page rules for consistency
    this.gameService.getGames(value, 1, 5, this.sortBy(), this.isDescending()).subscribe({
      next: (data) => this.suggestions.set(data)
    });
  }

  toggleSort(column: string) {
    if (this.sortBy() === column) {
      this.isDescending.update(val => !val);
    } else {
      this.sortBy.set(column);
      this.isDescending.set(false);
    }
    this.currentPage.set(1);
    this.fetchGames();
  }

  goToPage(direction: number) {
    this.currentPage.update(val => val + direction);
    this.fetchGames(); 
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