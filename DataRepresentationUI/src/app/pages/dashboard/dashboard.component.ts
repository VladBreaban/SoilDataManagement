import { Component, OnInit } from '@angular/core';
import { DataServiceService } from 'app/data-service.service';
import Chart from 'chart.js';


@Component({
    selector: 'dashboard-cmp',
    moduleId: module.id,
    templateUrl: 'dashboard.component.html'
})

export class DashboardComponent implements OnInit{

  public canvas : any;
  public ctx;
  public chartColor;
  public chartEmail;
  public chartHours;
//Initializing Primary X Axis
public primaryXAxis: Object = {
  valueType: 'DateTime',
        intervalType: 'Seconds',
            rangePadding: 'None'
};
  public chartData: Object[]=[];
  public isDataLoaded: boolean = false;

    constructor(protected data_service:DataServiceService){}
    ngOnInit(){
      this.data_service.GetAllDataFromCloud().subscribe(x=>{
        for(let item of x)
        {
          this.chartData.push(new Object({

              x: item.createdDate,
              y:item.n
          }))
        }
        console.log(x);
        this.isDataLoaded=true;
      });
   
    }
  
    
}
