import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryService, ChartModule, DataLabelService, DateTimeCategoryService, DateTimeService, LegendService, LineSeriesService, StripLineService, TooltipService } from '@syncfusion/ej2-angular-charts';

import { AdminLayoutRoutes } from './admin-layout.routing';

import { DashboardComponent }       from '../../pages/dashboard/dashboard.component';
import { IconsComponent }           from '../../pages/icons/icons.component';
import { AreaSeriesService } from '@syncfusion/ej2-angular-charts';
import {  ZoomService } from '@syncfusion/ej2-angular-charts';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MaterialModule } from 'app/material/material.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    NgbModule,
    ChartModule,
    MaterialModule
  ],
  declarations: [
    DashboardComponent,
    IconsComponent,
  ],
  providers: [ DateTimeService,AreaSeriesService, ZoomService, LineSeriesService, DateTimeCategoryService, StripLineService,CategoryService,LegendService, TooltipService, DataLabelService]

})

export class AdminLayoutModule {}
