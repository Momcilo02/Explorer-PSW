import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ObjectService } from '../object.service';
import { Router } from '@angular/router';
import { Object } from '../model/object.model';
import { ImagesService } from '../images.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-object-form',
  templateUrl: './object-form.component.html',
  styleUrls: ['./object-form.component.css']
})
export class ObjectFormComponent implements OnChanges, OnInit {

  @Output() objectUpdated = new EventEmitter<null>();
  @Input() object: Object;
  @Input() shouldEdit: boolean=false;
  @Input() shouldAdd: boolean=true;
  @Input() alert: boolean=false;
  loggedInUser:User;
  @Input() longitude: number;
  @Input() latitude: number;

  constructor(private service: ObjectService, private router: Router, private imageService: ImagesService, private authService: AuthService,private snackBar: MatSnackBar) {}
  ngOnInit(): void {
    if(!this.shouldEdit){
      this.objectsForm.patchValue({
        latitude: this.latitude,
        longitude: this.longitude
      });
    }
  }

  setCoordinates(lat: number, lng: number) {
    this.objectsForm.patchValue({
      latitude: lat,
      longitude: lng
    });
  }

  ngOnChanges(changes: SimpleChanges) : void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.objectsForm.reset();
    if(this.shouldEdit){
      this.objectsForm.patchValue({
      name: this.object.name,
      description: this.object.description,
      image: this.object.image,
      category: this.object.category.toString(),
      longitude: this.object.longitude,
      latitude: this.object.latitude
    })
    if(this.latitude !== 0 && this.longitude !==0){
      this.objectsForm.patchValue({
        latitude: this.latitude,
        longitude: this.longitude
      });
    }
    }
    else if(this.shouldAdd)
      this.objectsForm.patchValue({
      name: "",
      description: "",
      image: "",
      category: "",
      longitude: this.longitude,
      latitude: this.latitude
    })
  }

  objectsForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    image: new FormControl('', []),
    category: new FormControl('', Validators.required),
    latitude: new FormControl(0, [Validators.required]),
    longitude: new FormControl(0, [Validators.required]),
    isPublic: new FormControl(false, [])
    });

  addObject() : void {
    if(this.objectsForm.invalid){
      this.snackBar.open('Please fill in all fields!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
        
      });
      return;
    }
    console.log(this.objectsForm.value);
    var status;
    if(this.objectsForm.value.isPublic){
      status = 1
    }else{
      status = 0
    }
    const object: Object = {
      id: 0,
      name: this.objectsForm.value.name || "",
      description: this.objectsForm.value.description || "",
      image: this.objectsForm.value.image || "",
      category: this.objectsForm.value.category ? parseInt(this.objectsForm.value.category) : 0, // Pretvara vrednost u broj
      latitude: this.objectsForm.value.latitude || 0,
      longitude: this.objectsForm.value.longitude || 0,
      status: status,
      comment:"",
      authorId: this.loggedInUser.id
      }

    this.service.addObject(object).subscribe({
      next:(_) =>{
        this.service.getObjects();
        this.objectUpdated.emit();
        this.snackBar.open('Successfully added object!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
        this.shouldAdd = false;
        this.objectsForm.patchValue({
          name: "",
          description: "",
          image: "",
          category: "",
          longitude: this.object.longitude,
          latitude: this.object.latitude
        })
      }
    });
    console.log("Izvrsio sam se?");
    this.objectsForm.reset();
  }

  updateObject() : void {
    if(!this.objectsForm.valid){
      this.snackBar.open('Please fill in all fields!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return;
    }
    var status;
    if(this.objectsForm.value.isPublic){
      status = 1
    }else{
      status = 0
    }
    const object: Object = {
      id: this.object.id,
      name: this.objectsForm.value.name || "",
      description: this.objectsForm.value.description || "",
      image: this.objectsForm.value.image || "",
      category: this.objectsForm.value.category ? parseInt(this.objectsForm.value.category) : 0, // Pretvara vrednost u broj
      longitude: this.objectsForm.value.longitude || 0,
      latitude: this.objectsForm.value.latitude || 0,
      status: status,
      comment: this.object.comment,
      authorId: this.object.authorId
      }
    console.log(object);

    this.service.updateObject(object).subscribe({
      next:(_) =>{
        this.objectUpdated.emit();
        window.location.reload();
        this.snackBar.open('Successfully updated object!', 'Close', {
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
      console.log('Nakon ubacene slike: ', this.objectsForm)
    }
  }
  uploadImage(file: File) {
    if (file) {
      const formData = new FormData();
      formData.append('file', file, file.name);
      this.imageService.addImage(formData).subscribe({
        next: (res) =>{
          console.log('res.filepath: ', res.filePath);
          this.objectsForm.patchValue({ image: res.filePath });
        }
      });
    }
  }

  reverseCategory(object: Object) : string{
    if(object.category === 0)
      return "WC";
    else if(object.category === 1)
      return "Restaurant";
    else if(object.category === 2)
      return "Parking";
    else
      return "Other";
  }
  alertOff() : void{
    this.alert=false;
  }
}
