export interface EncounterExecution {
    id?: number;
    encounterId: number;
    touristId: number;
    touristLongitude: number;
    touristLatitude: number;
    status: number;
    numberOfActiveTourists?:number;
}