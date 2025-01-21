import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { authGuard } from './_guard/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guard/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './_guard/admin.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate:[authGuard],
    children: [
      { path: 'members', component: MemberListComponent, canActivate: [authGuard] },
      { path: 'members/edit', component: MemberEditComponent, canDeactivate:[preventUnsavedChangesGuard] },
      { path: 'members/:username', component: MemberDetailsComponent, resolve: {member:memberDetailedResolver} },
      { path: 'messages', component: MessagesComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard] },
    ],
  },
  {path :'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];
