import React, { Component, useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import $ from 'jquery';
import Swal from 'sweetalert2';
import '../alerts.css'

const CustomerDetailsComp = () => {
    let history = useHistory();
    const [user, setUser] = useState({})
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [address, setAddress] = useState('')
    const [phoneNum, setPhoneNum] = useState('')
    const [creditCard, setCreditCard] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')
    const [currentPassword, setCurrentPassword] = useState('')
    const [prefix, setPrefix] = useState('')

    useEffect(() => {
        let jwt = localStorage.getItem('JWT')
        $.ajax({
            type: "GET",
            url: 'https://localhost:44309/api/Customer/GetCustomerDetails',
            contentType: 'application/json',
            dataType: 'json',
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        }).done(function (response) {
            setUser(response)
            setFirstName(response.First_Name)
            setLastName(response.Last_Name)
            setAddress(response.Address)
            setPhoneNum(response.Phone_No)
            setCreditCard(response.Credit_Card_No)
            setPrefix(response.Phone_No.split('-')[0])
            if (response.User != undefined) {
                setUserName(response.User.User_Name)
                setPassword(response.User.Password)
                setEmail(response.User.Email)
                setCurrentPassword(response.User.Password)
                setConfirm(response.User.Password)
            }
        }).fail(function (err) {
            console.log(err);
        });
    }, [])

    const updateCustomer = () => {
        let jwt = localStorage.getItem('JWT')
        let phone = phoneNum.includes('-') ? phoneNum.split('-')[1] : phoneNum;
        let customer = JSON.stringify({
            Id: user.Id,
            First_Name: firstName,
            Last_Name: lastName,
            Address: address,
            Phone_No: prefix + '-' + phone,
            Credit_Card_No: creditCard,
            User_Id: user.User_Id,
            User: {
                Id: user.User_Id,
                User_Name: userName,
                Password: password,
                Email: email,
                User_Role: 3
            }
        })
        $.ajax({
            url: "https://localhost:44309/api/Customer/UpdateCustomerDetails",
            type: "PUT",
            contentType: 'application/json',
            data: customer,
            headers: {
                'Authorization': 'Bearer ' + jwt
            }
        }).done((response) => {
            Swal.fire(
                'Your details have been updated succefully!',
                'You can change it again anytime',
                'success'
            ).then(() => {
                history.push('/customer/tickets')
                history.push('/customer/details')
            })
        }).fail((err) => {
            if (err.status === 403) {
                let text = err.responseText.split("_")[1];
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `${text} is allready taken`
                })
            }
            if (err.status === 401) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `You are not allowed to update details`
                })
            }
        })
    }

    const updateDetails = () => {
        let jwt = localStorage.getItem('JWT')
        let error = validateDetails();
        if (error !== '') {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `${error}`
            })
        } else {
            if (currentPassword !== password) {
                $.ajax({
                    type: "PUT",
                    url: `https://localhost:44309/api/Customer/ChangeCustomerPassword?oldPassword=${currentPassword}&newPassword=${password}`,
                    contentType: 'application/json',
                    headers: {
                        'Authorization': 'Bearer ' + jwt
                    }
                }).done(function (response) {
                    updateCustomer();
                }).fail(function (err) {
                    if (err.status === 403) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: `New password can\'t be like the old one`
                        }).then(() => {
                            history.push('/customer/tickets')
                            history.push('/customer/details')
                        })
                    }
                    if (err.status === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: `You are not allowed to change password`
                        }).then(() => {
                            history.push('/customer/tickets')
                            history.push('/customer/details')
                        })
                    }
                });
            } else {
                updateCustomer();
            }
        }
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
            return 'You must confirm your password correctly!'
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
        if (address.length < 10) {
            return 'Address is too short'
        }
        if (phoneNum.split('-')[1].length !== 7) {
            return 'Phone number must include 7 digits'
        }
        if (creditCard.length !== 16) {
            return 'Credit card number must include 16 digits'
        }
        if (!email.includes('@') || email.length < 4) {
            return 'Email must be in right format'
        }
        return '';
    }

    return (<div>
        <h1>Personal Details</h1> <br />
        <span style={{ fontWeight: 'bold' }}> First Name: </span> <input type='text' defaultValue={user.First_Name}
            onChange={(e) => { setFirstName(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Last Name: </span> <input type='text' defaultValue={user.Last_Name}
            onChange={(e) => { setLastName(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Address: </span> <input type='text' defaultValue={user.Address}
            onChange={(e) => { setAddress(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Phone Number: </span>
        <select id="prefixNum" value={prefix} onChange={(e) => setPrefix(e.target.value)}>
            <option value="050">050</option>
            <option value="051">051</option>
            <option value="052">052</option>
            <option value="053">053</option>
            <option value="054">054</option>
            <option value="055">055</option>
        </select>
        <input type='text' defaultValue={phoneNum.split('-')[1]}
            onChange={(e) => { setPhoneNum(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Credit Card Number: </span> <input type='text' defaultValue={user.Credit_Card_No}
            onChange={(e) => { setCreditCard(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> User Name: </span> <input type='text' defaultValue={userName}
            onChange={(e) => { setUserName(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Password: </span> <input type='text' defaultValue={password}
            onChange={(e) => { setPassword(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Confirm Password: </span> <input type='text' defaultValue={confirm}
            onChange={(e) => { setConfirm(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Email: </span> <input type='email' defaultValue={email}
            onChange={(e) => { setEmail(e.target.value) }} /> <br /> <br />
        <input type='button' value='Edit' style={{ backgroundColor: 'whitesmoke', height: '50px', width: '100px', fontSize: '25px', fontFamily: 'Franklin Gothic Medium' }}
            onClick={updateDetails} />
    </div>)
}

export default CustomerDetailsComp;