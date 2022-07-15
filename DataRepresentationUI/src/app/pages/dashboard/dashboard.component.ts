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

  public primaryYAxis: Object;
  public border: Object;
  public zoom: Object={
    enableMouseWheelZooming: true,
    enablePinchZooming: true,
    enableSelectionZooming: true
};
  public animation: Object= { enable: false};
  public legend: Object= { visible: false };
//Initializing Primary X Axis
public primaryXAxis: Object = {
  valueType: 'DateTime',
        intervalType: 'Days',
            rangePadding: 'None'
};

  public chartData: Object[]=[];
  public chartDataP: Object[]=[];
  public chartDataK: Object[]=[];

  public isDataLoaded: boolean = false;

    constructor(protected data_service:DataServiceService){}
    ngOnInit(){
      this.data_service.GetAllDataFromDatabase().subscribe(x=>{
        for(let item of x)
        {
          this.chartData.push(new Object({

              x: item.measuredDate,
              y:item.n
          }))

          this.chartDataP.push(new Object({

            x: item.measuredDate,
            y:item.p
        }))

        this.chartDataK.push(new Object({

          x: item.measuredDate,
          y:item.k
      }))
        }

        console.log(this.chartData);
        this.isDataLoaded=true;
      });
   
    }
  
    
}
