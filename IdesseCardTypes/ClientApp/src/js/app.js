import '../css/app.css';

import * as Turbo from '@hotwired/turbo'

// Turns Turbo Drive on/off (default on). 
// If off, we must opt-in to Turbo Drive on a per-link and per-form basis using data-turbo="true".
Turbo.session.drive = true

// Turbo event listeners
document.addEventListener('turbo:load', function (e) {
  console.log('turbo:load', e);
});

document.addEventListener('turbo:visit', function (e) {
  console.log('turbo:visit', e);
});

document.addEventListener('turbo:frame-load', function (e) {
  console.log('turbo:frame-load', e);
});

//const loginForm = document.getElementById("login-form");

//loginForm.addEventListener('turbo:submit-start', function (e) {
//  const userName = document.getElementById("user-name");
//  const password = document.getElementById("password");

//  console.log(userName.value);
//  console.log(password.value);

//  if (userName.value.trim().length === 0 || password.value.trim().length ===0 ) {
//    event.detail.formSubmission.stop();
//	}
//});

