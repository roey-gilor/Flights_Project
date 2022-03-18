import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import flightsDAL from '../../DAL/flightsDAL';
import airlineDAL from '../../DAL/airlineDAL';
import customerDAL from '../../DAL/customerDAL';
import '../../tableDesign.css'
import '../designedSelect.css'
import Swal from 'sweetalert2';

const FlightsComp = () => {
    let history = useHistory()
    const [flights, setFlights] = useState([])
    const [mainFligthsArr, setMainFligthsArr] = useState([])
    const [airlines, setAirlines] = useState([])
    const [countries, setCountries] = useState([])
    const [showPurchaseDiv, setShowPurchaseDiv] = useState(false)
    const [selectedFlight, setSelectedFlight] = useState({})

    useEffect(async () => {
        let countriesArr = await airlineDAL.GetAllCountries();
        setCountries(JSON.parse(countriesArr))
        let flightsArr = await flightsDAL.getAllFlights();
        setFlights(flightsArr)
        setMainFligthsArr(flightsArr)
        let airlinesArr = await flightsDAL.getAllAirlineCompanies();
        setAirlines(airlinesArr)
    }, [])

    if (flights) {
        flights.sort((a, b) => (a.Id > b.Id) ? 1 : (a.Id < b.Id) ? -1 : 0)
    }

    const getFlightsByOrigionCountry = async (country) => {
        document.getElementById('airline').value = 'Airline Name'
        document.getElementById('destination').value = 'Destination Country'
        let flightsArr = await flightsDAL.getFlightsByOrigionCountry(country)
        setFlights(flightsArr)
    }

    const getFlightsByDestinationCountry = async (country) => {
        document.getElementById('airline').value = 'Airline Name'
        document.getElementById('origion').value = 'Origion Country'
        let flightsArr = await flightsDAL.getFlightsByDestinationCountry(country)
        setFlights(flightsArr)
    }

    const getFlightsByAirlineCompany = (airline) => {
        document.getElementById('origion').value = 'Origion Country'
        document.getElementById('destination').value = 'Destination Country'
        let flightsArr = mainFligthsArr.filter(flight => flight.AirlineCompanyName == airline)
        setFlights(flightsArr)
    }

    const getFlightsByDepatrureDate = async (date) => {
        document.getElementById('airline').value = 'Airline Name'
        document.getElementById('origion').value = 'Origion Country'
        document.getElementById('destination').value = 'Destination Country'
        let flightsArr = await flightsDAL.getFlightsByDepatrureDate(date)
        setFlights(flightsArr)
    }

    const getFlightsByLandingDate = async (date) => {
        document.getElementById('airline').value = 'Airline Name'
        document.getElementById('origion').value = 'Origion Country'
        document.getElementById('destination').value = 'Destination Country'
        let flightsArr = await flightsDAL.getFlightsByLandingDate(date)
        setFlights(flightsArr)
    }

    let airlinesSelector = <select style={{ backgroundColor: 'white' }} class="select-text" id='airline'
        onChange={(e) => { getFlightsByAirlineCompany(e.target.value) }}>
        <option disabled selected>Airline Name</option>
        {airlines.map(a => {
            return <option value={a.Name}>{a.Name}</option>
        })} </select>

    let origionCountriesSelector = <select style={{ backgroundColor: 'white' }} class="select-text" id='origion'
        onChange={(e) => { getFlightsByOrigionCountry(e.target.value) }}>
        <option disabled selected>Origion Country</option>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>

    let destinationCountriesSelector = <select style={{ backgroundColor: 'white' }} class="select-text" id='destination'
        onChange={(e) => { getFlightsByDestinationCountry(e.target.value) }}>
        <option disabled selected>Destination Country</option>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>

    const designDate = (date) => {
        let part1 = date.split(':')[0]
        let part2 = date.split(':')[1]
        date = part1 + ':' + part2
        date = date.replace('T', ' ')
        return date
    }

    const showToPurchaseDiv = (flight) => {
        let currentDate = new Date();
        let detpartureDate = new Date(flight.Departure_Time)
        if (detpartureDate.getTime() > currentDate.getTime()) {
            let role = localStorage.getItem('JWT')
            if (role !== 'Customer') {
                Swal.fire({
                    title: 'Only customer can buy tickets',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                })
            } else {
                setShowPurchaseDiv(true)
                setSelectedFlight(flight)
            }
        } else {
            Swal.fire({
                icon: 'error',
                title: 'You can\'t buy ticket to this flight',
                text: `The flight has already taken off`
            })
        }
    }

    const getRow = (flight, index) => {
        return <tr onClick={() => { showToPurchaseDiv(flight) }} className="popupOpen" key={index}>
            <td>{flight.Id}</td>
            <td>{flight.AirlineCompanyName}</td>
            <td>{flight.OrigionCountry}</td>
            <td>{flight.DestinationCountry}</td>
            <td>{designDate(flight.Departure_Time)}</td>
            <td>{designDate(flight.Landing_Time)}</td>
            <td>{flight.RemainingTickets}</td>
        </tr>
    }

    let flightsToRender;
    if (flights) {
        flightsToRender = <table id='flightsTbl' border='1'>
            {/* Creating table header */}
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Airline Company</th>
                    <th>Origion Country</th>
                    <th>Destination Country</th>
                    <th>Departure Time</th>
                    <th>Landing Time</th>
                    <th>Remaining Tickets</th>
                </tr>
            </thead>
            <tbody>
                {flights.map((flight, index) => {
                    return getRow(flight, index)
                })}
            </tbody>
        </table>
    }

    const resetAll = () => {
        document.getElementById('airline').value = 'Airline Name'
        document.getElementById('origion').value = 'Origion Country'
        document.getElementById('destination').value = 'Destination Country'
        document.getElementById('departure').value = ''
        document.getElementById('landing').value = ''
        setFlights(mainFligthsArr)
    }

    const buyTicket = () => {
        let flight = JSON.stringify({
            Id: selectedFlight.Id,
            AirlineCompanyName: selectedFlight.AirlineCompanyName,
            OrigionCountry: selectedFlight.OrigionCountry,
            DestinationCountry: selectedFlight.DestinationCountry,
            Departure_Time: selectedFlight.Departure_Time,
            Landing_Time: selectedFlight.Landing_Time,
            RemainingTickets: selectedFlight.RemainingTickets
        })
        Swal.fire({
            title: 'Are you sure?',
            text: "You can only buy one ticket to this flight",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, buy it!'
        }).then(async (result) => {
            if (result.isConfirmed) {
                let res = await customerDAL.purchaseTicket(flight)
                if (res === true) {
                    Swal.fire(
                        'Ticket was purchased succefully!',
                        'You can see it on the website',
                        'success'
                    ).then(() => {
                        let flightsArr = flights
                        let index = flightsArr.findIndex(flight => flight.Id === selectedFlight.Id)
                        flightsArr[index].RemainingTickets--
                        setFlights(flightsArr)
                        setSelectedFlight({})
                        setShowPurchaseDiv(false)
                    })
                } else {
                    if (res === 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Can buy ticket only once!',
                            text: 'Could not purchase ticket again'
                        })
                    } else {
                        if (res === 406) {
                            Swal.fire({
                                icon: 'error',
                                title: 'No tickets left',
                                text: 'Could not purchase ticket again'
                            })
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Something wrong happend',
                                text: 'Could not purchase ticket'
                            })
                        }
                    }
                }
            }
        })
    }

    const goBack = () => {
        let role = localStorage.getItem('JWT')
        switch (role) {
            case 'Administrator':
                history.push("/admin/details")
                break;
            case 'Airline Company':
                history.push('/airline/details')
                break;
            case 'Customer':
                history.push('/customer/tickets')
                break;
            default: 
                window.location.href = "/FlightPages/loginPage.html"
        }
    }

    let buyDiv
    if (JSON.stringify(selectedFlight) !== '{}') {
        buyDiv = (
            <div>  <br /> <br />
                <h1>Purchase Ticket</h1> <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {selectedFlight.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Airline Company Name: </span> {selectedFlight.AirlineCompanyName} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Origion Country: </span> {selectedFlight.OrigionCountry} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Destination Country: </span> {selectedFlight.DestinationCountry} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Departure Time: </span> {designDate(selectedFlight.Departure_Time)}<br /> <br />
                <span style={{ fontWeight: 'bold' }}> Landing Time: </span> {designDate(selectedFlight.Landing_Time)} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Remaining Tickets: </span> {selectedFlight.RemainingTickets} <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowPurchaseDiv(false) }} /> {' '}
                <input type='button' value='Purchase' style={{ width: '80px' }} className="button btnBorder btnLightBlue" onClick={() => buyTicket()} />
            </div>
        )
    }

    return (<div> <br />
        {!showPurchaseDiv ?
            <div>
                <h1>Flights</h1>  <br /> <br />
                <input type='button' onClick={() => { resetAll() }}
                    className="button btnBorder btnLightBlue" style={{ width: '80px' }} value='Reset All' /> {' '}
                <input type='button' style={{ width: '85px' }} className="button btnBorder btnBlueGreen" value='Go back'
                    onClick={() => { goBack() }} /> <br /> <br />
                {airlinesSelector} {' '}
                {origionCountriesSelector} {' '}
                {destinationCountriesSelector} {' '} <br /> <br />
                <label>Departure Date: </label>
                <input type='date' id='departure' onChange={(e) => { getFlightsByDepatrureDate(e.target.value) }} /> {' '}
                <label>Landing Date: </label>
                <input type='date' id='landing' onChange={(e) => { getFlightsByLandingDate(e.target.value) }} /> <br /> <br />
                {flightsToRender}
            </div>
            : buyDiv}
    </div>)
}

export default FlightsComp