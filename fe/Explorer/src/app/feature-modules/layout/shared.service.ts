import { Injectable, EventEmitter } from '@angular/core';
import { ClubMessage } from './model/clubMessage.model';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  eventEmitted = new EventEmitter<void>();
  messageEditEvent = new EventEmitter<ClubMessage>();

  emitEvent() {
    this.eventEmitted.emit();
  }

  emitEditMessageEvent(message: ClubMessage) {
    this.messageEditEvent.emit(message);
  }
}
