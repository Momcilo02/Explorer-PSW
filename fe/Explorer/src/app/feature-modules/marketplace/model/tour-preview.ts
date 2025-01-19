import { Equipment } from "../../administration/model/equipment.model";
import { KeyPoint } from "./key-point.model";

export interface TourPreview {
    id: number;
    name: string;
    difficulty: string;
    description: string;
    cost: number;
    status:string;
    tags: string[];
    authorId : number;
    equipment: Equipment[];
    rating: number;
    length: number;
    startTime: string;
    firstKeyPoint: KeyPoint;
    keyPoints: KeyPoint[];
    averageRate: number;
    image: string;
}
