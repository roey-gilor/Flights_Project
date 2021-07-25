
const createNewCustomer = () => {
    if (!validateForm()) {
        event.preventDefault();
        let error = validatePassword();
        if (error !== '') {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `${error}`
            })
        }
        else {
            let customer = JSON.stringify({
                First_Name: $("#firstName").val(),
                Last_Name: $("#lastName").val(),
                Address: $("#address").val(),
                Phone_No: $("#prefixNum").val() + '-' + $("#suffixNum").val(),
                Credit_Card_No: $("#card").val(),
                User: {
                    User_Name: $("#userName").val(),
                    Password: $("#Password").val(),
                    Email: $("#email").val(),
                    User_Role: 3
                }
            })
            let jqXhr = $.ajax({
                url: "/api/Anonymous/CreateNewCustomer",
                type: "POST",
                data: customer,
                contentType: 'application/json'
            }).done(() => {
                Swal.fire(
                    'New Customer was created succefully!',
                    'You can login to the system now!',
                    'success'
                )
            }).fail(() => {
                let text = (jqXhr.responseText.split("\"")[2]).split("_")[1];
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `${text} is allready taken`
                })
            })
        }
    }
}

const validateForm = () => {
    let inputs = [$("#firstName").val(), $("#lastName").val(), $("#address").val(), $("#suffixNum").val(),
    $("#userName").val(), $("#Password").val(), $("#email").val(), $("#card").val()]
    return inputs.some(val => val.length === 0)
}

const validatePassword = () => {
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
    return '';
}