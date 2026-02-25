import { Routes } from '@angular/router';
import { Home } from './home/home';
// import { GameEdit } from './game-edit/game-edit';

export const routes: Routes = [
  { path: '', component: Home },           // Default page
//   { path: 'edit/:id', component: GameEdit }, // Edit page
//   { path: 'add', component: GameEdit }      // Add page (reusing the same component)
];