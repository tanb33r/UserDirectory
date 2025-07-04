import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
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
  pagedUsers: User[] = [];
  page = 1;
  pageSize = 5;
  totalPages = 1;
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

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private toastr: ToastrService
  ) {
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
        next: (users) => {
          this.users = users;
          this.page = 1;
          this.updatePagedUsers();
        },
        error: (err) => (this.error = ''),
      });
  }

  updatePagedUsers() {
    if (!this.users) {
      this.pagedUsers = [];
      this.totalPages = 1;
      return;
    }
    this.totalPages = Math.max(1, Math.ceil(this.users.length / this.pageSize));
    const start = (this.page - 1) * this.pageSize;
    this.pagedUsers = this.users.slice(start, start + this.pageSize);
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.page = page;
    this.updatePagedUsers();
  }

  setPageSize(size: number) {
    this.pageSize = size;
    this.page = 1;
    this.updatePagedUsers();
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
        this.toastr.success('User created successfully!');
      },
      error: (err) => {
        this.error = 'Failed to create user.';
        this.toastr.error('Failed to create user.');
      },
    });
  }
}
