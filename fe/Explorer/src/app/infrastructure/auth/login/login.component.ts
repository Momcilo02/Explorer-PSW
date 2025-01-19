import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { Login } from '../model/login.model';
import { jwtDecode } from 'jwt-decode';
import { MarketplaceService } from '../../../feature-modules/marketplace/marketplace.service';
import { ShoppingCart } from '../../../feature-modules/marketplace/model/shopping-cart.model';

@Component({
  selector: 'xp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(
    private authService: AuthService,
    private router: Router,
    private marketService: MarketplaceService
  ) {}

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  login(): void {
    const login: Login = {
      username: this.loginForm.value.username || "",
      password: this.loginForm.value.password || "",
    };

    if (this.loginForm.valid) {
      this.authService.login(login).subscribe({
        next: (res) => {
          this.router.navigate(['/']);
          const decoded: any = jwtDecode(res.accessToken);

            this.marketService.getShoppingCart(decoded.id).subscribe({
                next: (result: ShoppingCart) => {
                  localStorage.setItem(decoded.username, result.items.length.toString());
                },
                error: (error) => {
                  console.log('Error occurred while fetching shopping cart: ' + error);
                }
              });
        },
      });
    }
  }
}
