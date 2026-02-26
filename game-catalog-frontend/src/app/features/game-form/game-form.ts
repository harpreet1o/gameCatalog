import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GameService } from '../../core/services/game.services'; 
import { CreateGameDto, UpdateGameDto } from '../../core/models/game.model';

@Component({
  selector: 'app-game-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './game-form.html',
})
export class GameForm implements OnInit {
  private gameService = inject(GameService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = signal(false);
  gameId: string | null = null;

  gameForm = new FormGroup({
    name: new FormControl('', { validators: [Validators.required], nonNullable: true }),
    description: new FormControl('', { validators: [Validators.required], nonNullable: true }),
    price: new FormControl(1, { validators: [Validators.required, Validators.min(0)], nonNullable: true }),
    genre: new FormControl('', { validators: [Validators.required], nonNullable: true }),
    gameImageURL: new FormControl<string | null>(null)
  });

  ngOnInit() {
    this.gameId = this.route.snapshot.paramMap.get('id');
    
    if (this.gameId) {
      this.isEditMode.set(true);
      this.loadGameData(this.gameId);
    }
  }

  private loadGameData(id: string) {
    this.gameService.getGameById(id).subscribe({
      next: (game) => this.gameForm.patchValue(game),
      error: (err) => console.error('Error fetching game details:', err)
    });
  }

  onSubmit() {
    if (this.gameForm.invalid) return;

    const formValue = this.gameForm.getRawValue();
    
    // Clean the URL: if it's just whitespace, set to null
    const gameImageURL = formValue.gameImageURL?.trim() || null;

    if (this.isEditMode() && this.gameId) {
      const updateData: UpdateGameDto = { ...formValue, id: this.gameId, gameImageURL };
      
      this.gameService.updateGame(updateData).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => console.error('Update failed:', err)
      });
    } else {
      const newData: CreateGameDto = { ...formValue, gameImageURL };
      
      this.gameService.createGame(newData).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => console.error('Creation failed:', err)
      });
    }
  }
}