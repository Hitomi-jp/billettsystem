import { Component } from "@angular/core";
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({

  templateUrl: "login.component.html"
})
export class LoginComponent {
  Skjema: FormGroup;

  constructor(private fb: FormBuilder) {
    this.Skjema = fb.group({
      brukernavn: ["", Validators.required],
      passord: ["", Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{8,}')]
      /*At least 8 characters in length
           Lowercase letters
           Uppercase letters
           Numbers
           Special characters */
    });
  }

  onSubmit() {
    console.log("Modellbasert skjema submitted:");
    console.log(this.Skjema);
    console.log(this.Skjema.value.brukernavn);
    console.log(this.Skjema.touched);
  }
}
