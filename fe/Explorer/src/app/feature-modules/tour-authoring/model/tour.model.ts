import { Equipment } from "../../administration/model/equipment.model";
import { KeyPoint } from "./key-point.model"
import { TourDuration } from "./tour-duration.model";


export interface Tour{
    id?:number,
    name: string,
    difficulty: number,
    description: string,
    cost: number,
    status: number,
    tags:string,
    equipments: Equipment[],
    keyPoints: KeyPoint[],
    length?: number,
    tourDurations: TourDuration[],
    authorId: number,
    image: string,
    hasQuiz?: boolean;
}
