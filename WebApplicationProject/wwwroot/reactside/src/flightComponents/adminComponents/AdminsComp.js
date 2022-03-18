import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const AdminsComp = () => {
    let history = useHistory();
    const [admins, setAdmins] = useState([])
    const [adminToUpdate, setAdminToUpdate] = useState({})
    const [showUpdateDiv, setShowUpdateDiv] = useState(false)
    const [showCreateDiv, setShowCreateDiv] = useState(false)

    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [level, setLevel] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')

    const getUserId = () => {
        const jwt = localStorage.getItem('JWT')
        var base64Url = jwt.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload).mainUserId;
    }

    useEffect(async () => {
        let adminsArr = await adminDAL.GetAllAdmins();
        let id = getUserId()
        adminsArr = adminsArr.filter(admin => admin.Id != id);
        setAdmins(adminsArr)
    }, [])

    if (admins) {
        admins.sort((a, b) => (a.Id > b.Id) ? 1 : (a.Id < b.Id) ? -1 : 0)
    }

    const showDiv = (admin) => {
        setAdminToUpdate(admin)
        setFirstName(admin.First_Name)
        setLastName(admin.Last_Name)
        setUserName(admin.User.User_Name)
        setPassword(admin.User.Password)
        setConfirm(admin.User.Password)
        setEmail(admin.User.Email)
        setShowUpdateDiv(true)
    }

    const deleteAdmin = (admin) => {
        Swal.fire({
            title: 'Are you sure you want to delete this admin?',
            text: "The user won\'t be able to login anymore",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(async (result) => {
            if (result.isConfirmed) {
                let res = await adminDAL.deleteAdmin(JSON.stringify(admin))
                if (res === true) {
                    Swal.fire(
                        'Admin has been deleted succefully!',
                        'The user cannot longer login to the system',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/admins')
                    })
                } else {
                    if (res === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not delete amdin'
                        })
                    } else {
                        if (res === 403) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'You are not allowed to delete admin'
                            })
                        }
                    }
                }
            }
        })
    }

    const getRow = (admin, index) => {
        return <tr className="popupOpen" key={index}>
            <td>{admin.Id}</td>
            <td>{admin.First_Name}</td>
            <td>{admin.Last_Name}</td>
            <td>{admin.Level}</td>
            <td>{admin.User.User_Name}</td>
            <td>{admin.User.Password}</td>
            <td>{admin.User.Email}</td>
            <td><input type='button' onClick={() => { showDiv(admin) }} className="button btnBorder btnBlueGreen" value='Edit' /></td>
            <td><input type='button' onClick={() => { deleteAdmin(admin) }} className="button btnBorder btnLightBlue" value='Delete' /></td>
        </tr>
    }

    let adminsToRender;
    if (admins) {
        adminsToRender = <table border='1'>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Level</th>
                    <th>User Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {admins.map((admin, index) => {
                    return getRow(admin, index)
                })}
            </tbody>
        </table>
    }

    const validateDetails = () => {
        if (userName.length < 5) {
            return 'User name is too short'
        } else {
            if (userName.length > 20) {
                return 'User name is too long'
            }
        }
        if (password !== confirm) {
            return 'You must confirm your password currectly!'
        }
        if (password.length < 5) {
            return 'Your password is too short'
        } else {
            if (password.length > 20) {
                return 'Your password is too long'
            }
        }
        if (firstName.length < 2) {
            return 'First name is too short'
        }
        if (lastName.length < 2) {
            return 'Last name is too short'
        }
        if (!email.includes('@') || email.length < 4) {
            return 'Email must be in right format'
        }
        return '';
    }

    const updateAdmin = () => {
        let result
        Swal.fire({
            title: 'Are you OK with the changes?',
            text: "You can review then again before accepting",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, change it'
        }).then(async (_result) => {
            if (_result.isConfirmed) {
                let error = validateDetails();
                if (error !== '') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: `${error}`
                    })
                } else {
                    let admin = JSON.stringify({
                        Id: adminToUpdate.Id,
                        First_Name: firstName,
                        Last_Name: lastName,
                        Level: adminToUpdate.Level,
                        User_Id: adminToUpdate.User_Id,
                        User: {
                            Id: adminToUpdate.User_Id,
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 1
                        }
                    })
                    result = await adminDAL.updateAdmin(admin)
                    if (result === true) {
                        Swal.fire(
                            'Airline was updated succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/admins')
                        })
                    } else {
                        if (result.status === 400) {
                            let text = result.error.split("_")[1];
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${text} is allready taken`
                            })
                        } else {
                            if (result.status === 403) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `You are not allowed to update details`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not update details`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let divUpdate
    if (adminToUpdate !== {}) {
        divUpdate = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {adminToUpdate.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> First Name: </span>
                <input type='text' defaultValue={firstName} onChange={(e) => setFirstName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Last Name: </span>
                <input type='text' defaultValue={lastName} onChange={(e) => setLastName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Level: </span> {adminToUpdate.Level} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' defaultValue={userName} onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' defaultValue={password} onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' defaultValue={confirm} onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' defaultValue={email} onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowUpdateDiv(false) }} /> {' '}
                <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => updateAdmin()} />
            </div>
        )
    }

    const createNewAdmin = () => {
        let result;
        Swal.fire({
            title: 'Are you sure with the details?',
            text: "You can review them again before creating",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, create it!'
        }).then(async (_result) => {
            if (_result.isConfirmed) {
                let error = validateDetails();
                if (error !== '') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: `${error}`
                    })
                } else {
                    let admin = JSON.stringify({
                        First_Name: firstName,
                        Last_Name: lastName,
                        Level: level,
                        User_Id: adminToUpdate.User_Id,
                        User: {
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 1
                        }
                    })
                    result = await adminDAL.createNewAdmin(admin)
                    if (result === true) {
                        Swal.fire(
                            'Admin was created succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/admins')
                        })
                    } else {
                        if (result.status === 400) {
                            let text = result.error.split("_")[1];
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${text} is allready taken`
                            })
                        } else {
                            if (result.status === 403) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `You are not allowed to create admin`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not create new admin`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let createDiv
    if (adminToUpdate !== {}) {
        createDiv = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {adminToUpdate.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> First Name: </span>
                <input type='text' defaultValue={firstName} onChange={(e) => setFirstName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Last Name: </span>
                <input type='text' defaultValue={lastName} onChange={(e) => setLastName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Level: </span>
                <select id="level" onChange={(e) => setLevel(e.target.value)}>
                    <option value="1" selected>Lowest Level</option>
                    <option value="2">Medium Level</option>
                    <option value="3">Highest Level</option>
                </select> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' defaultValue={userName} onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' defaultValue={password} onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' defaultValue={confirm} onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' defaultValue={email} onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowCreateDiv(false) }} /> {' '}
                <input type='button' value='Create' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => createNewAdmin()} />
            </div>
        )
    }

    const showDivCreate = () => {
        setFirstName('')
        setLastName('')
        setLevel('1')
        setUserName('')
        setPassword('')
        setConfirm('')
        setEmail('')
        setShowCreateDiv(true)
    }

    let createBtn = <input type='button' value='Create New Admin' className="button btnBorder btnLightBlue"
        style={{ width: '150px' }} onClick={() => { showDivCreate() }} />

    return (<div>
        <h1>Admins</h1> <br />
        {(!showUpdateDiv && !showCreateDiv) && createBtn}
        {showUpdateDiv ? divUpdate : showCreateDiv ? createDiv : adminsToRender}
    </div>)
}

export default AdminsComp;