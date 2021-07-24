const createNewCustomer = () => {
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