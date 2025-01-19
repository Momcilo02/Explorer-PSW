export interface Coupon{
    id: number,
    identifier: string,
    authorId: number,
    percentage: number,
    expirationDate: Date | null,
    toursEligible: number[],
    couponStatus: number
}