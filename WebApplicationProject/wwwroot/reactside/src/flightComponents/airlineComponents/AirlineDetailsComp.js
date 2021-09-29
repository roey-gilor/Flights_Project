import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import airlineDAL from '../../DAL/airlineDAL.js'
import '../../tableDesign.css'
import $ from 'jquery';
import Swal from 'sweetalert2';

const AirlineDetailsComp = () => {
    let history = useHistory();
    const [airline, setAirline] = useState({})
    const [countries, setCountries] = useState([])
    const [companyName, setCompanyName] = useState('')
    const [userName, setUserName] = useState('')
    const [password, setPassword] = useState('')
    const [confirm, setConfirm] = useState('')
    const [email, setEmail] = useState('')
    const [country, setCountry] = useState('')
    const [currentPassword, setCurrentPassword] = useState('')

    useEffect(async () => {
        let airline = await airlineDAL.getAirlineDetails();
        setAirline(airline)
        setCountry(airline.Country_Name)
        setCompanyName(airline.Name)
        if (airline.User != undefined) {
            setUserName(airline.User.User_Name)
            setPassword(airline.User.Password)
            setEmail(airline.User.Email)
            setCurrentPassword(airline.User.Password)
            setConfirm(airline.User.Password)
        }
        let countriesArr = await airlineDAL.GetAllCountries();
        setCountries(JSON.parse(countriesArr))
        setTimeout(() => {
            $(`#countries option:contains(${airline.Country_Name})`).prop('selected', true)
        }, 10)
    }, [])

    let countriesSelector = <select id="countries" onChange={(e) => setCountry(e.target.value)}>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>


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

    const updateAirline = async () => {
        let result
        let _airline = JSON.stringify({
            Id: airline.Id,
            Name: companyName,
            Country_Name: country,
            User_Id: airline.User_Id,
            User: {
                Id: airline.User_Id,
                User_Name: userName,
                Password: password,
                Email: email,
                User_Role: 2
            }
        })
        result = await airlineDAL.updateAirline(_airline)
        if (result === true) {
            Swal.fire(
                'Your details have been updated succefully!',
                'You can change it again anytime',
                'success'
            ).then(() => {
                history.push('/airline/flights')
                history.push('/airline/details')
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
                result = await airlineDAL.updatePassword(currentPassword, password)
                if (result === true) {
                    updateAirline();
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
                            history.push('/airline/flights')
                            history.push('/airline/details')
                        })
                    }
                }
            } else {
                updateAirline();
            }
        }
    }

    return (<div>
        <h1>Personal Details</h1> <br />
        <span style={{ fontWeight: 'bold' }}> Company Name: </span> <input type='text' defaultValue={airline.Name}
            onChange={(e) => { setCompanyName(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Country: </span> {countriesSelector} <br /> <br />
        <span style={{ fontWeight: 'bold' }}> User Name: </span> <input type='text' defaultValue={userName}
            onChange={(e) => { setUserName(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Email: </span> <input type='email' defaultValue={email}
            onChange={(e) => { setEmail(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Password: </span> <input type='text' defaultValue={password}
            onChange={(e) => { setPassword(e.target.value) }} /> <br /> <br />
        <span style={{ fontWeight: 'bold' }}> Confirm Password: </span> <input type='text' defaultValue={confirm}
            onChange={(e) => { setConfirm(e.target.value) }} /> <br /> <br />
        <input type='button' value='Edit' style={{ height: '50px', width: '100px', fontSize: '25px' }}
            className="button btnBorder btnLightBlue" onClick={() => updateDetails()} />
    </div>)
}

export default AirlineDetailsComp;