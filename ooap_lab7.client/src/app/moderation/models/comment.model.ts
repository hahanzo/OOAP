export enum CommentStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
  Spam = 3
}

export interface Comment {
  id: number;
  author: string;
  text: string;
  status: CommentStatus;
  createdAt: Date;
}
