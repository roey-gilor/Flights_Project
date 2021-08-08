import React, { Component, useEffect, useState } from 'react'
import $ from 'jquery';
import CustomerFlightComp from './CustomerFlightComp';

const CustomerTicketsComp = () => {
    const [tickets, setTickets] = useState([])

    useEffect(() => {
        let jwt = localStorage.getItem('JWT')
        $.ajax({
            type: "GET",
            url: '/api/Customer/GetAllCustomerFlights',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        }).done(function (response) {
            console.log(response);
            const ticketsArr = JSON.parse(response);
            setTickets(ticketsArr);
        }).fail(function (err) {
            console.log(err);
        });
    }, [])

    let flightsToRender;
    if (tickets.length !== 0) {
        flightsToRender = tickets.map(flight => {
            return <CustomerFlightComp key={flight.id} flight={flight} />
        })
    }

    return (<div>
        <h1> viewing & managing your tickets</h1> <br /> <br />
        {flightsToRender}  <br />  <br />
    </div>)
}

export default CustomerTicketsComp;