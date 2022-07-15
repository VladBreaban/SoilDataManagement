import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { MeasuredData } from './classes/MeasuredData';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DataServiceService {
  constructor(private http: HttpClient) { }

  public GetDataBetweenTimeInterval(start:string,end:string) : Observable<MeasuredData[]>{
    const url = environment.urlServices + 'GetDataBetweenTimeInterval?startDate='+start+'&endDate='+end;
    return this.http.get<MeasuredData[]>(url);
}


public GetAllDataFromCloud() : Observable<MeasuredData[]>{
  const url = environment.urlServices + 'GetAllDataFromCloud';
  return this.http.get<MeasuredData[]>(url);
}

public GetAllDataFromDatabase() : Observable<MeasuredData[]>{
  const url = environment.urlServices + 'GetAllDataFromDatabase';
  return this.http.get<MeasuredData[]>(url);
}

public GetDataBetweenTimeIntervalFromDatabase(start:string,end:string) : Observable<MeasuredData[]>{
  const url = environment.urlServices + 'GetDataBetweenTimeIntervalFromDatabase?startDate='+start+'&endDate='+end;
  return this.http.get<MeasuredData[]>(url);
}
}
