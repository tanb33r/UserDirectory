
import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { UserDetailComponent } from './user-detail/user-detail.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'user/:id', component: UserDetailComponent }
];
