import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { LoginComponent } from './components/login/login.component'
import { RegisterComponent } from './components/register/register.component'
import { HttpClientModule } from '@angular/common/http';
import { DpDatePickerModule } from 'ng2-jalali-date-picker';
import { AdminFooterComponent } from './components/adminLTE/admin.footer.component';
import { AdminHeaderComponent } from './components/adminLTE/admin.header.component';
import { AdminMenuComponent } from './components/adminLTE/admin.menu.component';
import { AdminSettingComponent } from './components/adminLTE/admin.setting.component';
import { AdminLayoutComponent } from './components/layout/AdminLayout.component';
import { LayoutComponent } from './components/layout/layout.component';

//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { MatProgressBarModule } from '@angular/material/progress-bar';
//import { NgProgressModule } from 'ngx-progressbar';

@NgModule({
  declarations: [
    AppComponent,
    UploadFileComponent,
    LoginComponent,
    RegisterComponent,
    AdminFooterComponent,
    AdminHeaderComponent,
    AdminMenuComponent,
    AdminSettingComponent,
    AdminLayoutComponent,
    LayoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    DpDatePickerModule
    //BrowserAnimationsModule,
    //MatProgressBarModule,
    //NgProgressModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
