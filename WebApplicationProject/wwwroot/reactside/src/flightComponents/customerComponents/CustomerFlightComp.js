import React, { Component } from 'react'
import { useHistory } from 'react-router-dom';
import customerDAL from '../../DAL/customerDAL.js'
import Swal from 'sweetalert2';
import '../alerts.css'

const CustomerFlightComp = (props) => {
    let flight = props.flight
    let history = useHistory();

    const deleteTicket = async () => {
        let currentDate = new Date();
        let detpartureDate = new Date(flight.Departure_Time)
        if (detpartureDate.getTime() > currentDate.getTime()) {
            let ticket = JSON.stringify({
                Id: flight.Ticket_Id,
                Flight_Id: flight.Id,
                AirlineCompanyName: flight.AirlineCompanyName,
                OrigionCountry: flight.OrigionCountry,
                DestinationCountry: flight.DestinationCountry,
                Departure_Time: flight.Departure_Time,
                Landing_Time: flight.Landing_Time
            })
            let result = await customerDAL.deleteTicket(ticket)
            if (result === true) {
                Swal.fire(
                    'Ticket purchase was canceled succefully!',
                    'You can purchase tickets to another flights',
                    'success'
                ).then(() => {
                    history.push('/customer/details')
                    history.push('/customer/tickets')
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'You can\'t cancel ticket purchase',
                    text: `You are not allowed to cancel purchase`
                })
            };
        } else {
            Swal.fire({
                icon: 'error',
                title: 'You can\'t cancel ticket purchase',
                text: `The flight has already taken off`
            })
        }
    }

    return (<div style={{ backgroundColor: 'whitesmoke', border: '1px solid black' }} >
        <span style={{ fontWeight: 'bold' }}> Company: </span>  {flight.AirlineCompanyName} {' '}
        <span style={{ fontWeight: 'bold' }}> Departure date: </span> {flight.Departure_Time.split('T')[0]} {' '}
        <span style={{ fontWeight: 'bold' }}> Origion Country: </span>  {flight.OrigionCountry} {' '}
        <span style={{ fontWeight: 'bold' }}> Departure Time: </span>  {flight.Departure_Time.split('T')[1]} {' '} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Destination Country: </span>  {flight.DestinationCountry} {' '}
        <span style={{ fontWeight: 'bold' }}> Landing date: </span>  {flight.Landing_Time.split('T')[0]} {' '}
        <span style={{ fontWeight: 'bold' }}> Landing Time: </span>  {flight.Landing_Time.split('T')[1]} {' '} <br /> <br />
        <input type="button" style={{ backgroundColor: 'salmon' }} value="Cancel Ticket" onClick={deleteTicket} />
    </div>)
}

export default CustomerFlightComp;