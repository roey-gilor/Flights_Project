// Show/hide password onClick of button using Javascript only

// const { event } = require("jquery");

// https://stackoverflow.com/questions/31224651/show-hide-password-onclick-of-button-using-javascript-only

function show() {
    var p = document.getElementById('pwd');
    p.setAttribute('type', 'text');
}

function hide() {
    var p = document.getElementById('pwd');
    p.setAttribute('type', 'password');
}

var pwShown = 0;

document.getElementById("eye").addEventListener("click", function () {
    if (pwShown == 0) {
        pwShown = 1;
        show();
    } else {
        pwShown = 0;
        hide();
    }
}, false);

const login = (event) => {
    event.preventDefault();
    let data = {};
    data.Name = $('#userName')[0].value;
    data.Password = $('#pwd')[0].value;
    let json = JSON.stringify(data);

    let xhr = new XMLHttpRequest();
    xhr.open("POST", '/api/Auth/token', true);
    xhr.setRequestHeader('Content-type',
        'application/json; charset=utf-8');
    xhr.onload = function () {
        jwt = xhr.responseText;
        if (xhr.readyState == 4 && xhr.status == "200") {
            window.localStorage.setItem('JWT', jwt);

            var base64Url = jwt.split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            let role = JSON.parse(jsonPayload)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            window.localStorage.setItem('role', role);

            location.href = 'reactpage/index.html'
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Login Failed',
                text: `One or more of the details are wrong`
            })
        }
    }
    xhr.send(json);
}