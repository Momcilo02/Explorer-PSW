import { Time } from "@angular/common";

export interface ProfileInfo {
  id: number;
  userId: number;
  name: string;
  surname: string;
  profilePictureUrl: string;
  biography: string;
  motto: string;
  email: string;
  touristLevel:number;
  touristXp:number;
  lastWheelSpinTime: Date;
  followers?: number[];
  following?: number[];
  clubMember?: number[];
  touristStatus?: number;
  touristRank?: number;
}
