import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/services/book';
import { BookService } from 'src/app/services/book.service';

@Component({
  selector: 'app-books',
  templateUrl: './bookList.component.html',
  styleUrls: ['./bookList.component.css']
})
export class BookListComponent implements OnInit {

  public books!: Book[];

  constructor(private service: BookService, private router: Router) { }

  ngOnInit() {
    this.service.getAllBooks().subscribe(data => { this.books = data; });
  }

  
  showBook(id: number){
    this.router.navigate(["/books/details/"+id]);
  }

  updateBook(id: number){
    this.router.navigate(["/books/update/"+id]);
  }

  deleteBook(id: number){
    this.router.navigate(["books/delete/"+id]);
  }

}
