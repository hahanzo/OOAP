import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap, finalize } from 'rxjs/operators';
import { Comment } from '../models/comment.model';
import { ModerationActionDto } from '../models/moderation.models';

@Injectable({
  providedIn: 'root'
})
export class ModerationService {
  private apiUrl: string;

  private commentsSubject = new BehaviorSubject<Comment[]>([]);
  public comments$ = this.commentsSubject.asObservable();

  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  public isLoading$ = this.isLoadingSubject.asObservable();

  private errorSubject = new BehaviorSubject<string | null>(null);
  public error$ = this.errorSubject.asObservable();


  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiUrl = `${baseUrl}api/moderation`;
  }

  loadPendingComments(): void {
    this.isLoadingSubject.next(true);
    this.errorSubject.next(null);
    this.http.get<Comment[]>(`${this.apiUrl}/pending`)
      .pipe(
        catchError(this.handleError.bind(this)),
        finalize(() => this.isLoadingSubject.next(false))
      )
      .subscribe(comments => this.commentsSubject.next(comments));
  }

  performAction(actionDto: ModerationActionDto): Observable<Comment> {
    this.isLoadingSubject.next(true);
    this.errorSubject.next(null);
    return this.http.post<Comment>(`${this.apiUrl}/actions`, actionDto)
      .pipe(
        tap(updatedComment => {
          this.removeCommentFromList(updatedComment.id);
          console.log('Action successful:', updatedComment);
        }),
        catchError(this.handleError.bind(this)),
        finalize(() => this.isLoadingSubject.next(false))
      );
  }

  undoLastAction(): Observable<string> {
    this.isLoadingSubject.next(true);
    this.errorSubject.next(null);
    return this.http.post<string>(`${this.apiUrl}/undo`, {})
      .pipe(
        tap((message) => {
          console.log('Undo successful:', message);
          this.loadPendingComments();
        }),
        catchError(this.handleError.bind(this)),
        finalize(() => this.isLoadingSubject.next(false))
      );
  }

  private removeCommentFromList(commentId: number): void {
    const currentComments = this.commentsSubject.getValue();
    const updatedComments = currentComments.filter(c => c.id !== commentId);
    this.commentsSubject.next(updatedComments);
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error(`Backend returned code ${error.status}, body was: ${JSON.stringify(error.error)}`);
    const userMessage = typeof error.error === 'string' && error.status !== 500
      ? error.error // Use specific backend message if available and not generic 500
      : 'An error occurred. Please try again later.';
    this.errorSubject.next(userMessage);
    return throwError(() => new Error(userMessage));
  }
}
