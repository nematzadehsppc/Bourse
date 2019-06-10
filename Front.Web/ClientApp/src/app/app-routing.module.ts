import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  { path: 'uploadfile', component: UploadFileComponent },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
