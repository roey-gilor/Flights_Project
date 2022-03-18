import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom';
import adminDAL from '../../DAL/adminDAL';
import airlineDAL from '../../DAL/airlineDAL.js'
import '../../tableDesign.css'
import '../designedSelect.css'
import Swal from 'sweetalert2';

const CountriesComp = () => {
    let history = useHistory()
    const [countries, setCountries] = useState([])
    const [country, setCountry] = useState('')
    const [editedCountry, setEditedcountry] = useState('')
    const [createdCountry, setCreatedCountry] = useState('')
    const [showUpdateDiv, setShowUpdateDiv] = useState(false)
    const [showCreateDiv, setShowCreateDiv] = useState(false)

    useEffect(async () => {
        let countriesArr = await airlineDAL.GetAllCountries();
        setCountries(JSON.parse(countriesArr))
    }, [])

    useEffect(() => {
        if (showUpdateDiv === true) {
            setEditedcountry(country)
        }
    }, [showUpdateDiv])

    let countriesSelector = <select class="select-text" id="countries"
        onChange={(e) => {
            setCountry(e.target.value)
            setShowUpdateDiv(false)
        }}>
        <option value='' disabled selected></option>
        {countries.map(c => {
            return <option value={c.Name}>{c.Name}</option>
        })} </select>

    const updateCountry = () => {
        Swal.fire({
            title: 'Are you sure?',
            text: "Changing Country Name will make many changes in the system",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, change it'
        }).then(async (result) => {
            if (result.isConfirmed) {
                if (editedCountry.length < 2) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: `Country Name is too short!`
                    })
                } else {
                    let countryToUpdate = countries.find(_country => _country.Name === country)
                    countryToUpdate.Name = editedCountry
                    let res = await adminDAL.updateCountry(JSON.stringify(countryToUpdate))
                    if (res === true) {
                        Swal.fire(
                            'Country Name was updated succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/countries')
                        })
                    } else {
                        countryToUpdate.Name = country
                        if (res.status === 400) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `Country Name is allready taken`
                            })
                        } else {
                            if (res.status === 403) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `You are not allowed to update country`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not update country`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    const deleteCountry = () => {
        Swal.fire({
            title: 'Are you sure you want to delete this country?',
            text: "This action will cause many changes to all users!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(async (result) => {
            if (result.isConfirmed) {
                let countryToDelete = countries.find(_country => _country.Name === country)
                let res = await adminDAL.deleteCountry(JSON.stringify(countryToDelete.Id))
                if (res === true) {
                    Swal.fire(
                        'Country has been deleted succefully!',
                        'All changes will be updated in the website',
                        'success'
                    ).then(() => {
                        history.push('/admin/details')
                        history.push('/admin/countries')
                    })
                } else {
                    if (res === 401) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Something wrong happend',
                            text: 'Could not delete country'
                        })
                    } else {
                        if (res === 403) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'You are not allowed to delete country'
                            })
                        }
                    }
                }
            }
        })
    }

    let countryDiv
    if (country !== '') {
        let countryToPresent = !showUpdateDiv ? country : <input type='text' defaultValue={country}
            onChange={(e) => { setEditedcountry(e.target.value) }} />
        countryDiv = <div>
            <span style={{ fontWeight: 'bold' }}> Name: </span> {countryToPresent} <br />  <br />
            {!showUpdateDiv ? <input type='button' onClick={() => { setShowUpdateDiv(true) }} className="button btnBorder btnBlueGreen"
                value='Edit' /> : <input type='button' value='Update' style={{ width: '70px' }} className="button btnBorder btnLightBlue"
                    onClick={() => { updateCountry() }} />} {' '}
            {!showUpdateDiv ? <input type='button' className="button btnBorder btnLightBlue" value='Delete'
                onClick={() => { deleteCountry() }} />
                : <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen" onClick={() => { setShowUpdateDiv(false) }} />}
        </div>
    }

    const createNewCountry = () => {
        let result;
        Swal.fire({
            title: 'Are you sure with country name?',
            text: "You can review it again before creating",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, create it!'
        }).then(async (_result) => {
            if (_result.isConfirmed) {
                if (createdCountry.length < 2) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: `Country name is too short!`
                    })
                } else {
                    let country = JSON.stringify({
                        Name: createdCountry
                    })
                    result = await adminDAL.createNewCountry(country)
                    if (result === true) {
                        Swal.fire(
                            'country was created succefully!',
                            'You can change it again anytime',
                            'success'
                        ).then(() => {
                            history.push('/admin/details')
                            history.push('/admin/countries')
                        })
                    } else {
                        if (result.status === 400) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `Country name is allready taken`
                            })
                        } else {
                            if (result.status === 403) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `You are not allowed to create country`
                                })
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `Could not create new country`
                                })
                            }
                        }
                    }
                }
            }
        })
    }

    let createDiv = <div>
        <h2>Create New Country</h2> <br />
        <span style={{ fontWeight: 'bold' }}> Name: </span>
        <input type='text' onChange={(e) => { setCreatedCountry(e.target.value) }} /> <br />  <br />
        <input type='button' value='Close' style={{ width: '70px' }} className="button btnBorder btnBlueGreen"
            onClick={() => {
                setShowCreateDiv(false)
                setCreatedCountry('')
            }} /> {' '}
        <input type='button' value='Create' style={{ width: '70px' }} className="button btnBorder btnLightBlue" onClick={() => createNewCountry()} />
    </div>

    let mainDiv = <div>
        <p>Select country to watch</p> <br />
        <div class="wrap">
            <div class="select">
                {countriesSelector}
                <span class="select-highlight"></span>
                <span class="select-bar"></span>
                <label class="select-label">Select</label>
            </div>
        </div> <br /> <br /> <br />
        {countryDiv} <br /> <br />
        {!showUpdateDiv && <input type='button' value='Create New Country' className="button btnBorder btnLightBlue"
            style={{ width: '160px' }}
            onClick={() => {
                setShowCreateDiv(true)
                setCreatedCountry('')
            }} />}
    </div>

    return (<div>
        <h1>Countries</h1> <br />
        {!showCreateDiv ? mainDiv : createDiv}
    </div>)
}

export default CountriesComp;