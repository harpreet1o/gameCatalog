import { Component, signal, OnInit, inject, effect } from '@angular/core'; 
import { RouterLink } from '@angular/router';
import { GameService } from '../../core/services/game.service';
import { UiService } from '../../core/services/ui.service';
import { Game } from '../../core/models/game.model';

import {
	NgbDropdown,
	NgbDropdownToggle,
	NgbDropdownMenu,
	NgbDropdownItem,
	NgbDropdownButtonItem,
} from '@ng-bootstrap/ng-bootstrap/dropdown';
import { debounceSignal } from '../../core/services/signal-utils';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem, NgbDropdownButtonItem],
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
  sortBy = signal('Name'); 
  searchQuery = signal(''); // Added to keep search persistent across pages
  searchDebounced = debounceSignal(this.searchQuery, 500,''); // For debounced search input

  ngOnInit() {
    this.fetchGames();
  }

  constructor(){
    effect(() => {
    const value = this.searchDebounced();
    if (value.length < 2) {
      this.suggestions.set([]); 
      if (value.length === 0) {
        this.currentPage.set(1); // Reset page on clear
        this.fetchGames();
      }
      return;
    }
    this.gameService.getGames(value, 1, 5, this.sortBy(), this.isDescending()).subscribe({
      next: (data) => {
        // console.log('Search suggestions:', data);
        this.suggestions.set(data)
      }
    });
  })
  }
  
  fetchGames() {
    this.gameService.getGames(this.searchQuery(), this.currentPage(), this.pageSize(), this.sortBy(),this.isDescending()).
    subscribe({
     
      next: (data) =>{this.games.set(data); 
        console.log('Fetched games:', data)

      }, error: (err) => console.error('Error fetching games:', err)
    });
  }
  onType(value: string) {
    this.searchQuery.set(value); // Update the global search state

  }

// This function is called when new sort column is selected and resetting to first page and also always ascending 
  setSortColumn(column: string) {
  if (this.sortBy() !== column) {
    this.sortBy.set(column);
    this.isDescending.set(false);
    this.currentPage.set(1);
    this.fetchGames();
  }
}

toggleDirection() {
  this.isDescending.update(val => !val);
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
// using the sweetaleart2 here 
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