import { CalendarDate } from "calendar-date";

export interface Comment {
  id: number;
  userId: number;
  username: string;
  createdAt: string;
  text: string;
  lastModified: string; 
  }
  export interface CommentDto {
    Text: string;
    Username: string;
    CreatedAt: string;     
    LastModified: string;  
    userId : number
  }
  