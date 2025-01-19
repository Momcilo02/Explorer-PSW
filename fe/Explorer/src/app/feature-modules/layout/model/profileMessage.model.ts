export interface ProfileMessage {
  id: number;
  senderId: number;
  receiverId: number;
  senderName: string;
  senderSurname: string;
  sentDate: Date;
  content: string;
  resourcesId?: number;
  resourcesType?: string;
  resourcesName?: string;
  type?: number;
  
}
