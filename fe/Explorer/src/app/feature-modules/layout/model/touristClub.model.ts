export interface TouristClub{
    id?: number;
    name: string;
    description:string;
    picture: string;
    ownerId: number;
    members?: number[];
    alreadyMember?: boolean;
    rates?: number[];
    averageRate?:number;
    ratedMembers?:number[];
}
