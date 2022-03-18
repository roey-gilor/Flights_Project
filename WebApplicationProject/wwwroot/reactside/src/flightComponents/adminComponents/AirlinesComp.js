import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import airlineDAL from '../../DAL/airlineDAL.js'
import $ from 'jquery';
import '../../tableDesign.css'
import Swal from 'sweetalert2';

const AirlinesComp = () => {
    let history = useHistory();
    const [airlines, setAirlines] = useState([])
    const [countries, setCountries] = useState([])
    const [airlineToUpdate, setAirlineToUpdate] = useState({})
    const [showUpdateDiv, setShowUpdateDiv] = useState(false)
    const [showCreateDiv, setShowCreateDiv] = useState(false)

    const [companyName, setCompanyName] = useState('')
    const [countryName, setCountryName] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')

    useEffect(async () => {
        let airlinesArr = await adminDAL.getAllAirlines();
        setAirlines(airlinesArr)
        let countriesArr = await airlineDAL.GetAllCountries();
        setCountries(JSON.parse(countriesArr))
    }, [])

    useEffect(() => {
        setTimeout(() => {
            $(`#countries option:contains(${airlineToUpdate.Country_Name})`).prop('selected', true)
        }, 10)
    }, [airlineToUpdate])

    if (airlines) {
        airlines.sort((a, b) => (a.Id > b.Id) ? 1 : (a.Id < b.Id) ? -1 : 0)
    }

    let countriesSelector = <select id="countries" onChange={(e) => setCountryName(e.target.value)}>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>

    const showDiv = (airline) => {
        setAirlineToUpdate(airline)
        setCompanyName(airline.Name)
        setCountryName(airline.Country_Name)
        setUserName(airline.User.User_Name)
        setPassword(airline.User.Password)
        setConfirm(airline.User.Password)
        setEmail(airline.User.Email)
        setShowUpdateDiv(true)
    }

    const deleteAirline = (airline) => {
        Swal.fire({
            title: 'Are you sure you want to delete this Airline Company?',
            text: "All it\'s flights will be canceled",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(async (result) => {
            if (result.isConfirmed) {
                let res = await adminDAL.deleteAirline(JSON.stringify(airline))
                if (res === true) {
                    Swal.fire(
                        'Airline has been deleted succefully!',
                        'All it\'s flights are canceled',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/airlines')
                    })
                } else {
                    if (res === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not delete airline'
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You are not allowed to delete airline company'
                        })
                    }
                }
            }
        })
    }

    const getRow = (airline, index) => {
        return <tr className="popupOpen" key={index}>
            <td>{airline.Id}</td>
            <td>{airline.Name}</td>
            <td>{airline.Country_Name}</td>
            <td>{airline.User.User_Name}</td>
            <td>{airline.User.Password}</td>
            <td>{airline.User.Email}</td>
            <td><input type='button' onClick={() => { showDiv(airline) }} className="button btnBorder btnBlueGreen" value='Edit' /></td>
            <td><input type='button' onClick={() => { deleteAirline(airline) }} className="button btnBorder btnLightBlue" value='Delete' /></td>
        </tr>
    }

    let airlinesToRender;
    if (airlines) {
        airlinesToRender = <table border='1'>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>company Name</th>
                    <th>Country</th>
                    <th>User Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {airlines.map((airline, index) => {
                    return getRow(airline, index)
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
        if (companyName.length < 3) {
            return 'Company name is too short'
        }
        if (!email.includes('@') || email.length < 4) {
            return 'Email must be in right format'
        }
        return '';
    }

    const updateAirline = () => {
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
                    let airline = JSON.stringify({
                        Id: airlineToUpdate.Id,
                        Name: companyName,
                        Country_Name: countryName,
                        User_Id: airlineToUpdate.User_Id,
                        User: {
                            Id: airlineToUpdate.User_Id,
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 2
                        }
                    })
                    result = await adminDAL.updateAirline(airline)
                    if (result === true) {
                        Swal.fire(
                            'Airline was updated succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/airlines')
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
    if (airlineToUpdate !== {}) {
        divUpdate = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> ID: </span> {airlineToUpdate.Id} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Airline Company Name: </span>
                <input type='text' defaultValue={airlineToUpdate.Name} onChange={(e) => setCompanyName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Country: </span> {countriesSelector} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' defaultValue={userName} onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' defaultValue={password} onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' defaultValue={confirm} onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' defaultValue={email} onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowUpdateDiv(false) }} /> {' '}
                <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => updateAirline()} />
            </div>
        )
    }

    const createNewAirline = () => {
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
                    let airline = JSON.stringify({
                        Name: companyName,
                        Country_Name: countryName,
                        User: {
                            User_Name: userName,
                            Password: password,
                            Email: email,
                            User_Role: 2
                        }
                    })
                    result = await adminDAL.createNewAirline(airline)
                    if (result === true) {
                        Swal.fire(
                            'Airline company was created succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/airlines')
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
                                    text: `You are not allowed to create airline company`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not create new airline company`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let createDiv
    if (airlineToUpdate !== {}) {
        createDiv = (
            <div>  <br />
                <span style={{ fontWeight: 'bold' }}> Airline Company Name: </span>
                <input type='text' onChange={(e) => setCompanyName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Country: </span> {countriesSelector} <br /> <br />
                <span style={{ fontWeight: 'bold' }}> User Name: </span>
                <input type='text' onChange={(e) => setUserName(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Password: </span>
                <input type='text' onChange={(e) => setPassword(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Confirm Password: </span>
                <input type='text' onChange={(e) => setConfirm(e.target.value)} /> <br /> <br />
                <span style={{ fontWeight: 'bold' }}> Email: </span>
                <input type='text' onChange={(e) => setEmail(e.target.value)} /> <br /> <br />
                <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowCreateDiv(false) }} /> {' '}
                <input type='button' value='Create' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => createNewAirline()} />
            </div>
        )
    }

    const showDivCreate = () => {
        setCompanyName('')
        setCountryName(countries[0].Name)
        setUserName('')
        setPassword('')
        setConfirm('')
        setEmail('')
        setShowCreateDiv(true)
    }

    let createBtn = <input type='button' value='Create New Airline' className="button btnBorder btnLightBlue"
        style={{ width: '150px' }} onClick={() => { showDivCreate() }} />

    return (<div>
        <h1>Airlines</h1> <br />
        {(!showUpdateDiv && !showCreateDiv) && createBtn}
        {showUpdateDiv ? divUpdate : showCreateDiv ? createDiv : airlinesToRender}
    </div>)
}

export default AirlinesComp;