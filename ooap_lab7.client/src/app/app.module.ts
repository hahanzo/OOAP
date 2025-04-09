import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommentListComponent } from './moderation/components/comment-list/comment-list.component';
import { CommentItemComponent } from './moderation/components/comment-item/comment-item.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    CommentListComponent,
    CommentItemComponent,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
