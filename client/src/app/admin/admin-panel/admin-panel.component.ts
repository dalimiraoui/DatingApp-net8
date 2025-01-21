import { Component } from '@angular/core';
import { PhotoManagementComponent } from "../photo-management/photo-management.component";
import { UserManagementComponent } from "../user-management/user-management.component";
import { TabsModule } from 'ngx-bootstrap/tabs';
import { HasRoleDirective } from '../../_directives/has-role.directive';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [TabsModule, PhotoManagementComponent, UserManagementComponent, HasRoleDirective],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent {

}
