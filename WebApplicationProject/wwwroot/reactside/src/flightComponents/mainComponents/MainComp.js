import React, { Component, useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import MainCustomerComp from '../customerComponents/MainCustomerComp.js';
import MainAirlineComp from '../airlineComponents/MainAirlineComp.js';
import MainAdminComp from '../adminComponents/MainAdminComp.js';

const MainComp = () => {
    let history = useHistory();
    const getToken = () => {
        let data = {};
        data.Name = 'danny121121';
        data.Password = 'fdsaa23';
        let json = JSON.stringify(data);

        let xhr = new XMLHttpRequest();
        xhr.open("POST", 'https://localhost:44309/api/Auth/token', true);
        xhr.setRequestHeader('Content-type',
            'application/json; charset=utf-8');
        xhr.onload = function () {
            let jwt = xhr.responseText;
            if (xhr.readyState == 4 && xhr.status == "200") {
                window.localStorage.setItem('JWT', jwt);
            }
        }
        xhr.send(json);
    }

    useEffect(() => {
        getToken();
        let role = 'Administrator';
        switch (role) {
            case 'Administrator':
                history.push("/admin/details")
                break;
            case 'Airline Company':
                history.push('/airline/details')
                break;
            case 'Customer':
                history.push('/customer/tickets')
                break;
            default:
                history.push('/')
        }
    }, [])

    return (<div id="bg">
        <Switch>
            <Route path='/customer'> <MainCustomerComp /> </Route>
            <Route path='/airline'> <MainAirlineComp /> </Route>
            <Route path='/admin' > <MainAdminComp /> </Route>
        </Switch>
    </div>)
}

export default MainComp;