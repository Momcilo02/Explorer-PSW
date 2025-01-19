export interface Encounter {
    id: number;
    name: string;
    description: string;
    totalXp: number;
    creatorId: number;
    longitude: number;
    latitude: number;
    encounterType: number;
    status: number;
    touristRequestStatus?: number;
    isTourRequired?: boolean;
    tourId?: number;
    activateRange: number;
    peopleNumb?: number;
    image?: string;
    imageLatitude?: number;
    imageLongitude?: number;
    instructions?: string;
}