import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import airlineDAL from '../../DAL/airlineDAL.js'
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const NewFlightComp = () => {
    let history = useHistory();
    let now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    let currentTime = now.toISOString().slice(0, 16);

    const [companyName, setCompanyName] = useState('')
    const [countries, setCountries] = useState([])
    const [origionCountry, setOrigionCountry] = useState('')
    const [destinationContry, setDestinationContry] = useState('')
    const [departureTime, setDepartureTime] = useState(currentTime)
    const [landingTime, setLandingTime] = useState(currentTime)
    const [ticketsAmount, seTticketsAmount] = useState(1)

    useEffect(async () => {
        let airline = await airlineDAL.getAirlineDetails();
        setCompanyName(airline.Name)
        let countriesArr = await airlineDAL.GetAllCountries();
        setOrigionCountry(JSON.parse(countriesArr)[0].Name)
        setDestinationContry(JSON.parse(countriesArr)[0].Name)
        setCountries(JSON.parse(countriesArr))
    }, [])

    let origionCountriesSelector = <select id="origionCountry" onChange={(e) => setOrigionCountry(e.target.value)}>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>
    let destinationCountriesSelector = <select id="destinationCountry" onChange={(e) => setDestinationContry(e.target.value)}>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>

    const createNewFlight = () => {
        let ticketsNum = parseFloat(ticketsAmount)
        if (ticketsAmount < 1 || ticketsAmount === '' || !Number.isInteger(ticketsNum)) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid ticket amount',
                text: 'Must be at least 1 and an integer number'
            })
        }
        else {
            let _departureTime = new Date(departureTime)
            let _landingTime = new Date(landingTime)
            if (_departureTime.getTime() > _landingTime.getTime()) {
                Swal.fire({
                    icon: 'error',
                    title: 'Could not create flight',
                    text: 'Departure time must be before landing time'
                })
            } else {
                let flight = JSON.stringify({
                    AirlineCompanyName: companyName,
                    OrigionCountry: origionCountry,
                    DestinationCountry: destinationContry,
                    Departure_Time: departureTime,
                    Landing_Time: landingTime,
                    RemainingTickets: ticketsAmount
                })
                Swal.fire({
                    title: 'Are you sure you are OK with flight details?',
                    text: "you can review details before approving the creation",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, create it!'
                }).then(async (result) => {
                    if (result.isConfirmed) {
                        let res = await airlineDAL.createNewFlight(flight)
                        if (res) {
                            Swal.fire(
                                'Flight has been created succefully!',
                                'Flight will be displayed in the website',
                                'success'
                            ).then(() => {
                                history.push('/airline/details')
                                history.push('/airline/newFlight')
                            })
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Something wrong happend',
                                text: 'Could not create flight'
                            })
                        }
                    }
                })
            }
        }
    }

    return (<div>
        <h1>Create New flight</h1> <br />
        <span style={{ fontWeight: 'bold' }}> Airline Company Name: </span> {companyName} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Origion Country: </span> {origionCountriesSelector} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Destination Country: </span> {destinationCountriesSelector} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Departure Time: </span> <input type='datetime-local' min={currentTime} defaultValue={currentTime}
            onChange={(e) => setDepartureTime(e.target.value)} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Landing Time: </span> <input type='datetime-local' min={currentTime} defaultValue={currentTime}
            onChange={(e) => setLandingTime(e.target.value)} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Tickets Amount: </span>
        <input style={{ width: '60px' }} type='number' min='1' defaultValue='1' onChange={(e) => seTticketsAmount(e.target.value)} /> <br /> <br />
        <input type='button' value='Create' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => createNewFlight()} />
    </div>)
}

export default NewFlightComp;