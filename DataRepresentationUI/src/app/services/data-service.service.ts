import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'environments/environment';
import { MeasuredData } from 'app/classes/MeasuredData';

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
}
