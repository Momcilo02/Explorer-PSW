import { Component, EventEmitter, inject, Input, OnChanges, Output, signal, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TourService } from '../tour.service';
import { MarketplaceService } from '../../marketplace/marketplace.service';
import { Tour } from '../model/tour.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ImagesService } from '../images.service';
import { MatChipInputEvent } from '@angular/material/chips';
import { Router } from '@angular/router';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-tour-form',
  templateUrl: './tour-form.component.html',
  styleUrls: ['./tour-form.component.css']
})
export class TourFormComponent implements OnChanges {

  @Output() tourUpdated = new EventEmitter<null>();
  @Input() tour: Tour;
  @Input() shouldEdit: boolean = false;
  loggedInUser: User;
  tagList: string[] = [];
  readonly templateKeywords = signal(this.tagList);

  constructor(
    private service: TourService,
    private shoppingService: MarketplaceService,
    private authService: AuthService,
    private imageService: ImagesService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.tourForm.reset();

    if (this.shouldEdit) {
      this.tagList = this.tour.tags.split("|");
      for (let tag of this.tagList) {
        this.templateKeywords.update(keywords => [...keywords, tag]);
      }

      this.tourForm.patchValue({
        name: this.tour.name,
        difficulty: this.tour.difficulty,
        description: this.tour.description,
        cost: this.tour.cost,
        status: this.tour.status,
        tags: this.tour.tags,
        image: this.tour.image
      });
    } else {
      this.tagList = ["Hiking", "Walk", "Summer", "Spring", "See", "Mountain", "City", "Village"];
      for (let tag of this.tagList) {
        this.templateKeywords.update(keywords => [...keywords, tag]);
      }
      this.tourForm.patchValue({
        cost: 0,
        status: 0
      });
    }
  }

  tourForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    difficulty: new FormControl(0, [Validators.required]),
    description: new FormControl('', [Validators.required]),
    cost: new FormControl(0, [Validators.required, Validators.min(0)]),
    status: new FormControl(0, [Validators.required]),
    tags: new FormControl('', []),
    image: new FormControl('', [])
  });

  addTour(): void {
    this.tagToString();
    if(!this.tourForm.valid){
        this.snackBar.open('Please fill in all fields!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
        return;
     }

    const tour: Tour = {
      name: this.tourForm.value.name || "",
      difficulty: Number(this.tourForm.value.difficulty) || 0,
      description: this.tourForm.value.description || "",
      cost: this.tourForm.value.cost || 0,
      status: this.tourForm.value.status || 0,
      tags: this.tourForm.value.tags || "",
      length: 0,
      authorId: this.loggedInUser.id,
      equipments: [],
      keyPoints: [],
      tourDurations: [],
      image: this.tourForm.value.image || ""
    };

    this.service.addTour(tour).subscribe({
      next: (response: Tour) => {
        this.tourUpdated.emit();
        this.snackBar.open('Successfully added tour!', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: 'success',
          });
        // Dodata potvrda za kreiranje kviza
        const createQuiz = confirm("Do you want to create a quiz for this tour?");
        if (createQuiz) {
          this.router.navigate([`/create-quiz/${response.id}`]);
        }
      },
      error: (err) => {
        console.error("Error creating tour:", err);
      }
    });
  }

  updateTour(): void {
    this.tagToString();
    if(!this.tourForm.valid){
        this.snackBar.open('Please fill in all fields!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
        return;
      }

    const tour: Tour = {
      name: this.tourForm.value.name || "",
      difficulty: Number(this.tourForm.value.difficulty) || 0,
      description: this.tourForm.value.description || "",
      cost: this.tourForm.value.cost || 0,
      status: this.tourForm.value.status || 0,
      tags: this.tourForm.value.tags || "",
      authorId: this.tour.authorId,
      equipments: this.tour.equipments,
      keyPoints: this.tour.keyPoints,
      tourDurations: this.tour.tourDurations,
      image: this.tourForm.value.image || "",
      length: this.tour.length
    };
    tour.id = this.tour.id;

    this.service.updatedTour(tour).subscribe({
      next: (_) => {
        this.shoppingService.createItem(tour).subscribe({
          next:(result)=>{},
          error: (err) => console.error("Error creating marketplace item:", err)
        });
        this.tourUpdated.emit();
        this.snackBar.open('Successfully updated tour!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
    });
   }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files) {
      this.uploadImage(target.files[0]);
    }
  }

  uploadImage(file: File) {
    if (file) {
      const formData = new FormData();
      formData.append('file', file, file.name);
      this.imageService.addImage(formData).subscribe({
        next: (res) => {
          this.tourForm.patchValue({ image: res.filePath });
        }
      });
    }
  }

  removeTag(keyword: string) {
    this.templateKeywords.update(keywords => {
      const index = keywords.indexOf(keyword);
      if (index >= 0) keywords.splice(index, 1);
      this.tagList = [...keywords];
      return keywords;
    });
  }

  addTag(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.templateKeywords.update(keywords => [...keywords, value]);
      this.tagList.push(value);
    }
    event.chipInput!.clear();
  }

  tagToString() {
    let str = this.tagList.join('|');
    this.tourForm.value.tags = str;
  }
}
