import { Message } from "./message.model";

export interface ReportedTourProblems {
    id: number;
    tourId: string;
    category: string;
    priority: number;
    description: string;
    time: Date;
    touristId: number;
    messages: Message[];
    status: number;
    solvingDeadline: Date | null;
}


export interface ReportedTourProblem{
  solvingDeadline?: Date,
  status?: number,
  id?: number
}
