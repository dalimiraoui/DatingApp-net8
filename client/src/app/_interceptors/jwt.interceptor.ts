import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService= inject(AccountService);
  req = req.clone({
    setHeaders : {
      Authorization: `Bearer ${accountService.currentUser()?.token}`
    }
  });
  return next(req);
};
