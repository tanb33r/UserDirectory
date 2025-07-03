import { Component } from '@angular/core';
import { UserListComponent } from '../user-list/user-list.component';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'home-page',
  standalone: true,
  imports: [UserListComponent, HeaderComponent],
  template: `
    <app-header></app-header>
    <main class="main">
      <div class="content">
        <user-list></user-list>
      </div>
    </main>
  `,
  styleUrls: ['../app.css']
})
export class HomePageComponent {}
