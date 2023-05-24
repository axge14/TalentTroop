function login() {
  var username = document.getElementById("username").value;
  var password = document.getElementById("password").value;
  var myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");

  var raw = JSON.stringify({
    username: username,
    password: password,
  });

  var requestOptions = {
    method: "POST",
    headers: myHeaders,
    body: raw,
    redirect: "follow",
  };

  fetch("http://localhost:3001/auth/login", requestOptions)
    .then((response) => response.text())
    .then((result) => console.log(result))
    .catch((error) => {
      console.log("error", error);
    });
    
        var sEmail = $('#username').val();
        if ($.trim(sEmail).length == 0) {
          $('.error-msg.username').show();
          e.preventDefault();
        }
        if (validateEmail(sEmail)) {
          $('.error-msg.username').hide();
        } else {
          $('.error-msg.username').show();
          e.preventDefault();
        }
        if ($('#password').val().length === 0) { //if password field is empty           
          $('.error-msg.password').show();
        }
        if ($('#password').val().length) { //if password has value      
          $('.error-msg.password').hide();
        };
    
}

function register() {
  var email = document.getElementById("email").value;
  var username = document.getElementById("username").value;
  var password = document.getElementById("password").value;
  var myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");

  var raw = JSON.stringify({
    email: email,
    username: username,
    password: password,
  });

  var requestOptions = {
    method: "POST",
    headers: myHeaders,
    body: raw,
    redirect: "follow",
  };

  fetch("http://localhost:3001/auth/register", requestOptions)
    .then((response) => response.text())
    .then((result) => console.log(result))
    .catch((error) => console.log("error", error));
}
