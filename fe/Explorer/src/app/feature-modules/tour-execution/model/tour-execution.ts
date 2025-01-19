export interface TourExecution{
    id: number,
    tourId: number,
    touristId: number,
    tourStartDate:Date,
    tourEndDate:Date,
    lastActivity:Date,
    status: number,
    completedPercentage:number,
}

