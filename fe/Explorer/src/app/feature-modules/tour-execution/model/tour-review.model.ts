export interface TourReview {
    id?: number,
    tourId: number,
    rating: number,
    comment: string,
    touristId: number,
    visitDate: Date | null;   
    reviewDate: Date | null;
    completedPercentage: number;
    images: string[];
}