import React, { useEffect } from 'react'
import { Route, useHistory, Switch } from 'react-router-dom';
import AdminsInboxComp from './AdminsInboxComp'
import AirlinesInboxComp from './AirlinesInboxComp'
import AirlinesComp from './AirlinesComp'
import CustomersComp from './CustomersComp'
import DetailsComp from './DetailsComp'
import Navbar from './AdminNavbar'
import '../navbar.css'
import '../background.css'

const MainAdminComp = () => {
    let history = useHistory();

    useEffect(() => {
        history.push('/admin/details')
    }, [])

    return (<div>
        <Navbar />
        <Switch>
            <div className="container">
                <Route path='/admin/adminsInbox'> <AdminsInboxComp /> </Route>
                <Route path='/admin/airlinesInbox'> <AirlinesInboxComp /> </Route>
                <Route path='/admin/airlines'> <AirlinesComp /> </Route>
                <Route path='/admin/customers'> <CustomersComp /> </Route>
                <Route path='/admin/details'> <DetailsComp /> </Route>
            </div>
        </Switch>
    </div>)
}

export default MainAdminComp;