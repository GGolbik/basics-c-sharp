import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BooksComponent } from './books.component';
import { BookListComponent } from '../components/books/bookList.component';
import { NewBookComponent } from '../components/books/add/new-book.component';
import { ShowBookComponent } from '../components/books/details/show-book.component';
import { UpdateBookComponent } from '../components/books/update/update-book.component';
import { DeleteBookComponent } from '../components/books/delete/delete-book.component';

const routes: Routes = [{ path: '', component: BooksComponent, children: [
  { path: '', component: BookListComponent },
  { path: 'add', component: NewBookComponent },
  { path: 'details/:id', component: ShowBookComponent },
  { path: 'update/:id', component: UpdateBookComponent },
  { path: 'delete/:id', component: DeleteBookComponent },
] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
