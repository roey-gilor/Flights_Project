import React, { Component, useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import CustomerDetailsComp from './CustomerDetailsComp.js'
import CustomerTicketsComp from './CustomerTicketsComp'
import Navbar from './CustomerNavbar.js'
import '../navbar.css'
import '../background.css'

const MainCustomerComp = () => {
    let history = useHistory();

    useEffect(() => {
        history.push('/customer/tickets')
    }, [])

    return (<div>
        <Navbar />
        <Switch>
            <div className="container">
                <Route path='/customer/tickets'> <CustomerTicketsComp /> </Route>
                <Route path='/customer/details'> <CustomerDetailsComp /> </Route>
            </div>
        </Switch>
    </div>)
}

export default MainCustomerComp;