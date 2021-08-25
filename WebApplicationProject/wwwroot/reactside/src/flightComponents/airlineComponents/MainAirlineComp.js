import React, { Component, useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import AirlineDetailsComp from './AirlineDetailsComp';
import AirlineFlightsComp from './AirlineFlightsComp';
import NewFlightComp from './NewFlightComp';
import Navbar from './AirlineNavbar'
import '../navbar.css'
import '../background.css'

const MainAirlineComp = () => {
    let history = useHistory();

    useEffect(() => {
        history.push('/airline/details')
    }, [])

    return (<div>
        <Navbar />
        <Switch>
            <div className="container">
                <Route path='/airline/flights'> <AirlineFlightsComp /> </Route>
                <Route path='/airline/details'> <AirlineDetailsComp /> </Route>
                <Route path='/airline/newFlight'> <NewFlightComp /> </Route>
            </div>
        </Switch>
    </div>)
}

export default MainAirlineComp;