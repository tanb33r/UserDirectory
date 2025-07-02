import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, User } from '../user.service';

@Component({
  selector: 'user-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})

export class UserListComponent {
  users: User[] = [];
  error = '';
  newUser: Partial<User> = {
    firstName: '',
    lastName: '',
    company: '',
    sex: '',
    active: true,
    contact: { id: 0, phone: '', address: '', city: '', country: '' },
    role: { id: 0, name: '' },
  };

  constructor(private userService: UserService) {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe({
      next: (users) => (this.users = users),
      error: (err) => (this.error = ''),
    });
  }

  createUser() {
    if (!this.newUser.contact) this.newUser.contact = { id: 0, phone: '', address: '', city: '', country: '' };
    if (!this.newUser.role) this.newUser.role = { id: 0, name: '' };
    this.userService.createUser(this.newUser).subscribe({
      next: (user) => {
        this.users.push(user);
        this.newUser = {
          firstName: '',
          lastName: '',
          company: '',
          sex: '',
          active: true,
          contact: { id: 0, phone: '', address: '', city: '', country: '' },
          role: { id: 0, name: '' },
        };
        this.error = '';
      },
      error: (err) => (this.error = 'Failed to create user.'),
    });
  }
}
