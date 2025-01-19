import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'xp-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  images: string[] = [
    'assets/images/venice.jpg',
    'assets/images/rome.jpg',
    'assets/images/barcelona.jpg'
  ];

  currentImage: string;
  currentIndex: number = 0;

  ngOnInit(): void {
    this.currentImage = this.images[this.currentIndex];
    setInterval(() => this.nextImage(), 4000);
  }

  nextImage(): void {
    this.currentIndex = (this.currentIndex + 1) % this.images.length; // Ciklična promena
    this.currentImage = this.images[this.currentIndex];
  }

  scrollToTourist(): void {
    const element = document.getElementById('tourist-section');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

  scrollToGuide(): void {
    const element = document.getElementById('guide-section');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

}
