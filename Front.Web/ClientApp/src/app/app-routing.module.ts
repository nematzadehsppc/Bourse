import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { LoginComponent } from './components/login/login.component';
import { AdminLayoutComponent } from './components/layout/AdminLayout.component';
import { LayoutComponent } from './components/layout/layout.component';


// مسیر یابی سرویس ها
const routes: Routes = [

  { path: '', component: LoginComponent },

  {
    // ip:port/admin/uploadfile
    path: 'admin', component: AdminLayoutComponent, children: [
      { path: 'uploadfile', component: UploadFileComponent }
    ]
  },
  {
    // ip:port or ip:port/login or ip:port/invalidDirectory
    path: '', component: LayoutComponent, children: [
      { path: 'login', component: LoginComponent }
    ]
  },

  // ip:port/invalidDirectory redirect to login page
  { path: '**', redirectTo: '' }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
