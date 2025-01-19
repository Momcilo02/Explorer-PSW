import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Chart,registerables } from 'chart.js';
import  ChartDataLabels from 'chartjs-plugin-datalabels';
import { ProfileInfo } from '../model/profileInfo.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { LayoutService } from '../layout.service';
import { MarketplaceService } from '../../marketplace/marketplace.service';

Chart.register(...registerables, ChartDataLabels);
@Component({
  selector: 'xp-lucky-wheel',
  templateUrl: './lucky-wheel.component.html',
  styleUrls: ['./lucky-wheel.component.css']
})
export class LuckyWheelComponent implements OnInit {
  @ViewChild('wheel') wheelCanvas: ElementRef;
  @ViewChild('spinBtn') spinBtn: ElementRef;
  @ViewChild('finalValue') finalValue: ElementRef;

  loggedInUser: User;
  userInfo: ProfileInfo;

  wheel: any; 
  spinBtnElement: HTMLButtonElement;
  finalValueElement: HTMLElement;

  spintButtonText : string;

  constructor(private authService: AuthService, private layoutService: LayoutService, 
      private marketplaceService: MarketplaceService,){}

  //Object that stores values of minimum and maximum angle for a value
  rotationValues = [
    { minDegree: 0, maxDegree: 30, value: 2 },
    { minDegree: 31, maxDegree: 90, value: 1 },
    { minDegree: 91, maxDegree: 150, value: 6 },
    { minDegree: 151, maxDegree: 210, value: 5 },
    { minDegree: 211, maxDegree: 270, value: 4 },
    { minDegree: 271, maxDegree: 330, value: 3 },
    { minDegree: 331, maxDegree: 360, value: 2 },
  ];

