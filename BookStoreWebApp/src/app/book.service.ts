import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private basePath = 'https://localhost:44339/api/Books';

  constructor(private http: HttpClient) { }

  public getBooks(): Observable<any> {
    return this.http.get(this.basePath);
  }

  public addBook(book:any): Observable<any> {
    return this.http.post(this.basePath,book);
  }
}
