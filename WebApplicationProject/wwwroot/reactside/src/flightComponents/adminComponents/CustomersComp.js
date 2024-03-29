import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const CustomersComp = () => {
    let history = useHistory();
    const [customers, setCustomers] = useState([])
    const [customerToUpdate, setCustomerToUpdate] = useState({})
    const [showUpdateDiv, setShowUpdateDiv] = useState(false)
    const [showCreateDiv, setShowCreateDiv] = useState(false)

    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [address, setAddress] = useState('')
    const [phoneNum, setPhoneNum] = useState('')
    const [creditCard, setCreditCard] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')
    const [prefix, setPrefix] = useState('')

    useEffect(async () => {
        let customersArr = await adminDAL.GetAllCustomers();
        setCustomers(customersArr)
    }, [])

    if (customers) {
        customers.sort((a, b) => (a.Id > b.Id) ? 1 : (a.Id < b.Id) ? -1 : 0)
    }

    const showDiv = (customer) => {
        setCustomerToUpdate(customer)
        setFirstName(customer.First_Name)
        setLastName(customer.Last_Name)
        setAddress(customer.Address)
        setPrefix(customer.Phone_No.split('-')[0])
        setPhoneNum(customer.Phone_No.split('-')[1])
        setCreditCard(customer.Credit_Card_No)
        setUserName(customer.User.User_Name)
        setPassword(customer.User.Password)
        setConfirm(customer.User.Password)
        setEmail(customer.User.Email)
        setShowUpdateDiv(true)
    }

    const deleteCustomer = (customer) => {
        Swal.fire({
            title: 'Are you sure you want to delete this customer?',
            text: "The user won\'t be able to login again",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(async (result) => {
            if (result.isConfirmed) {
                let res = await adminDAL.deleteCustomer(JSON.stringify(customer))
                if (res === true) {
                    Swal.fire(
                        'Customer has been deleted succefully!',
                        'The user cannot longer login to the system anymore',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/customers')
                    })
                } else {
                    if (res === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not delete customer'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to delete customer'
                        })
                    }
                }
            }
        })
    }

    const getRow = (customer, index) => {
        return <tr className="popupOpen" key={index}>
            <td>{customer.Id}</td>
            <td>{customer.First_Name}</td>
            <td>{customer.Last_Name}</td>
            <td>{customer.Address}</td>
            <td>{customer.Phone_No}</td>
            <td>{customer.Credit_Card_No}</td>
            <td>{customer.User.User_Name}</td>
            <td>{customer.User.Password}</td>
            <td>{customer.User.Email}</td>
            <td><input type='button' onClick={() => { showDiv(customer) }} className="button btnBorder btnBlueGreen" value='Edit' /></td>
            <td><input type='button' onClick={() => { deleteCustomer(customer) }} className="button btnBorder btnLightBlue" value='Delete' /></td>
        </tr>
    }

    let customersToRender;
    if (customers) {
        customersToRender = <table border='1'>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Address</th>
                    <th>Phone</th>
                    <th>Credit Card</th>
                    <th>User Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {customers.map((customer, index) => {
                    return getRow(customer, index)
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
        if (address.length < 10) {
            return 'Address is too short'
        }
        if (phoneNum.length !== 7) {
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

    const updateCustomer = () => {
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
                    let customer = JSON.stringify({
                        Id: customerToUpdate.Id,
                        First_Name: firstName,
                        Last_Name: lastName,
                        Address: address,
                        Phone_No: prefix + '-' + phoneNum,
                        Credit_Card_No: creditCard,
                        User_Id: customerToUpdate.User_Id,
                        User: {
                            Id: customerToUpdate.User_Id,
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 3
                        }
                    })
                    result = await adminDAL.updateCustomer(customer)
                    if (result === true) {
                        Swal.fire(
                            'Customer was updated succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/customers')
                        })
                    } else {
                        if (result.status === 403) {
                            let text = result.error.split("_")[1];
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${text} is allready taken`
                            })
                        } else {
                            if (result.status === 401) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `You are not allowed to update details`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let divUpdate
    if (customerToUpdate !== {}) {
        divUpdate = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {customerToUpdate.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> First Name: </span>
                <input type='text' defaultValue={firstName} onChange={(e) => setFirstName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Last Name: </span>
                <input type='text' defaultValue={lastName} onChange={(e) => setLastName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Address: </span>
                <input type='text' defaultValue={address} onChange={(e) => setAddress(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Phone: </span>
                <select id="prefixNum" value={prefix} onChange={(e) => setPrefix(e.target.value)}>
                    <option value="050">050</option>
                    <option value="051">051</option>
                    <option value="052">052</option>
                    <option value="053">053</option>
                    <option value="054">054</option>
                    <option value="055">055</option>
                </select>
                <input type='text' defaultValue={phoneNum} onChange={(e) => setPhoneNum(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Credit Card: </span>
                <input type='text' defaultValue={creditCard} onChange={(e) => setCreditCard(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' defaultValue={userName} onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' defaultValue={password} onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' defaultValue={confirm} onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' defaultValue={email} onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowUpdateDiv(false) }} /> {' '}
                <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => updateCustomer()} />
            </div>
        )
    }

    const createNewCustomer = () => {
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
                    let customer = JSON.stringify({
                        First_Name: firstName,
                        Last_Name: lastName,
                        Address: address,
                        Phone_No: prefix + '-' + phoneNum,
                        Credit_Card_No: creditCard,
                        User: {
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 3
                        }
                    })
                    result = await adminDAL.createNewCustomer(customer)
                    if (result === true) {
                        Swal.fire(
                            'Customer was created succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/customers')
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
                                    text: `You are not allowed to create customer`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not create new customer`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let createDiv
    if (customerToUpdate !== {}) {
        createDiv = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> First Name: </span>
                <input type='text' onChange={(e) => setFirstName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Last Name: </span>
                <input type='text' onChange={(e) => setLastName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Address: </span>
                <input type='text' onChange={(e) => setAddress(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Phone: </span>
                <select onChange={(e) => setPrefix(e.target.value)}>
                    <option value="050">050</option>
                    <option value="051">051</option>
                    <option value="052">052</option>
                    <option value="053">053</option>
                    <option value="054">054</option>
                    <option value="055">055</option>
                </select>
                <input type='text' onChange={(e) => setPhoneNum(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Credit Card: </span>
                <input type='text' onChange={(e) => setCreditCard(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowCreateDiv(false) }} /> {' '}
                <input type='button' value='Create' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => createNewCustomer()} />
            </div>
        )
    }

    const showDivCreate = () => {
        setFirstName('')
        setLastName('')
        setAddress('')
        setPrefix('050')
        setPhoneNum('')
        setCreditCard('')
        setUserName('')
        setPassword('')
        setConfirm('')
        setEmail('')
        setShowCreateDiv(true)
    }

    let createBtn = <input type='button' value='Create New Customer' className="button btnBorder btnLightBlue"
        style={{ width: '170px' }} onClick={() => { showDivCreate() }} />

    return (<div>
        <h1>Customers</h1> <br />
        {(!showUpdateDiv && !showCreateDiv) && createBtn}
        {showUpdateDiv ? divUpdate : showCreateDiv ? createDiv : customersToRender}
    </div>)
}

export default CustomersComp;