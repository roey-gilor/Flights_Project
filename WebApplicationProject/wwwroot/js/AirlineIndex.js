let jqXhr = $.ajax({
    url: "/api/Anonymous/GetAllCountries",
    type: "GET",
    contentType: 'application/json'
}).done(() => {
    let namesStr = jqXhr.responseText.split(',')
    $.each(namesStr, function (index, value) {
        if (index % 2 === 0) {
            return;
        }
        name = value.split(":")[1].replace("\"", "").replace("\"", "")
        if (index + 1 === namesStr.length) {
            name = name.substring(0, name.length - 1)
        }
        $("#countryName").append(new Option(name.substring(0, name.length - 1), name.substring(0, name.length - 1)));
    });
})

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
            let airline = JSON.stringify({
                Name: $("#airline").val(),
                Country_Name: $("#countryName").val(),
                User: {
                    User_Name: $("#userName").val(),
                    Password: $("#Password").val(),
                    Email: $("#email").val()
                }
            })
            let jqXhr = $.ajax({
                url: "/api/Anonymous/CreateNewWaitingAirline",
                type: "POST",
                data: airline,
                contentType: 'application/json'
            }).done(() => {
                Swal.fire(
                    'New Airline creation request was created succefully!',
                    'You can login to the system only after admin will approve your request',
                    'success'
                )
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
    let inputs = [$("#userName").val(), $("#Password").val(), $("#conPassword").val(), $("#email").val(), $("#airline").val()]
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
        return 'You must confirm your password correctly!'
    }
    if ($("#Password").val().length < 5) {
        return 'Your password is too short'
    } else {
        if ($("#Password").val().length > 20) {
            return 'Your password is too long'
        }
    }
    if ($("#airline").val().length < 3) {
        return 'Airline name is too short'
    }
    return '';
}