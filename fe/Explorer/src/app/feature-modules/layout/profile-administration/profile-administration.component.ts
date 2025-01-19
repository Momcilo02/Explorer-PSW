import { Component, Input, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ProfileInfo } from '../model/profileInfo.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Router } from '@angular/router';
import { LayoutService } from '../layout.service';
import { TouristWallet } from 'src/app/feature-modules/marketplace/model/tourist-wallet.model';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TouristClub } from '../model/touristClub.model';

@Component({
  selector: 'xp-profile-administration',
  templateUrl: './profile-administration.component.html',
  styleUrls: ['./profile-administration.component.css']
})
export class ProfileAdministrationComponent implements OnInit {

  @ViewChild('editForm') editForm!: ElementRef;

  constructor(
    private authService: AuthService,
    private layoutService: LayoutService,
    private marketplaceService: MarketplaceService,
    private router: Router
  ) {}

  isEditing = true;
  isTourist = false;
  showForm = false;
  user!: User; // Logged-in user
  currentUser!: ProfileInfo;
  selectedUser!: ProfileInfo;
  allUsers: ProfileInfo[] = [];
  followersList: ProfileInfo[] = [];
  selectedFollower: ProfileInfo | null = null;
  paymentAmount: number = 0; // For handling AdventureCoins payments
  imageUrl: string = "";
  imageMap: Map<string, string> = new Map();
  profileImage: string = "";
  myTouristClubs: TouristClub[]=[];
  countMembers:number;


  showUsersModal: boolean = false;
  showFollowersModal: boolean = false;

  saveChanges() {
    this.edit();
    this.showForm = false;
  }

  ngOnInit(): void {
    this.checkUserRole();
    this.layoutService.fetchCurrentUser().subscribe((user) => {
      this.profileInfoForm.patchValue({
        id: user.id,
        userId: user.userId,
        name: user.name,
        surname: user.surname,
        email: user.email,
        profilePictureUrl: user.profilePictureUrl,
        biography: user.biography,
        motto: user.motto,
        touristLevel: user.touristLevel,
        touristXp: user.touristXp
      });
      this.profileImage = user.profilePictureUrl;
      this.currentUser = user;
      this.imageUrl = user.profilePictureUrl;
      this.getImage(this.imageUrl);
    });
    this.getUsers();
    this.getFollowers();
    this.countClubMembers();
    this.checkStatus();
    this.setTouristStatus();
  }

