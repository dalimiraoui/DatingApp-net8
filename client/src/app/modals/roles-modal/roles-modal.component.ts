import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AdminService } from '../../_services/admin.service';

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent {
  
  private adminService = inject(AdminService)
  bsModalRef = inject(BsModalRef)
  username = ''
  title = '';
  availableRoles : string[] = []
  selectedRoles : string[] = []
  roleUpdated = false

  updateChecked(checkedValue : string) {
    if (this.selectedRoles.includes(checkedValue)) {
      this.selectedRoles = this.selectedRoles.filter( r => r != checkedValue)
    } else {
      this.selectedRoles.push(checkedValue)
    }
  }

  onSelectRoles() {
    this.roleUpdated = true,
    this.bsModalRef.hide()

  }

}
