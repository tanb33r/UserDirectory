import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, User } from '../../services/user.service';
import { RoleService, Role } from '../../services/role.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'user-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  user: User | null = null;
  roles: Role[] = [];
  error = '';
  isEditing = false;
  showDeleteModal = false;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadUser(id);
    this.loadRoles();
  }

  loadUser(id: number) {
    this.userService.getUser(id).subscribe({
      next: (user) => (this.user = user),
      error: () => (this.error = 'Failed to load user')
    });
  }

  loadRoles() {
    this.roleService.getRoles().subscribe({
      next: (roles) => (this.roles = roles),
      error: () => (this.roles = [])
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }

  enableEdit() {
    this.isEditing = true;
  }

  updateUser() {
    if (!this.user) return;
    this.userService.updateUser(this.user).subscribe({
      next: (updatedUser) => {
        if (this.user && this.roles.length > 0) {
          const newRole = this.roles.find(r => r.id === updatedUser!.role.id);
          if (newRole) this.user.role = { ...newRole };
        }
        this.isEditing = false;
        this.error = '';
      },
      error: () => (this.error = 'Failed to update user')
    });
  }

  confirmDelete() {
    this.showDeleteModal = true;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  deleteUser() {
    if (!this.user) return;
    this.userService.deleteUser(this.user.id).subscribe({
      next: () => this.goBack(),
      error: () => (this.error = 'Failed to delete user')
    });
    this.showDeleteModal = false;
  }
}
