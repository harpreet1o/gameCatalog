import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { GameForm } from './features/game-form/game-form';


export const routes: Routes = [
  { path: '', component: Home },         
  { path: 'edit/:id', component: GameForm }, 
  { path: 'add', component: GameForm }      
];