import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router'; 
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, NgbNavModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {

}
