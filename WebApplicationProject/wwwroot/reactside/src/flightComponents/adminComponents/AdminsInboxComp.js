import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const AdminsInboxComp = () => {
    let history = useHistory();
    const [admins, setAdmins] = useState([])
    const [errorMessage, setErrorMessage] = useState('')

    useEffect(async () => {
        let res = await adminDAL.getWaitingAdmins();
        if (typeof (res) == 'object') {
            setAdmins(res)
        } else {
            let msg = res === 403 ? 'You are not allowed to watch waiting admins' :
                'There is a problem to present waiting admins';
            setErrorMessage(msg)
        }
    }, [])

    const approveAdmin = (admin) => {
        Swal.fire({
            title: 'Are you sure?',
            text: "The user will get admin authorization",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Accept the admin'
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await adminDAL.approveAdmin(JSON.stringify(admin))
                    Swal.fire(
                        'Admin was accepted succefully!',
                        'Now the admin can login to the system',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/adminsInbox')
                    })
                } catch (status) {
                    if (status == 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to accept admin'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not approve admin'
                        })
                    }
                }
            }
        })
    }

    const rejectAdmin = (admin) => {
        Swal.fire({
            title: 'Are you sure?',
            text: "The user will not be created",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, reject the admin'
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await adminDAL.rejectAdmin(JSON.stringify(admin))
                    Swal.fire(
                        'Admin was rejected succefully!',
                        'User wasn\'t created',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/adminsInbox')
                    })
                } catch (status) {
                    if (status == 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to reject admin'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not reject admin'
                        })
                    }
                }
            }
        })
    }

    const getRow = (admin, index) => {
        return <tr className="popupOpen" key={index}>
            <td>{admin.First_Name}</td>
            <td>{admin.Last_Name}</td>
            <td>{admin.Level}</td>
            <td>{admin.User.User_Name}</td>
            <td>{admin.User.Password}</td>
            <td>{admin.User.Email}</td>
            <td><input type='button' onClick={() => { approveAdmin(admin) }} style={{ width: '75px' }}
                className="button btnBorder btnBlueGreen" value='Accept' /></td>
            <td><input type='button' onClick={() => { rejectAdmin(admin) }} className="button btnBorder btnRed" value='Reject' /></td>
        </tr>
    }

    let dataToDisplay;
    if (admins && errorMessage === '') {
        dataToDisplay = <table border='1'>
            <thead>
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Level</th>
                    <th>User Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Accept</th>
                    <th>Reject</th>
                </tr>
            </thead>
            <tbody>
                {admins.map((admin, index) => {
                    return getRow(admin, index)
                })}
            </tbody>
        </table>
    } else {
        dataToDisplay = <h1 style={{ color: 'red' }}>{errorMessage}</h1>
    }

    return (<div>
        {errorMessage === '' && <h1>Admins Inbox</h1>}
        {dataToDisplay}
    </div>)
}

export default AdminsInboxComp;