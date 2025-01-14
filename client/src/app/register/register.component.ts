import { Component, inject, Input, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent, DatePickerComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{

  accountService = inject(AccountService);
  private fb = inject(FormBuilder)
  private router = inject(Router)
  @Input() usersFromHomeComponent : any
  //usersFromHomeComponent = input.required<any>(); // this is a new way to input data starting from angular v.17
  //@Output() cancelRegister = new EventEmitter();
  cancelRegister = output<boolean>();
  model:any ={}

  registerForm : FormGroup = new FormGroup({})
  maxDate = new Date()
  validationsError : string[] =[];

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18)
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender : ['male'],
      username : ['', [Validators.required]],
      knownAs : ['', [Validators.required]],
      dateOfBirth : ['', [Validators.required]],
      city : ['', [Validators.required]],
      country : ['', [Validators.required]],
      password : ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      confirmPassword : ['', [Validators.required, this.matchValues('password')]],
    })
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
  
  matchValues(matchTo : string): ValidatorFn {
    return ( control : AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {isMatching:true}
    }
  }
  register() {
    const dob= this.getDateOnly(this.registerForm.get('dateOfBirth')?.value)
    this.registerForm.patchValue({dateOfBirth :dob})
    this.accountService.register(this.registerForm.value).subscribe({
      next : _ => { this.router.navigateByUrl('/members')},
      error : error  => this.validationsError =error
      
    })
    
   }
   cancel() {
    this.cancelRegister.emit(false);
   }

  private getDateOnly(dob : string | undefined) {
    if (!dob) return
    return new Date(dob).toISOString().slice(0, 10);
  }

}
