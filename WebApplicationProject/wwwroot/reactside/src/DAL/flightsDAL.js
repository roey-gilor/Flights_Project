import $ from 'jquery';

const getAllAirlineCompanies = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Anonymous/GetAllAirlineCompanies',
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getAllFlights = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Anonymous/GetAllFlights',
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getAllFlightsVacancy = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Anonymous/GetAllFlightsVacancy',
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getFlightsByDepatrureDate = async (date) => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: `/api/Anonymous/GetFlightsByDepatrureDate?date=${date}`,
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getFlightsByLandingDate = async (date) => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: `/api/Anonymous/GetFlightsByLandingDate?date=${date}`,
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getFlightsByDestinationCountry = async (countryName) => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: `/api/Anonymous/GetFlightsByDestinationCountry?countryName=${countryName}`,
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

const getFlightsByOrigionCountry = async (countryName) => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: `/api/Anonymous/GetFlightsByOriginCountry?countryName=${countryName}`,
            contentType: 'application/json',
            dataType: 'json'
        })
        return result
    } catch (error) {
        console.log(error);
    }
}

export default {
    getAllAirlineCompanies, getAllFlights, getAllFlightsVacancy, getFlightsByDepatrureDate,
    getFlightsByLandingDate, getFlightsByDestinationCountry, getFlightsByOrigionCountry
}