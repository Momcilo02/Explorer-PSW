export interface ClubMessage {
  id: number;
  senderId: number;
  touristClubId: number;
  senderName: string;
  senderSurname: string;
  sentDate: Date;
  content: string;
  likesCount: number;
  likedByLoggedUser: boolean;
}
