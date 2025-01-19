import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'xp-rank-up',
  templateUrl: './rank-up.component.html',
  styleUrls: ['./rank-up.component.css']
})
export class RankUpComponent {
  @Input() rank: number | undefined;
  @Output() close = new EventEmitter<void>();

  get rankImage(): string {
    if (this.rank == 0)
      return `/assets/images/explorer.webp`; 
    else if (this.rank == 1)
      return `/assets/images/survivor.jpg`; 
    else if (this.rank == 2)
      return `/assets/images/traveller.jpg`; 
    else if (this.rank == 3)
      return `/assets/images/captain.jpeg`; 
    else
      return `/assets/images/ultimate.jpg`; 
  }

  rankToString(): string {
    if (this.rank == 0)
      return "EXPLORER"; 
    else if (this.rank == 1)
      return "SURVIVOR"; 
    else if (this.rank == 2)
      return "TRAVELLER"; 
    else if (this.rank == 3)
      return "CAPTAIN"; 
    else
      return "ULTIMATE"; 
  }

  closeModal(): void {
    this.close.emit();
  }
}
