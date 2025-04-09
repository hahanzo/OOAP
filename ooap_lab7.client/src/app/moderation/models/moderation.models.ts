export enum ActionType {
  Approve = 0,
  Reject = 1,
  MarkAsSpam = 2
}

export interface ModerationActionDto {
  commentId: number;
  action: ActionType;
}
