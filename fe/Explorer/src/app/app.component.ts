import { Component, OnInit } from '@angular/core';
import { AuthService } from './infrastructure/auth/auth.service';
import 'leaflet-routing-machine';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Explorer';

  constructor(
    private authService: AuthService,
  ) {}


  ngOnInit(): void {
    this.checkIfUserExists();
  }
  
  private checkIfUserExists(): void {
    this.authService.checkIfUserExists();
  }
}
