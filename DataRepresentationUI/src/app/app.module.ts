import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainLayoutComponent } from './services/main-layout/main-layout.component';
import { ChartModule } from '@syncfusion/ej2-angular-charts';
import { GraphicsComponentComponent } from './graphics-component/graphics-component.component';
@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    GraphicsComponentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ChartModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
