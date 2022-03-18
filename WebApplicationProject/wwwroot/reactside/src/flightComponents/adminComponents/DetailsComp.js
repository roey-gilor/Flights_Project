import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const DetailsComp = () => {
    let history = useHistory();
    const [admin, setAdmin] = useState({})
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')
    const [currentPassword, setCurrentPassword] = useState('')
    const [notAllowed, setNotAllowed] = useState(false)

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
        if (id != 0) {
            let _admin = adminsArr.find(admin => admin.Id == id);
            setAdmin(_admin)
            setFirstName(_admin.First_Name)
            setLastName(_admin.Last_Name)
            if (_admin.User !== undefined) {
                setUserName(_admin.User.User_Name)
                setPassword(_admin.User.Password)
                setConfirm(_admin.User.Password)
                setCurrentPassword(_admin.User.Password)
                setEmail(_admin.User.Email)
            }
        } else {
            setNotAllowed(true)
        }
    }, [])

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

    const updateAdmin = async () => {
        let result
        let _admin = JSON.stringify({
            Id: admin.Id,
            First_Name: firstName,
            Last_Name: lastName,
            Level: admin.Level,
            User_Id: admin.User_Id,
            User: {
                Id: admin.User_Id,
                User_Name: userName,
                Password: password,
                Email: email,
                User_Role: 1
            }
        })
        result = await adminDAL.updateMyUser(_admin)
        if (result === true) {
            Swal.fire(
                'Your details have been updated succefully!',
                'You can change it again anytime',
                'success'
            ).then(() => {
                history.push('/admin/airlines')
                history.push('/admin/details')
            })
        } else {
            if (result.status === 403) {
                let text = result.error.split("_")[1];
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `${text} is allready taken`
                })
            }
            if (result.status === 401) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `You are not allowed to update details`
                })
            }
        }
    }

    const updateDetails = async () => {
        let result;
        let error = validateDetails();
        if (error !== '') {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `${error}`
            })
        } else {
            if (currentPassword !== password) {
                result = await adminDAL.updatePassword(currentPassword, password)
                if (result === true) {
                    updateAdmin();
                } else {
                    if (result.status === 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: `New password can\'t be like the old one`
                        }).then(() => {
                            history.push('/airline/flights')
                            history.push('/airline/details')
                        })
                    }
                    if (result.status === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: `You are not allowed to change password`
                        }).then(() => {
                            history.push('/admin/airlines')
                            history.push('/admin/details')
                        })
                    }
                }
            } else {
                updateAdmin();
            }
        }
    }

    return (<div>
        {!notAllowed ?
            <div>
                <h1>Personal Details</h1>
                <span style={{ fontWeight: 'bold' }}> ID: </span> {admin.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> First Name: </span>
                <input type='text' defaultValue={firstName} onChange={(e) => setFirstName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Last Name: </span>
                <input type='text' defaultValue={lastName} onChange={(e) => setLastName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Level: </span> {admin.Level} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' defaultValue={userName} onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' defaultValue={password} onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' defaultValue={confirm} onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' defaultValue={email} onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => updateDetails()} />
            </div>
            : <h1 style={{ color: 'red' }}>Main Admin is not allowed to update details</h1>
        }
    </div>)
}

export default DetailsComp;