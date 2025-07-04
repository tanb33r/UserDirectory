import { Component, ViewChild } from '@angular/core';
import { UserListComponent } from '../user-list/user-list.component';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'home-page',
  standalone: true,
  imports: [UserListComponent, HeaderComponent],
  template: `
    <app-header (dataSourceChange)="onDataSourceChange($event)"></app-header>
    <main class="main">
      <div class="content">
        <user-list #userList></user-list>
      </div>
    </main>
  `,
  styleUrls: ['../../app.css']
})
export class HomePageComponent {
  @ViewChild('userList') userListComponent?: UserListComponent;

  onDataSourceChange(_source: string) {
    this.userListComponent?.loadUsers();
    this.userListComponent?.loadRoles();
  }
}
