export interface HiddenEncounters {
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
    image:string;
    imageLongitude:number;
    imageLatitude: number;
}