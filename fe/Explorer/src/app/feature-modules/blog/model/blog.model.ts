import { CalendarDate } from "calendar-date";
import { Rating } from "./rating.model";
import {Comment} from "../model/comment.model"

export interface Blogs {
    id: number,
    title: string,
    description: string,
    status: number,
    imageUrl: string[],
    date?: string,
    comments?: Comment[],
    ratings?: Rating[],
    ownerId: number,
    activityStatus: number
}

export interface BlogDto{
    blog: Blogs,
    isUpvoted: boolean,
    isDownvoted: boolean,
    isOwner: boolean,
    voteCount: number,
}