import $ from 'jquery';
const jwt = localStorage.getItem('JWT')

const getCustomerDetails = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Customer/GetCustomerDetails',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const updatePassword = async (currentPassword, password) => {
    try {
        await $.ajax({
            type: "PUT",
            url: `/api/Customer/ChangeCustomerPassword?oldPassword=${currentPassword}&newPassword=${password}`,
            contentType: 'application/json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const updateCustomer = async (customer) => {
    try {
        await $.ajax({
            url: "/api/Customer/UpdateCustomerDetails",
            type: "PUT",
            contentType: 'application/json',
            data: customer,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const deleteTicket = async (ticket) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Customer/CancelTicketPurchase',
            contentType: 'application/json',
            data: ticket,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch {
        return false
    }
}

const getAllTickets = async () => {
    let result
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Customer/GetAllCustomerFlights',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return result
    } catch (error) {
        return false;
    }
}

const purchaseTicket = async (ticket) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Customer/PurchaseTicket',
            contentType: 'application/json',
            data: ticket,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return error.status
    }
}

export default { getCustomerDetails, updatePassword, updateCustomer, deleteTicket, getAllTickets, purchaseTicket }