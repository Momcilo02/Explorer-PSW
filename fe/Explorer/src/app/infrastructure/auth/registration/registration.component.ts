import { Component, EventEmitter, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Registration } from '../model/registration.model';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { User } from '../model/user.model';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'xp-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  imageUrl: string = "";
  imageMap: Map<string, string> = new Map();

  loggedInUser:User;
  constructor(
    private authService: AuthService,
    private shoppingService:MarketplaceService,
    private router: Router,
  ) {}

  registrationForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    motto: new FormControl('', [Validators.required]),
    biography: new FormControl('', [Validators.required]),
    profilePictureUrl: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  register(): void {
    const registration: Registration = {
      name: this.registrationForm.value.name || "",
      surname: this.registrationForm.value.surname || "",
      email: this.registrationForm.value.email || "",
      username: this.registrationForm.value.username || "",
      password: this.registrationForm.value.password || "",
      role: "Tourist",
      motto: this.registrationForm.value.motto || "",
      biography: this.registrationForm.value.biography || "",
      touristLevel: 0,
      touristXp: 0,
      profilePictureUrl: this.registrationForm.value.profilePictureUrl || ""
    };

    console.log("Dosao sam dovde: ", registration);

    if (this.registrationForm.valid) {
      console.log("Sad sam u if-u: ", registration);

      this.authService.register(registration).subscribe({
        next: (result) => {
          const createWallet$ = this.shoppingService.createWallet(result.id);
          const createCart$ = this.shoppingService.createCart(result.id);

          // Izvršavanje oba zahteva paralelno i nastavak nakon završetka
          forkJoin([createWallet$, createCart$]).subscribe({
            next: () => {
              console.log("Wallet and cart created successfully.");
              this.router.navigate(['home']);
            },
            error: (err) => {
              console.error("Error creating wallet or cart:", err);
            }
          });
        },
        error: (err) => {
          console.error("Registration error:", err);
        },
      });
    }
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
      console.log(formData)
      this.authService.addImage(formData).subscribe({
        next: (res) =>{
          this.imageUrl = res.filePath;
          this.getImage(res.filePath);
          this.registrationForm.patchValue({
            profilePictureUrl: res.filePath
          })
        }
      });
    }
  }
  getImage(path: string): void {
    this.authService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }
}
