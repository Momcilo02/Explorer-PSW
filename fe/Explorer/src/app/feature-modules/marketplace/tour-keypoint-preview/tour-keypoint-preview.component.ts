import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { KeyPoint } from '../model/key-point.model';
import { ImagesService } from '../../tour-authoring/images.service';

@Component({
  selector: 'xp-tour-keypoint-preview',
  templateUrl: './tour-keypoint-preview.component.html',
  styleUrls: ['./tour-keypoint-preview.component.css']
})
export class TourKeypointPreviewComponent implements OnChanges, OnDestroy {
  @Input() keyPoint!: KeyPoint;
  image: string;
  constructor(private imageService: ImagesService){

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['keyPoint'] && this.keyPoint) {
      this.loadImage();
    }
  }
  loadImage(): void {
    if (this.keyPoint && this.keyPoint.image) {
      this.imageService.getImage(this.keyPoint.image).subscribe(blob => {
        const image = URL.createObjectURL(blob);
        this.image = image;
      });
    }
  }
  ngOnDestroy(): void {
    if (this.image) {
      URL.revokeObjectURL(this.image);
    }
  }
}
