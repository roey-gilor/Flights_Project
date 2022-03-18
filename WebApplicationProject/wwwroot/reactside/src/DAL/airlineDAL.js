import $ from 'jquery';
const jwt = localStorage.getItem('JWT')

const getAirlineFlights = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Airline/GetAllAirlineFlights',
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

const getFlightsTickets = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Airline/GetAllAirlineTickets',
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

const GetAllCountries = async () => {
    let countries;
    try {
        countries = await $.ajax({
            type: "GET",
            url: '/api/Anonymous/GetAllCountries',
            contentType: 'application/json'
        })
        return countries
    }
    catch (error) {
        console.log(error);
    }
}

const updateFlight = async (flight) => {
    try {
        await $.ajax({
            type: "PUT",
            url: '/api/Airline/UpdateFlight',
            contentType: 'application/json',
            data: flight,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        })
        return true
    } catch (error) {
        return false
    }
}

const deleteFlight = async (flight) => {
    try {
        await $.ajax({
            type: "DELETE",
            url: '/api/Airline/CancelFlight',
            contentType: 'application/json',
            data: flight,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch {
        return false
    }
}

const getAirlineDetails = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Airline/GetAirlineDetails',
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

const createNewFlight = async (flight) => {
    try {
        await $.ajax({
            type: "POST",
            url: '/api/Airline/CreateNewFlight',
            contentType: 'application/json',
            data: flight,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        })
        return true
    } catch {
        return false
    }
}

const updatePassword = async (currentPassword, password) => {
    try {
        await $.ajax({
            type: "PUT",
            url: `/api/Airline/ChangeAirlinePassword?oldPassword=${currentPassword}&newPassword=${password}`,
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

const updateAirline = async (airline) => {
    try {
        await $.ajax({
            url: "/api/Airline/UpdateAirlineDetails",
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

export default {
    getAirlineFlights, getFlightsTickets, GetAllCountries,
    updateFlight, deleteFlight, getAirlineDetails, createNewFlight,
    updatePassword, updateAirline
}