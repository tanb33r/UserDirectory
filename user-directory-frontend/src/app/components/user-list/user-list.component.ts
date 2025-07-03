import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserService, User } from '../../services/user.service';
import { RoleService, Role } from '../../services/role.service';

@Component({
  selector: 'user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent {
  users: User[] | null = null;
  error = '';
  roles: Role[] = [];
  newUser: Partial<User> = {
    firstName: '',
    lastName: '',
    company: '',
    sex: 'M',
    active: true,
    contact: { id: 0, phone: '', address: '', city: '', country: '' },
    role: { id: 0, name: '' },
  };

  constructor(private userService: UserService, private roleService: RoleService) {
    this.loadUsers();
    this.loadRoles();
  }

  loadRoles() {
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        this.roles = roles;
        if (roles.length > 0) {
          this.newUser.role = { ...roles[0] };
        }
      },
      error: () => this.roles = []
    });
  }

  loadUsers() {
    this.users = null;
    this.userService.getUsers()
      .subscribe({
        next: (users) => (this.users = users),
        error: (err) => (this.error = ''),
      });
  }

  createUser() {
    if (!this.newUser.contact) this.newUser.contact = { id: 0, phone: '', address: '', city: '', country: '' };
    const contact = this.newUser.contact || { phone: '', address: '', city: '', country: '' };
    const payload: any = {
      firstName: this.newUser.firstName,
      lastName: this.newUser.lastName,
      active: this.newUser.active,
      company: this.newUser.company,
      sex: this.newUser.sex,
      contact: {
        phone: contact.phone,
        address: contact.address,
        city: contact.city,
        country: contact.country,
      },
      roleId: this.newUser.role?.id || 0
    };
    console.log('Creating user:', payload);
    this.userService.createUser(payload).subscribe({
      next: (user) => {
        this.loadUsers();
        this.newUser = {
          firstName: '',
          lastName: '',
          company: '',
          sex: 'M',
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
