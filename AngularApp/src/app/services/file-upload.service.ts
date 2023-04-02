import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  upload(email: string, file: File): Observable<HttpEvent<any>> {
    const formData: FormData = new FormData();

    formData.append('Email', email);
    formData.append('Document', file);

    const req = new HttpRequest('POST', `${this.baseUrl}`, formData);

    return this.http.request(req);
  }
}