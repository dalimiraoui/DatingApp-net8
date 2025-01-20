import { ResolveFn } from '@angular/router';
import { Member } from '../_models/member';
import { MemberService } from '../_services/member.service';
import { inject } from '@angular/core';

export const memberDetailedResolver: ResolveFn<Member |null> = (route, state) => {
  const memberService = inject(MemberService);

  const username = route.paramMap.get('username');

  if(!username) return null

  return memberService.getMemberByUserName(username);
};
