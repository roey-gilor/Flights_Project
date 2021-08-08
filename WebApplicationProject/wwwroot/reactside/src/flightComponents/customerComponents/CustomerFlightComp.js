import React, { Component } from 'react'
import $ from 'jquery';

const CustomerFlightComp = (props) => {
    let flight = props.flight

    const deleteTicket = () => {
        let jwt = localStorage.getItem('JWT')
        let ticket = JSON.stringify({
            Id: flight.Ticket_Id,
            Flight_Id: flight.Id,
            AirlineCompanyName: flight.AirlineCompanyName,
            OrigionCountry: flight.OrigionCountry,
            DestinationCountry: flight.DestinationCountry,
            Departure_Time: flight.Departure_Time,
            Landing_Time: flight.Landing_Time
        })
        $.ajax({
            type: "DELETE",
            url: '/api/Customer/CancelTicketPurchase',
            contentType: 'application/json',
            data: ticket,
            headers: {
                Authorization: 'Bearer ' + jwt
            }
        }).done(function (response) {

        }).fail(function (err) {

        });
    }

    return (<div style={{ backgroundColor: 'whitesmoke', border: '1px solid black' }} >
        <span style={{ fontWeight: 'bold' }}> Company: </span>  {flight.AirlineCompanyName} {' '}
        <span style={{ fontWeight: 'bold' }}> Departure date: </span> {flight.Departure_Time.split('T')[0]} {' '}
        <span style={{ fontWeight: 'bold' }}> Origion Country: </span>  {flight.OrigionCountry} {' '}
        <span style={{ fontWeight: 'bold' }}> Departure Time: </span>  {flight.Departure_Time.split('T')[1]} {' '} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Destination Country: </span>  {flight.DestinationCountry} {' '}
        <span style={{ fontWeight: 'bold' }}> Landing date: </span>  {flight.Landing_Time.split('T')[0]} {' '}
        <span style={{ fontWeight: 'bold' }}> Landing Time: </span>  {flight.Landing_Time.split('T')[1]} {' '} <br /> <br />
        <input type="button" style={{ backgroundColor: 'salmon' }} value="Cancel Ticket" />
    </div>)
}

export default CustomerFlightComp;