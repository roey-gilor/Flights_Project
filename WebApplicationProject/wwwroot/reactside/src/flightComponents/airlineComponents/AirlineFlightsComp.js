import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import airlineDAL from '../../DAL/airlineDAL.js'
import $ from 'jquery';
import '../../tableDesign.css'
import 'bootstrap';
import { Modal } from 'react-bootstrap'
import Swal from 'sweetalert2';
// import 'bootstrap/dist/css/bootstrap.min.css';

const AirlineFlightsComp = () => {
    let history = useHistory();
    const [flights, setFlights] = useState([])
    const [flightsTickets, setFlightsTickets] = useState([])
    const [ticketsArr, setTicketsArr] = useState([])
    const [countries, setCountries] = useState([])
    const [flightToUpdate, setFlightToUpdate] = useState({})
    const [showUpdateDiv, setShowUpdateDiv] = useState(false)

    const [origionCountry, setOrigionCountry] = useState('')
    const [destinationContry, setDestinationContry] = useState('')
    const [departureTime, setDepartureTime] = useState('')
    const [landingTime, setLandingTime] = useState('')

    const [show, setShow] = useState(false)
    const handleClose = () => setShow(false)
    const handleShow = () => setShow(true)

    useEffect(async () => {
        let flightsArr = await airlineDAL.getAirlineFlights();
        setFlights(flightsArr)
        let flightsTicketsArr = await airlineDAL.getFlightsTickets();
        setFlightsTickets(flightsTicketsArr)
        let countriesArr = await airlineDAL.GetAllCountries();
        setCountries(JSON.parse(countriesArr))
    }, [])

    useEffect(() => {
        setTimeout(() => {
            $(`#origionCountry option:contains(${flightToUpdate.OrigionCountry})`).prop('selected', true)
            $(`#destinationCountry option:contains(${flightToUpdate.DestinationCountry})`).prop('selected', true)
        }, 10)
    }, [flightToUpdate])

    if (flights) {
        flights.sort((a, b) => (a.Id > b.Id) ? 1 : (a.Id < b.Id) ? -1 : 0)
    }

    let origionCountriesSelector = <select id="origionCountry" onChange={(e) => setOrigionCountry(e.target.value)}>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>
    let destinationCountriesSelector = <select id="destinationCountry" onChange={(e) => setDestinationContry(e.target.value)}>
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
    const showCustomers = (id) => {
        let ticketsArr = flightsTickets.filter(ticket => ticket.Flight_Id == id);
        setTicketsArr(ticketsArr)
        handleShow()
    }

    const showDiv = (flight) => {
        setFlightToUpdate(flight)
        setOrigionCountry(flight.OrigionCountry)
        setDestinationContry(flight.DestinationCountry)
        setDepartureTime(flight.Departure_Time)
        setLandingTime(flight.Landing_Time)
        setShowUpdateDiv(true)
    }

    const deleteFlight = (flight) => {
        let currentDate = new Date();
        let detpartureDate = new Date(flight.Departure_Time)
        if (detpartureDate.getTime() > currentDate.getTime()) {
            Swal.fire({
                title: 'Are you sure you want to cancel this flight?',
                text: "All customers tickets purchases will be canceled",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, cancel it!'
            }).then(async (result) => {
                if (result.isConfirmed) {
                    let res = await airlineDAL.deleteFlight(JSON.stringify(flight))
                    if (res) {
                        Swal.fire(
                            'Flight has been canceled succefully!',
                            'All customers tickets purchases are be canceled',
                            'success'
                        ).then(() => {
                            history.push('/airline/details')
                            history.push('/airline/flights')
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not delete flight'
                        })
                    }
                }
            })
        } else {
            Swal.fire({
                icon: 'error',
                title: 'You can\'t cancel Flight',
                text: `The flight has already taken off`
            })
        }
    }

    const getRow = (flight, index) => {
        return <tr className="popupOpen" key={index}>
            <td onClick={() => showCustomers(flight.Id)}>{flight.Id}</td>
            <td>{flight.OrigionCountry}</td>
            <td>{flight.DestinationCountry}</td>
            <td>{designDate(flight.Departure_Time)}</td>
            <td>{designDate(flight.Landing_Time)}</td>
            <td>{flight.RemainingTickets}</td>
            <td><input type='button' onClick={() => { showDiv(flight) }} className="button btnBorder btnBlueGreen" value='Edit' /></td>
            <td><input type='button' onClick={() => { deleteFlight(flight) }} className="button btnBorder btnLightBlue" value='Cancel' /></td>
        </tr>
    }

    let flightsToRender;
    if (flights) {
        flightsToRender = <table id='flightsTbl' border='1'>
            {/* Creating table header */}
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Origion Country</th>
                    <th>Destination Country</th>
                    <th>Departure Time</th>
                    <th>Landing Time</th>
                    <th>Remaining Tickets</th>
                    <th>Edit</th>
                    <th>Cancel</th>
                </tr>
            </thead>
            <tbody>
                {flights.map((flight, index) => {
                    return getRow(flight, index)
                })}
            </tbody>
        </table>
    }

    const updateFlight = () => {
        let _departureTime = new Date(departureTime)
        let _landingTime = new Date(landingTime)
        if (_departureTime.getTime() > _landingTime.getTime()) {
            Swal.fire({
                icon: 'error',
                title: 'Could not update flight',
                text: 'Departure time must be before landing time'
            })
        } else {
            let flight = JSON.stringify({
                Id: flightToUpdate.Id,
                AirlineCompanyName: flightToUpdate.AirlineCompanyName,
                OrigionCountry: origionCountry,
                DestinationCountry: destinationContry,
                Departure_Time: departureTime,
                Landing_Time: landingTime,
                RemainingTickets: flightToUpdate.RemainingTickets
            })
            Swal.fire({
                title: 'Are you Ok with the changes?',
                text: "You might want to review the changed again",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, update details!'
            }).then(async (result) => {
                if (result.isConfirmed) {
                    let res = await airlineDAL.updateFlight(flight)
                    if (res) {
                        Swal.fire(
                            'Flight details have been updated succefully!',
                            'Updated Data will be displayed in the website',
                            'success'
                        ).then(() => {
                            let flightsArr = flights
                            let index = flightsArr.findIndex(flight => flight.Id === flightToUpdate.Id)
                            flightsArr[index] = JSON.parse(flight)
                            setFlights(flightsArr)
                            setShowUpdateDiv(false)
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not update flight details'
                        })
                    }
                }
            })
        }
    }

    let divUpdate
    if (flightToUpdate !== {}) {
        divUpdate = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {flightToUpdate.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Airline Company Name: </span> {flightToUpdate.AirlineCompanyName} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Origion Country: </span> {origionCountriesSelector} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Destination Country: </span> {destinationCountriesSelector} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Departure Time: </span> <input type='datetime-local'
                    onChange={(e) => setDepartureTime(e.target.value)} defaultValue={flightToUpdate.Departure_Time} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Landing Time: </span> <input type='datetime-local'
                    onChange={(e) => setLandingTime(e.target.value)} defaultValue={flightToUpdate.Landing_Time} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Remaining Tickets: </span> {flightToUpdate.RemainingTickets} <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowUpdateDiv(false) }} /> {' '}
                <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => updateFlight()} />
            </div>
        )
    }

    const ModalContent = () => {
        return (
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                </Modal.Header>
                <Modal.Body>
                    <table border='1'>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>First Name</th>
                                <th>Last Name</th>
                                <th>Address</th>
                                <th>Phone</th>
                            </tr>
                        </thead>
                        <tbody>
                            {ticketsArr.map((ticket, index) => {
                                return <tr key={index}>
                                    <td>{ticket.Id}</td>
                                    <td>{ticket.First_Name}</td>
                                    <td>{ticket.Last_Name}</td>
                                    <td>{ticket.Address}</td>
                                    <td>{ticket.Phone_No}</td>
                                </tr>
                            })}
                        </tbody>
                    </table>
                </Modal.Body>
            </Modal>
        )
    }
    let headline = show ? 'Flight\'s passengers' : showUpdateDiv ? 'Update Flight' : 'Flights'

    return (<div>
        <h1>{headline}</h1>
        {show ? <ModalContent /> : showUpdateDiv ? divUpdate : flightsToRender}
    </div>)
}

export default AirlineFlightsComp;