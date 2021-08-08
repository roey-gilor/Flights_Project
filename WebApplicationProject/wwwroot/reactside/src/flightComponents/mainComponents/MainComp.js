import React, { Component, useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import MainCustomerComp from '../customerComponents/MainCustomerComp.js';
import MainAirlineComp from '../airlineComponents/MainAirlineComp.js';
import MainAdminComp from '../adminComponents/MainAdminComp.js';

const MainComp = () => {
    let history = useHistory();
    useEffect(() => {
        let role = 'Customer';
        switch (role) {
            case 'Administrator':
                history.push("/admin")
                break;
            case 'Airline Company':
                history.push('/airline')
                break;
            case 'Customer':
                history.push('/customer/tickets')
                break;
            default:
                history.push('/')
        }
    }, [])

    return (<div>
        <Switch>
            <Route path='/customer'> <MainCustomerComp /> </Route>
            <Route path='/airline'> <MainAirlineComp /> </Route>
            <Route path='/admin' > <MainAdminComp /> </Route>
        </Switch>
    </div>)
}

export default MainComp;