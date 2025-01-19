import { Component, OnInit } from '@angular/core';
import { LayoutService } from '../layout.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TouristClub } from '../model/touristClub.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ProfileInfo } from '../model/profileInfo.model';
import { ImagesService } from '../../tour-authoring/images.service';

@Component({
  selector: 'xp-tourist-club',
  templateUrl: './tourist-club.component.html',
  styleUrls: ['./tourist-club.component.css']
})
export class TouristClubComponent implements OnInit {

  myTouristClubs: TouristClub[] = [];
  memberTouristClubs: TouristClub[] = [];
  availableTouristClubs: TouristClub[] =[];
  allTouristClubs: TouristClub[] = [];

  selectedTouristClub: TouristClub;
  shouldRenderTouristClubForm: boolean = false;
  shouldEdit: boolean = false;
  showChatToggle: boolean = false;
  loggedInUser: User;
  currentUser!: ProfileInfo;
  allUsers: ProfileInfo[];
  averageRate: number;
  selectedRate:number=1;
  imageMap: Map<string, string> = new Map();

  constructor(private service: LayoutService, private authService: AuthService, private imageService: ImagesService){ }

  ngOnInit(): void {

      this.getLoggedInUser();
      this.getCurrentUser();
      this.getMyTouristClubs();
      this.getMemberTouristClubs();
      this.getAvailableTouristClubs();
      this.getAllTouristClubs();
  }

  getImage(path: string): void {
    this.imageService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }

  getLoggedInUser()
  {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
  }

  getCurrentUser(): void {
    this.service.fetchCurrentUser().subscribe({
      next: (user) => {
        if (user) {
          this.currentUser = user;
        }
      },
      error: (err) => {
        console.error('Error fetching current user:', err);
      },
    });
  }

  getMyTouristClubs() : void{

    if(!this.loggedInUser) return;

    this.service.getTouristClub().subscribe({
      next: (result : PagedResults<TouristClub>) =>{
        console.log(result);
        this.myTouristClubs = result.results.filter(tC=>tC.ownerId === this.loggedInUser.id);
      },
      error: (err: any) =>{
        console.log("Error fetching clubs",err);
      }
    });
  }

  getMemberTouristClubs(): void{
    this.service.getTouristClub().subscribe({
      next: (result : PagedResults<TouristClub>) =>{
        console.log(result);
        this.memberTouristClubs = result.results.filter(club => club.ownerId !== this.loggedInUser.id
          &&club.members?.some(memberId => memberId === this.loggedInUser.id)
        );
      },
      error: (err: any) =>{
        console.log(err);
      }
    });
  }

  getAvailableTouristClubs(): void{
    this.service.getTouristClub().subscribe({
      next: (result : PagedResults<TouristClub>) =>{
        console.log(result);
        this.availableTouristClubs = result.results.filter(tC=>tC.ownerId !== this.loggedInUser.id
          &&!tC.members?.includes(this.loggedInUser.id)
        );
      },
      error: (err: any) =>{
        console.log(err);
      }
    });
  }

  getAllTouristClubs(): void{
    this.service.getTouristClub().subscribe({
      next: (result : PagedResults<TouristClub>) =>{
        console.log(result);
        this.allTouristClubs = result.results;
        this.allTouristClubs = result.results.map(tC => {
          return {
            ...tC,
            alreadyMember: tC.members?.includes(this.loggedInUser.id) || tC.ownerId === this.loggedInUser.id
          };
        });
        for (let to of this.allTouristClubs) {
          this.getImage(to.picture);
        }
      },
      error: (err: any) =>{
        console.log(err);
      }
    });
  }

  onEditClicked(club: TouristClub): void {
    this.selectedTouristClub = club;
    this.shouldRenderTouristClubForm = true;
    this.shouldEdit = true;
  }

  onAddClicked(): void {
    this.shouldEdit = false;
    if(this.shouldRenderTouristClubForm)
    {
      this.shouldRenderTouristClubForm =false;
    }
    else
    {
      this.shouldRenderTouristClubForm =true;
    }

  }

  showChat(touristClubId: number | undefined, club: TouristClub): void{
    this.selectedTouristClub = club;
    this.showChatToggle = true;
    console.log(touristClubId);
  }

  refreshMessages() {
    console.log("Poruke su osvežene!");
    // Dodajte vašu logiku za osvežavanje poruka ovde
  }

  becomeMember(selectedClub:TouristClub)
  {
    this.selectedTouristClub = selectedClub;
    this.selectedTouristClub.members?.push(this.loggedInUser.id);
    this.service.addMemberInClub(this.selectedTouristClub).subscribe({
      next: () => {
        alert("You are become a member of "+ selectedClub.name+" tourist Club");
      },error(err) {
        alert("Cannot add user id in members list");
      },
    });

    if(this.selectedTouristClub.id !=undefined)
      this.currentUser.clubMember?.push(this.selectedTouristClub.id);

    this.service.addClubInMember(this.currentUser).subscribe({
      next: ()=>{
        console.log("Uspesno dodatt")
      },error(err) {
        alert("Cannot add tourist club id in ClubMember list");
      },
    });

        this.getAvailableTouristClubs();
        this.getMemberTouristClubs();
        this.getMyTouristClubs();
  }

onRateChange(tc: TouristClub){
  this.selectedRate = Number(this.selectedRate);
  tc.ratedMembers?.push(2);
  tc.rates?.push(this.selectedRate);
  if(tc.averageRate!==undefined)
  tc.averageRate = tc.rates?.length
  ? tc.rates.reduce((sum, current) => sum + current, 0) / tc.rates.length
  : 0;


  this.service.rateClub(tc).subscribe({
    next:()=>{
      alert("Uspesno ste ocenili klub");
      this.getMemberTouristClubs();
    }
  });

}

showRateTools(cl :TouristClub){
  return !cl.ratedMembers?.includes(this.loggedInUser.id);
}

}
