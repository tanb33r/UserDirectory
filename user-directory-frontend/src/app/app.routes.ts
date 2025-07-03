
import { Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'user/:id', component: UserDetailComponent }
];
