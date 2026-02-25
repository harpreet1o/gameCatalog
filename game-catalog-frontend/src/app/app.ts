import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Home } from './home/home';
import { Header } from './components/header/header';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,  Header],
  template:`
  <app-header/>
  <main>
 <router-outlet />
  <main>`,
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('game-catalog-frontend');
}
 