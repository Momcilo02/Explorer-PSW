import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { KeyPoint } from '../model/key-point.model';
import { ImagesService } from '../images.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-key-point-form',
  templateUrl: './key-point-form.component.html',
  styleUrls: ['./key-point-form.component.css']
})
export class KeyPointFormComponent implements OnChanges, OnInit{
  @Output() keyPointAdded = new EventEmitter<KeyPoint>();
  @Output() keyPointUpdated = new EventEmitter<KeyPoint>();
  @Input() keyPoint: KeyPoint;
  @Input() shouldEdit: boolean=false;
  @Input() longitude: number;
  @Input() latitude: number;

  keyPointForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    image: new FormControl('', []),
    latitude: new FormControl(0, [Validators.required, Validators.min(0.1)]),
    longitude: new FormControl(0, [Validators.required, Validators.min(0.1)]),
    isPublic: new FormControl(false, [])
  })

  constructor(private imagesService: ImagesService,private snackBar: MatSnackBar){}
  ngOnInit(): void {
    if(this.shouldEdit){
      this.keyPointForm.patchValue({
        isPublic: this.isChecked()
      });
      return;
    }
    this.keyPointForm.patchValue({
      latitude: this.latitude,
      longitude: this.longitude,
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(this.shouldEdit){
      this.keyPointForm.patchValue(this.keyPoint);
      this.keyPointForm.value.isPublic = this.isChecked();
      if(this.latitude !== 0 && this.longitude !== 0){
        this.keyPointForm.patchValue({
          latitude: this.latitude,
          longitude: this.longitude
        });
      }
    }
  }
  
  setCoordinates(lat: number, lng: number) {
    this.keyPointForm.patchValue({
      latitude: lat,
      longitude: lng
    });
  }
  submit(){
    if(!this.keyPointForm.valid){
      this.snackBar.open('Please fill in all fields!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
        
      });
      return;
    }
    var status;
    if(this.keyPointForm.value.isPublic === true){
      status = 1
    }
    else{
      status = 0
    }
    const keyPoint = {
      id: 0,
      name: this.keyPointForm.value.name || "",
      description: this.keyPointForm.value.description || "",
      image: this.keyPointForm.value.image || "",
      latitude: this.keyPointForm.value.latitude || 0,
      longitude: this.keyPointForm.value.longitude || 0,
      status: status,
      comment:""
    }
    if(this.shouldEdit)
      this.updateKeyPoint(keyPoint);
    else
      this.addKeyPoint(keyPoint)
    this.keyPointForm.reset()
  }   

  addKeyPoint(keyPoint: KeyPoint) {
    this.keyPointAdded.emit(keyPoint);
  }
  updateKeyPoint(keyPoint: KeyPoint){
    keyPoint.id = this.keyPoint.id;
    this.keyPointUpdated.emit(keyPoint);
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
      this.imagesService.addImage(formData).subscribe({
        next: (res) =>{
          this.keyPointForm.value.image = res.filePath;
        }
      });
    }
  }

  isChecked(): boolean{
    return this.keyPoint.status === 1;
  }
}
