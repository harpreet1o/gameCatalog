import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-game-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './game-form.html',
})
export class GameForm implements OnInit {
  private http = inject(HttpClient);
  private router = inject(Router);
  private route = inject(ActivatedRoute); // Needed to read the ID from URL

  apiUrl = 'https://localhost:7187/api/Games';
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
    // Check if there's an 'id' in the route params
    this.gameId = this.route.snapshot.paramMap.get('id');
    
    if (this.gameId) {
      this.isEditMode.set(true);
      this.fetchGameData(this.gameId);
    }
  }

  fetchGameData(id: string) {
    this.http.get<any>(`${this.apiUrl}/${id}`).subscribe({
      next: (game) => {
        // patchValue fills the form with the existing data
        this.gameForm.patchValue(game);
      },
      error: (err) => console.error('Error fetching game details:', err)
    });
  }

  onSubmit() {
    if (this.gameForm.valid) {
      const formValue = this.gameForm.getRawValue();
      
      const payload = {
        ...formValue,
        gameImageURL: formValue.gameImageURL?.trim() ? formValue.gameImageURL : null
      };

      if (this.isEditMode()) {
        // EDIT MODE: Use PUT
        this.http.put(`${this.apiUrl}/${this.gameId}`, payload)
          .subscribe({
            next: () => this.router.navigate(['/']),
            error: (err) => console.error('Error updating game:', err)
          });
      } else {
        // ADD MODE: Use POST
        this.http.post(this.apiUrl, payload)
          .subscribe({
            next: () => this.router.navigate(['/']),
            error: (err) => console.error('Error adding game:', err)
          });
      }
    }
  }
}