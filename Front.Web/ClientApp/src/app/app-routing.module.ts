import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { LoginComponent } from './components/login/login.component';
import { AdminLayoutComponent } from './components/layout/AdminLayout.component';
import { LayoutComponent } from './components/layout/layout.component';

const routes: Routes = [

  { path: '', component: LoginComponent },

  {
    path: 'admin', component: AdminLayoutComponent, children: [
      { path: 'uploadfile', component: UploadFileComponent }
    ]
  },
  {
    path: '', component: LayoutComponent, children: [
      { path: 'login', component: LoginComponent }
    ]
  },

  { path: '**', redirectTo: '' }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
