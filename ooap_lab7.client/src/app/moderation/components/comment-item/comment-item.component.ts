import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Comment } from '../../models/comment.model';
import { ActionType, ModerationActionDto } from '../../models/moderation.models';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.css']
})
export class CommentItemComponent {
  @Input() comment!: Comment;
  @Input() disabled: boolean | null = false;
  @Output() action = new EventEmitter<ModerationActionDto>();

  ActionType = ActionType;

  performAction(type: ActionType): void {
    if (!this.disabled) {
      const actionDto: ModerationActionDto = {
        commentId: this.comment.id,
        action: type
      };
      this.action.emit(actionDto);
    }
  }
}
