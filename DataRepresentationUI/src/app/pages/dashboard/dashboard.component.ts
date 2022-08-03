import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartComponent, ILoadedEventArgs } from '@syncfusion/ej2-angular-charts';
import { DataServiceService } from 'app/data-service.service';
import Chart from 'chart.js';


@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html'
})

export class DashboardComponent implements OnInit {


  public average1:number=0;
  public average2:number=0;
  public average3:number=0;
  public canvas: any;
  public ctx;
  @ViewChild('chartK')
  public chartK: ChartComponent;
  @ViewChild('chartP')
  public chartP: ChartComponent;
  @ViewChild('chartN')
  public chartN: ChartComponent;
  public primaryYAxis: Object;
  public border: Object;
  public zoom: Object = {
    enableMouseWheelZooming: true,
    enablePinchZooming: true,
    enableSelectionZooming: true
  };
  public animation: Object = { enable: false };
  public legend: Object = { visible: false };
  //Initializing Primary X Axis
  public primaryXAxis: Object = {
    valueType: 'DateTime',
    intervalType: 'Days',
    rangePadding: 'None'
  };

  public chartData: Object[] = [];
  public chartDataP: Object[] = [];
  public chartDataK: Object[] = [];

  public isDataLoaded: boolean = false;

  public startDate: Date;
  public endDate: Date;

  constructor(protected data_service: DataServiceService, public datePipe: DatePipe) { }
  ngOnInit() {
    this.data_service.GetAllDataFromDatabase().subscribe(x => {
      for (let item of x) {
        this.chartData.push(new Object({

          x: item.measuredDate,
          y: item.n
        }))
        this.average1+=item.n;
        this.average2+=item.p;
        this.average3+=item.k;


        this.chartDataP.push(new Object({

          x: item.measuredDate,
          y: item.p
        }))

        this.chartDataK.push(new Object({

          x: item.measuredDate,
          y: item.k
        }))
      }

      console.log(this.chartData);
      this.average1=this.average1/this.chartData.length
      this.average2=this.average2/this.chartData.length
      this.average3=this.average3/this.chartData.length

      this.isDataLoaded = true;
    });

  }

  filterChartsData() {
    console.log(this.datePipe.transform(this.startDate, 'yyyy-MM-dd'));
    console.log(this.endDate.toString());
    this.chartData = [];
    this.chartDataP = [];
    this.chartDataK = [];
    this.data_service.GetDataBetweenTimeIntervalFromDatabase(this.datePipe.transform(this.startDate, 'yyyy-MM-dd'), this.datePipe.transform(this.endDate, 'yyyy-MM-dd')).subscribe(x => {
      for (let item of x) {
        this.chartData.push(new Object({

          x: item.measuredDate,
          y: item.n
        }))

        this.chartDataP.push(new Object({

          x: item.measuredDate,
          y: item.p
        }))

        this.chartDataK.push(new Object({

          x: item.measuredDate,
          y: item.k
        }))
      }
    });
    console.log(this.chartData);
    console.log(this.chartDataP);
    console.log(this.chartDataK);
    this.chartN.refresh();
    this.chartP.refresh();
    this.chartK.refresh();
  }
}
