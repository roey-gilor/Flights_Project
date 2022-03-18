import $ from 'jquery';
const jwt = localStorage.getItem('JWT')

const getWaitingAdmins = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Administrator/GetAllWaitingAdmins',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return result
    } catch (error) {
        return error.status;
    }
}

const approveAdmin = async (admin) => {
    let result;
    try {
        result = await $.ajax({
            type: "POST",
            url: '/api/Administrator/ApproveAdmin',
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

const rejectAdmin = async (admin) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Administrator/RejectAdmin',
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

const getWaitingAirlines = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Administrator/GetAllWaitingAirlines',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return result
    } catch (error) {
        return error.status;
    }
}

const approveAirline = async (airline) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Administrator/ApproveAirline',
            contentType: 'application/json',
            data: airline,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

const rejectAirline = async (airline) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Administrator/RejectAirline',
            contentType: 'application/json',
            data: airline,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        throw error.status;
    }
}

const getAllAirlines = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Administrator/GetAllAirlines',
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

const updateAirline = async (airline) => {
    try {
        await $.ajax({
            url: "/api/Administrator/UpdateAirlineDetails",
            type: "PUT",
            contentType: 'application/json',
            data: airline,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const deleteAirline = async (airline) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Administrator/RemoveAirline',
            contentType: 'application/json',
            data: airline,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return error.status
    }
}

const createNewAirline = async (airline) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Administrator/CreateAirlineCompany',
            contentType: 'application/json',
            data: airline,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const GetAllCustomers = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Administrator/GetAllCustomers',
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

const updateCustomer = async (customer) => {
    try {
        await $.ajax({
            url: "/api/Administrator/UpdateCustomerDetails",
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

const deleteCustomer = async (customer) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Administrator/RemoveCustomer',
            contentType: 'application/json',
            data: customer,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return error.status
    }
}

const createNewCustomer = async (customer) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Administrator/CreateCustomer',
            contentType: 'application/json',
            data: customer,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const GetAllAdmins = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Administrator/GetAllAdmins',
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

const updateAdmin = async (admin) => {
    try {
        await $.ajax({
            url: "/api/Administrator/UpdateAdmin",
            type: "PUT",
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const deleteAdmin = async (admin) => {
    try {
        await $.ajax({
            url: '/api/Administrator/RemoveAdmin',
            type: "DELETE",
            contentType: 'application/json',
            data: admin,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return error.status
    }
}

const createNewAdmin = async (admin) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Administrator/CreateAdmin',
            contentType: 'application/json',
            data: admin,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const updateCountry = async (country) => {
    try {
        await $.ajax({
            url: "/api/Administrator/UpdateCountry",
            type: "PUT",
            contentType: 'application/json',
            data: country,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const deleteCountry = async (countryId) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: `/api/Administrator/RemoveCountry?countryId=${countryId}`,
            contentType: 'application/json',
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return error.status
    }
}

const createNewCountry = async (country) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Administrator/CreateCountry',
            contentType: 'application/json',
            data: country,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

const updatePassword = async (currentPassword, password) => {
    try {
        await $.ajax({
            type: "PUT",
            url: `/Administrator/ChangeAdminPassword?oldPassword=${currentPassword}&newPassword=${password}`,
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

const updateMyUser = async (admin) => {
    try {
        await $.ajax({
            url: "/api/Administrator/UpdateMyDetails",
            type: "PUT",
            contentType: 'application/json',
            data: admin,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true;
    } catch (error) {
        return { status: error.status, error: error.responseText };
    }
}

export default {
    getWaitingAdmins, approveAdmin, rejectAdmin, getWaitingAirlines, approveAirline, rejectAirline,
    getAllAirlines, updateAirline, deleteAirline, createNewAirline, GetAllCustomers, updateCustomer,
    deleteCustomer, createNewCustomer, GetAllAdmins, updateAdmin, deleteAdmin, createNewAdmin,
    updateCountry, deleteCountry, createNewCountry, updatePassword, updateMyUser
}