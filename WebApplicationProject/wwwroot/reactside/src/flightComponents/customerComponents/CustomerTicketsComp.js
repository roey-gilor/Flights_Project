import React, { useEffect, useState } from 'react'
import CustomerFlightComp from './CustomerFlightComp';
import customerDAL from '../../DAL/customerDAL.js'

const CustomerTicketsComp = () => {
    const [tickets, setTickets] = useState([])

    useEffect(async () => {
        let ticketsArr = await customerDAL.getAllTickets()
        if (ticketsArr !== false) {
            setTickets(ticketsArr)
        }
        else {
            console.log("error");
        }
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