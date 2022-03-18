import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const AirlinesInboxComp = () => {
    let history = useHistory();
    const [airlines, setAirlines] = useState([])
    const [errorMessage, setErrorMessage] = useState('')

    useEffect(async () => {
        let res = await adminDAL.getWaitingAirlines();
        if (typeof (res) == 'object') {
            setAirlines(res)
        } else {
            let msg = res === 403 ? 'You are not allowed to watch waiting airlines' :
                'There is a problem to present waiting airlines';
            setErrorMessage(msg)
        }
    }, [])

    const approveAirline = (airline) => {
        Swal.fire({
            title: 'Are you sure?',
            text: "This action will create new airline company",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Accept the airline company'
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await adminDAL.approveAirline(JSON.stringify(airline))
                    Swal.fire(
                        'Airline was accepted succefully!',
                        'Now the airline can login to the system',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/airlinesInbox')
                    })
                } catch (status) {
                    if (status == 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to accept airline'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not approve airline'
                        })
                    }
                }
            }
        })
    }

    const rejectAirline = (airline) => {
        Swal.fire({
            title: 'Are you sure?',
            text: "The user will not be created",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, reject the airline'
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await adminDAL.rejectAirline(JSON.stringify(airline))
                    Swal.fire(
                        'Airline was rejected succefully!',
                        'User wasn\'t created',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/airlinesInbox')
                    })
                } catch (status) {
                    if (status == 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to reject airline'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not reject airline'
                        })
                    }
                }
            }
        })
    }

    const getRow = (airline, index) => {
        return <tr className="popupOpen" key={index}>
            <td>{airline.Name}</td>
            <td>{airline.Country_Name}</td>
            <td>{airline.User.User_Name}</td>
            <td>{airline.User.Password}</td>
            <td>{airline.User.Email}</td>
            <td><input type='button' onClick={() => { approveAirline(airline) }} style={{ width: '75px' }}
                className="button btnBorder btnBlueGreen" value='Accept' /></td>
            <td><input type='button' onClick={() => { rejectAirline(airline) }} className="button btnBorder btnRed" value='Reject' /></td>
        </tr>
    }

    let dataToDisplay;
    if (airlines && errorMessage === '') {
        dataToDisplay = <table border='1'>
            <thead>
                <tr>
                    <th>company Name</th>
                    <th>Country</th>
                    <th>User Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Accept</th>
                    <th>Reject</th>
                </tr>
            </thead>
            <tbody>
                {airlines.map((airline, index) => {
                    return getRow(airline, index)
                })}
            </tbody>
        </table>
    } else {
        dataToDisplay = <h1 style={{ color: 'red' }}>{errorMessage}</h1>
    }

    return (<div>
        {errorMessage === '' && <h1>Airlines Inbox</h1>}
        {dataToDisplay}
    </div>)
}

export default AirlinesInboxComp;