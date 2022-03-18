import React, { Component, useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import MainCustomerComp from '../customerComponents/MainCustomerComp.js';
import MainAirlineComp from '../airlineComponents/MainAirlineComp.js';
import MainAdminComp from '../adminComponents/MainAdminComp.js';
import FlightsComp from '../mainComponents/FlightsComp.js'

const MainComp = () => {
    let history = useHistory();

    useEffect(() => {
        let role = localStorage.getItem('role')
        switch (role) {
            case 'Administrator':
                history.push("/admin/details")
                break;
            case 'Airline Company':
                history.push('/airline/details')
                break;
            case 'Customer':
                history.push('/customer/details')
                break;
            default:
                history.push('/flights')
        }
    }, [])

    return (<div id="bg">
        <Switch>
            <Route path='/customer'> <MainCustomerComp /> </Route>
            <Route path='/airline'> <MainAirlineComp /> </Route>
            <Route path='/admin' > <MainAdminComp /> </Route>
            <Route path='/flights' > <FlightsComp /> </Route>
        </Switch>
    </div>)
}

export default MainComp;