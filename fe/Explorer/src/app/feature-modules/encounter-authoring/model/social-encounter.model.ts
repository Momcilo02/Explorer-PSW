export interface SocialEncounters {
    id: number;
    name: string;
    description: string;
    totalXp: number;
    creatorId: number;
    longitude: number;
    latitude: number;
    encounterType: number;
    status: number;
    TouristRequestStatus: number;
    isTourRequired?: boolean;
    tourId?: number;
    activateRange: number;
    peopleNumb: number;
}