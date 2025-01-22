import { inject, Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef?: BsModalRef
  private modalService = inject(BsModalService)

  confirm(
    title ='Confirmation',
    message ='Are you sure you want to do this ?',
    btnOkText ='Ok',
    btnCancelText ='Cancel'
  ) {
    const initialState : ModalOptions = {
          class :'modal-lg',
          initialState : {
            title : title,
            message : message,
            btnOkText : btnOkText,
            btnCancelText : btnCancelText
          }
        }
    
        this.bsModalRef = this.modalService.show(ConfirmDialogComponent, initialState)
        return this.bsModalRef.onHidden?.pipe(
          map(() => {
            if (this.bsModalRef?.content) {
              return this.bsModalRef.content.result
            } else return false
            
          })
        )

  }
}
