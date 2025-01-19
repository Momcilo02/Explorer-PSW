import { Encounter } from "../../encounter-authoring/model/encounter.model";

export interface ActiveEncounter {
    id?: number;
    encounter: Encounter | null | undefined;
    touristId: number;
    touristLongitude: number;
    touristLatitude: number;
    status: number;
    numberOfActiveTourists?:number;
}