  checkUserRole() {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.isTourist = this.user?.role === 'tourist'; // Assuming 2 is the role for Tourist
    });
  }

  profileInfoForm = new FormGroup({
    id: new FormControl(-1),
    userId: new FormControl(-1),
    name: new FormControl(''),
    surname: new FormControl(''),
    email: new FormControl(''),
    profilePicture: new FormControl(null),
    profilePictureUrl: new FormControl(''),
    biography: new FormControl(''),
    motto: new FormControl(''),
    touristLevel: new FormControl(-1),
    touristXp: new FormControl(-1),
    lastWheelSpinTime : new FormControl(new Date())
  });

  private fileValidator(control: FormControl): { [key: string]: any } | null {
    const value: File | null = control.value;

    if (!value) {
      return { required: true, invalidFileType: false };
    }

    return null;
  }

  edit(): void {
    if (this.profileInfoForm.valid) {
      const profileInfo: ProfileInfo = {
        id: this.profileInfoForm.value.id || -1,
        userId: this.profileInfoForm.value.userId || -1,
        name: this.profileInfoForm.value.name || "",
        surname: this.profileInfoForm.value.surname || "",
        email: this.profileInfoForm.value.email || "",
        profilePictureUrl: this.imageUrl || "",
        biography: this.profileInfoForm.value.biography || "",
        motto: this.profileInfoForm.value.motto || "",
        touristLevel: this.profileInfoForm.value.touristLevel || -1,
        touristXp: this.profileInfoForm.value.touristXp || -1,
        lastWheelSpinTime: this.profileInfoForm.value.lastWheelSpinTime ||new Date(),
      };

      const formData = new FormData();
      Object.entries(profileInfo).forEach(([key, value]) => {
        formData.append(key, value?.toString() || '');
      });

      this.layoutService.saveNewInfo(profileInfo, formData).subscribe({
        next: () => {
          this.getUsers();
          this.getFollowers();
          this.setTouristStatus();
        },
        error: (err) => {
          console.error('Error saving profile info:', err);
        }
      });
    } else {
      console.error('Form is not valid');
    }
  }

  startEditing() {
    this.isEditing = true;
    this.showForm = true;
    setTimeout(() => {
      this.scrollToEditForm();
    }, 0);
  }

  scrollToEditForm() {
    if (this.editForm) {
      this.editForm.nativeElement.scrollIntoView({ behavior: 'smooth' });
    }
  }

  /// AdventureCoins functionality
  getAdventureCoins(userId: number): void {
    this.marketplaceService.getAdventureCoins(userId).subscribe({
      next: (wallet: TouristWallet) => {
        alert(`User has ${wallet.adventureCoins} Adventure Coins.`);
      },
      error: (err) => {
        console.error('Error fetching Adventure Coins:', err);
      }
    });
  }

  payAdventureCoins(userId: number, amount: number): void {
    if (amount <= 0) {
      alert('Please enter a valid payment amount.');
      return;
    }

    this.marketplaceService.paymentAdventureCoins(userId, amount).subscribe({
      next: (wallet: TouristWallet) => {
        alert(`Payment successful! Remaining Adventure Coins: ${wallet.adventureCoins}`);
      },
      error: (err) => {
        console.error('Error during payment:', err);
      }
    });
  }

  /// Followers functionality
  getUsers() {
    this.layoutService.getAllUsers().subscribe({
      next: (result) => { // Remove the logged-in user from the list and filter already followed users
        this.allUsers = result.results.filter(u => u.id !== this.currentUser.id);
        this.allUsers = this.allUsers.filter(u => !this.currentUser.following?.includes(u.id));
      }
    });
  }

  getFollowers() {
    this.layoutService.getAllUsers().subscribe({
      next: (result) => { // Get users who have the current user ID in their following list
        this.followersList = result.results.filter(u => u.following?.includes(this.currentUser.id));
      }
    });
  }

  followUser(idU: number) {
    if (this.currentUser.following) {
      this.currentUser.following.push(idU);
      const selUser = this.allUsers.find(u => u.id === idU);

      if (!selUser) {
        alert("Selected user does not exist.");
        return;
      }

      this.selectedUser = selUser;
      this.selectedUser.followers?.push(this.currentUser.id);

      this.layoutService.updateCurrentUser(this.currentUser).subscribe({
        next: (res) => {
        }
      });

      this.layoutService.updateSelectedUser(this.selectedUser).subscribe({
        next: (res) => {
        }
      });

      this.getUsers();
      this.getFollowers();
    }
  }

  // Section for followers
  sendMessage(follower: ProfileInfo): void {
    if (this.selectedFollower === follower) {
    
      this.selectedFollower = null;
    } else {
      
      this.selectedFollower = follower;
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

countClubMembers()
{
    this.countMembers=0;
    this.layoutService.getTouristClub().subscribe({
      next: (res) =>{
        this.myTouristClubs = res.results.filter(tc=>tc.ownerId === this.currentUser.userId);
        this.countMembers = this.myTouristClubs.reduce((total, club) => total + (club.members?.length || 0), 0);
      }});
}

checkStatus(): string
{
     if(this.countMembers>4)
      this.currentUser.touristStatus=0;
    else if(this.countMembers<=4 && this.countMembers>2)
      this.currentUser.touristStatus=1;
    else if(this.countMembers<=2&& this.countMembers>0)
      this.currentUser.touristStatus=2;
    else
      this.currentUser.touristStatus=3;
      switch(this.currentUser.touristStatus){
        case 0:
          return "GOLDEN";
        case 1:
          return "SILVER";
        case 2:
          return "BRONZE";
        case 3:
          return "BASIC";
  }
  return "";
}

setTouristStatus(){
  this.layoutService.changeTouristStatus(this.currentUser).subscribe({
    next: ()=>{}
  });
}




toggleUsersModal(): void {
  this.showUsersModal = !this.showUsersModal;
}

toggleFollowersModal(): void {
  this.showFollowersModal = !this.showFollowersModal;
}

checkRank(): string
{
  if(this.currentUser.touristRank == 0)
    return "EXPLORER";
  else if(this.currentUser.touristRank == 1)
    return "SURVIVOR";
  else if(this.currentUser.touristRank == 2)
    return "TRAVELLER";
  else if(this.currentUser.touristRank == 3)
    return "CAPTAIN";
  else
    return "ULTIMATE";
}
}
