const createNewAirline = () => {
    if (!validateForm()) {
        event.preventDefault();
        let error = validateDetails();
        if (error !== '') {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `${error}`
            })
        }
        else {
            let admin = JSON.stringify({
                First_Name: $("#firstName").val(),
                Last_Name: $("#lastName").val(),
                Level: $("#level").val(),
                User: {
                    User_Name: $("#userName").val(),
                    Password: $("#Password").val(),
                    Email: $("#email").val()
                }
            })
            let jqXhr = $.ajax({
                url: "/api/Anonymous/CreateNewWaitingAdmin",
                type: "POST",
                data: admin,
                contentType: 'application/json'
            }).done(() => {
                Swal.fire(
                    'New Admin creation request was created succefully!',
                    'You can login to the system only after admin will approve your request',
                    'success'
                ).then(() => { location.href = "/FlightPages/loginPage.html" })
            }).fail(() => {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `${jqXhr.responseText}`
                })
            })
        }
    }
}

const validateForm = () => {
    let inputs = [$("#firstName").val(), $("#lastName").val(), $("#userName").val(),
    $("#Password").val(), $("#conPassword").val(), $("#email").val()]
    return inputs.some(val => val.length === 0)
}

const validateDetails = () => {
    if ($("#userName").val().length < 5) {
        return 'User name is too short'
    } else {
        if ($("#userName").val().length > 20) {
            return 'User name is too long'
        }
    }
    if ($("#Password").val() !== $("#conPassword").val()) {
        return 'You must confirm your password currectly!'
    }
    if ($("#Password").val().length < 5) {
        return 'Your password is too short'
    } else {
        if ($("#Password").val().length > 20) {
            return 'Your password is too long'
        }
    }
    if ($("#firstName").val().length < 2) {
        return 'First name is too short'
    }
    if ($("#lastName").val().length < 2) {
        return 'Last name is too short'
    }
    return '';
}