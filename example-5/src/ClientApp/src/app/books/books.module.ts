import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { BooksComponent } from './books.component';
import { BookListComponent } from '../components/books/bookList.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NewBookComponent } from '../components/books/add/new-book.component';
import { ShowBookComponent } from '../components/books/details/show-book.component';
import { UpdateBookComponent } from '../components/books/update/update-book.component';
import { DeleteBookComponent } from '../components/books/delete/delete-book.component';

@NgModule({
  declarations: [
    BooksComponent,
    BookListComponent,
    NewBookComponent,
    ShowBookComponent,
    UpdateBookComponent,
    DeleteBookComponent
  ],
  imports: [
    CommonModule,
    BooksRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class BooksModule { }
