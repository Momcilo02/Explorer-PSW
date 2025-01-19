export interface ReportingTourProblem {
    id?: number;
    tourId?: number;
    category: string;
    priority: number;
    description: string;
    time: Date;
    touristId: number;
}