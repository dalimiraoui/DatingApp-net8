import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AccountService } from '../_services/account.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';
import { HasRoleDirective } from '../_directives/has-role.directive';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, TitleCasePipe, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService)
  private router= inject(Router)
  private toaster= inject(ToastrService)
  
  model : any ={};

  login() {
    this.accountService.login(this.model).subscribe({
      next : _ => {this.router.navigateByUrl('/members')},
      error : error  => this.toaster.error(error.error)
    })
    
  }

  logout() {
    this.accountService.logout();
  }
}