  data = [16, 16, 16, 16, 16, 16];
  pieColors = ["#8b35bc", "#4568d9", "#FF5733", "#5ec435", "#e5ed00", "#eb0000"];
  myChart: any;
  count = 0;
  resultValue = 101;

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.getCurrentTourist();
    this.initWheel();
  }


  isWheelSpinnable(){
    if(new Date().getTime() - new Date(this.userInfo.lastWheelSpinTime).getTime() > 2 * 24 * 60 * 60 * 1000){
      this.spintButtonText = "Spin the wheel!";
      this.spinBtnElement.disabled = false;
    } else{
      const timeDifference = new Date().getTime() - new Date(this.userInfo.lastWheelSpinTime).getTime();
      const remainingTime = 2 * 24 * 60 * 60 * 1000 - timeDifference; // Preostalo vreme u milisekundama

       // Pretvaranje preostalog vremena u sate i minute
      const hours = Math.floor(remainingTime / (1000 * 60 * 60));
      const minutes = Math.floor((remainingTime % (1000 * 60 * 60)) / (1000 * 60));

      this.spintButtonText = `You can't spin the wheel for: ${hours}h ${minutes}min`;
      this.spinBtnElement.disabled = true;
    }

  }
  getCurrentTourist(){
    this.layoutService.fetchCurrentUser().subscribe((user) => {
       this.userInfo = user;
       console.log(this.userInfo);
       this.isWheelSpinnable();
    });
  }

  ngAfterViewInit() {
    this.spinBtnElement = this.spinBtn.nativeElement;
    this.finalValueElement = this.finalValue.nativeElement;
    this.initWheel();
  }

  initWheel() {
    this.myChart = new Chart(this.wheelCanvas.nativeElement, {
      plugins: [ChartDataLabels],
      type: 'pie',
      data: {
        labels: ["100 XP", "   Quiz token", "50 XP", "10 Coins", "15 XP", "MISS"],
        datasets: [{
          backgroundColor: this.pieColors,
          data: this.data,
        }],
      },
      options: {
        responsive: true,
        animation: { duration: 0 },
        plugins: {
          tooltip: { enabled: false },
          legend: { display: false },
          datalabels: {
            color: '#ffffff',
            formatter: (_, context) => context.chart.data.labels?.[context.dataIndex],
            font: { size: 22 },
          },
        },
      },
    });
  }

  valueGenerator(angleValue: number): void {
    for (let i of this.rotationValues) {
      if (angleValue >= i.minDegree && angleValue <= i.maxDegree) {
        switch(i.value){
          case (1): 
          this.finalValueElement.innerHTML = `<p>Congratulations you won: 100 XP !</p>`;
          this.updateUserXP(100);
          break;

          case (2): 
          this.finalValueElement.innerHTML = `<p>Congratulations you won: Quiz token!</p>`;
          break;

          case (3): 
          this.finalValueElement.innerHTML = `<p>Congratulations you won: 50 XP !</p>`;
          this.updateUserXP(50);
          break; 

          case (4): 
          this.finalValueElement.innerHTML = `<p>Congratulations you won: 10 Adventure coins</p>`;
          this.updateAdventurePoints(10);
          break;

          case (5): 
          this.finalValueElement.innerHTML = `<p>Congratulations you won: 15 XP !</p>`;
          this.updateUserXP(15);
          break;

          case (6): 
          this.finalValueElement.innerHTML = `<p>And that's a MISS, better luck next time!</p>`;
          break;
        }
        this.spinBtnElement.disabled = true;
        break;
      }
    }
  }

  updateAdventurePoints(points : number){
     this.marketplaceService.paymentAdventureCoins(this.loggedInUser.id, points).subscribe({
          next: () => {
            this.userInfo.lastWheelSpinTime = new Date();
            this.layoutService.updateCurrentUser(this.userInfo).subscribe({
              next:( data ) =>{
                  this.userInfo = data;
              }
            });

          },
          error: (err) => {
            console.error('Error during payment:', err);
          }
        });
  }

  updateUserXP(givenXP: number){
    this.userInfo.touristXp += givenXP;
    let currentLevel = this.userInfo.touristLevel;
    if(this.userInfo.touristXp >= 100 )
      currentLevel = 1;
  
    if(this.userInfo.touristXp >= 200 )
      currentLevel= 2;
  
    if(this.userInfo.touristXp >= 300 )
      currentLevel = 3;
  
    if(this.userInfo.touristXp >= 400 )
      currentLevel = 4;
  
    if(this.userInfo.touristXp >= 500 )
      currentLevel = 5;
  
    if(this.userInfo.touristXp >= 600 )
      currentLevel = 6;
  
    if(this.userInfo.touristXp >= 700 )
      currentLevel = 7;
  
    if(this.userInfo.touristXp >= 800 )
      currentLevel = 8;
  
    if(this.userInfo.touristXp >= 900 )
      currentLevel = 9;
  
    if(this.userInfo.touristXp >= 1000 )
      currentLevel = 10;
  
    if(currentLevel != this.userInfo.touristLevel){
      this.userInfo.touristLevel = currentLevel;
      setTimeout(() => {
        alert('Congratulations you leveled up! New level is ' + currentLevel);
      }, 1000);
    }
    this.userInfo.lastWheelSpinTime = new Date();
    this.layoutService.updateCurrentUser(this.userInfo).subscribe({
      next:( data ) =>{
          this.userInfo = data;
      }
    });

  }
  startSpinning() {
    this.spinBtnElement.disabled = true;
    this.finalValueElement.innerHTML = `<p>Good Luck!</p>`;
    
    let randomDegree = Math.floor(Math.random() * 355);
    let rotationInterval = setInterval(() => {
      this.myChart.options.rotation += this.resultValue;
      this.myChart.update();
      
      if (this.myChart.options.rotation >= 360) {
        this.count += 1;
        this.resultValue -= 5;
        this.myChart.options.rotation = 0;
      } else if (this.count > 15 && this.myChart.options.rotation === randomDegree) {
        this.valueGenerator(randomDegree);
        clearInterval(rotationInterval);
        this.count = 0;
        this.resultValue = 101;
      }
    }, 10);
  }

  onSpinButtonClick(): void {
    this.startSpinning();
  }
}