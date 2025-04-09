import { Component, OnInit } from '@angular/core';
import { ModerationService } from '../../services/moderation.service';
import { Observable } from 'rxjs';
import { Comment } from '../../models/comment.model';
import { ModerationActionDto } from '../../models/moderation.models';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit {
  comments$: Observable<Comment[]>;
  isLoading$: Observable<boolean>;
  error$: Observable<string | null>;

  constructor(private moderationService: ModerationService) {
    this.comments$ = this.moderationService.comments$;
    this.isLoading$ = this.moderationService.isLoading$;
    this.error$ = this.moderationService.error$;
  }

  ngOnInit(): void {
    this.loadComments();
  }

  loadComments(): void {
    this.moderationService.loadPendingComments();
  }

  handleAction(actionDto: ModerationActionDto): void {
    this.moderationService.performAction(actionDto).subscribe({
      next: () => { }, // Service handles success side-effects
      error: () => { } // Service handles error side-effects
    });
  }

  handleUndo(): void {
    this.moderationService.undoLastAction().subscribe({
      next: () => { }, // Service handles success side-effects
      error: () => { } // Service handles error side-effects
    });
  }
}
