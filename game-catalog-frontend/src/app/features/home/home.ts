import { Component, signal, inject, effect, untracked, DestroyRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { GameService } from '../../core/services/game.service';
import { UiService } from '../../core/services/ui.service';
import { Game } from '../../core/models/game.model';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

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
  imports: [RouterLink, FormsModule, NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem, NgbDropdownButtonItem],
  templateUrl: './home.html',
})
export class Home {
  private gameService = inject(GameService); 
  private ui = inject(UiService); 
  private destroyRef = inject(DestroyRef);
  private isSelecting = false;
  
  games = signal<Game[]>([]);
  suggestions = signal<Game[]>([]);
  
  // State Signals
  currentPage = signal(1);
  pageSize = signal(6);
  isDescending = signal(false);
  sortBy = signal('Name'); 
  searchQuery = signal(''); // Added to keep search persistent across pages
  searchDebounced = debounceSignal(this.searchQuery, 500,''); // For debounced search input
  activeSearch = signal('');
  reloadTrigger = signal(0); // Forces reload even when activeSearch is already ''

  private loadGamesEffect = effect(() => {
    const page = this.currentPage();
    const size = this.pageSize();
    const sort = this.sortBy();
    const desc = this.isDescending();
    const search = this.activeSearch();
    const _ = this.reloadTrigger(); // Reading this signal makes the effect depend on it

    this.gameService.getGames(search, page, size, sort, desc)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => this.games.set(data),
        error: () => this.ui.showError('Error', 'Could not load games.')
      });
  });

  private searchSuggestionsEffect = effect(() => { // the effect can't have another signal so using untracked 
    const input = this.searchDebounced();
    const rawValue = untracked(() => this.searchQuery());
    if(this.isSelecting){
      this.isSelecting = false;
      return;
    }
    if (!rawValue) {
      this.suggestions.set([]);
      console.log("empty search, resetting to first page");
      this.clearSearch();
      return;
    }

    // Ignore single character searches
    if (rawValue.length < 2) {
      return;
    }
    this.gameService.getGames(input, 1, 5, this.sortBy(), this.isDescending())
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => {
          this.suggestions.set(data)
        }
      });
  });
 
  onEnter() {
    this.activeSearch.set(this.searchQuery());
    this.currentPage.set(1); // Reset to first page on new search
    this.suggestions.set([]); // Clear suggestions on search
  }

  clearSearch() {
    this.searchQuery.set('');
    this.activeSearch.set('');
    this.currentPage.set(1);
    this.reloadTrigger.update(v => v + 1); // Forces a fresh reload
  }

  // This function is called when new sort column is selected and resetting to first page and also always ascending 
  setSortColumn(column: string) {
    if (this.sortBy() !== column) {
      this.sortBy.set(column);
      this.isDescending.set(false);
      this.currentPage.set(1);
    }
  }

  toggleDirection() {
    this.isDescending.update(val => !val);
    this.currentPage.set(1);
  }

  goToPage(direction: number) {
    this.currentPage.update(val => val + direction);
    console.log("current page: ", this.currentPage())
  }

  selectGame(game: Game) {
    this.games.set([game]);    
    this.searchQuery.set(game.name);
    this.suggestions.set([]);
    this.isSelecting = true; // To prevent the bug causing the suggestion to reappear after selecting a game it appears again
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