import React, { useEffect, useState } from 'react';
import '../style/Global.css'
import { AuthenticationUrl } from '../../BaseUrl'
import { useNavigate } from "react-router-dom";
import axios from 'axios';
import Alert from '@mui/material/Alert'
import Collapse from '@mui/material/Collapse';
import './Register.css'
import Select from 'react-select'

const RegisterWithClanTag = () => {

    let navigate = useNavigate();

    const [error, showError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [Message, showMessage] = useState(false);
    const [clanTags, setClanTags] = useState([]);
    const [loading, setLoading] = useState(true);
    const [register, setRegister] = useState(false);

    const handleDropdownChange = (event) => {
        setClanTag(event.value)
    };

    const options = clanTags.map((item) => ({
        value: item.clanTag,
        label: item.name,
    }));

    useEffect(() => {
        var config = {
            method: 'get',
            url: AuthenticationUrl + 'GetClanTags',
            headers: {
                'Content-Type': 'application/json',
            },
        };
        axios(config).then((response) => {
            setClanTags(response.data.result)
            setLoading(false)
            console.log(response.data);
        }).catch(function (error) {
            console.log(error);
        });
    }, [])

    const [formValue, setFormValue] = useState({
        clantag: "",
        email: "",
        username: "",
        password: "",
    });

    const onChange = event => {
        setFormValue({
            ...formValue,
            [event.target.name]: event.target.value
        })
    }
    
    const setClanTag = (value) => {

        const tag = value;
        const parameter = value.slice(1);

        var config = {
            method: 'post',
            url: AuthenticationUrl + 'registerwithclantag?clantag=%23' + parameter,
            headers: {
                'Content-Type': 'application/json',
            }
        };

        axios(config).then((response) => {
            setFormValue(({
                ...formValue,
                "clantag" : tag
            }))
            setRegister(true);
            showError(false)
        }).catch(function (error) {
            console.log(error.response)
            setErrorMessage(error.message + " - " + error.response.data)
            showError(true)
        });     
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        
        var data = JSON.stringify({
            "clanTag": formValue.clantag,
            "email": formValue.email,
            "password": formValue.password,
        })

        var config = {
            method: 'post',
            url: AuthenticationUrl + 'registerUser' ,
            headers: {
                'Content-Type': 'application/json',
            },
            data: data
        };

        axios(config).then((response) => {
            console.log(response.message)
            navigate("/")
        }).catch(function (error) {
            console.log(error.response)
            setErrorMessage(error.message + " - " + error.response.data)
            showError(true)
        });     
    }

    if (!register) {
        return (
            <>
                <br></br>
                <div class="wrapperRegister">
                    <div className='loginform-container'>
                        <h2>Please select your Clash Royale user name</h2>
                        <br></br>
                        <br></br>
                        <Select
                            options={options}
                            onChange={handleDropdownChange}
                            placeholder={loading ? 'Loading...' : 'Select an option'}
                            isDisabled={loading}
                            isSearchable={true}
                            styles={{ textAlign: 'center', display: "flex", justifyContent: "center"}}
                        />
                        <br></br>

                        <Collapse in={error}>
                            <Alert show={error} severity="error"
                            >Oops, something went wrong! - {errorMessage}</Alert>
                        </Collapse>
                    </div>
                </div >
            </>
        );
    }
    else {
        return (
            <>
                <br></br>
                <div class="wrapperRegister">
                    <div className='loginform-container'></div>
                    <h2>Register</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label htmlFor="name">Clantag</label>
                            <input
                                type="text"
                                id="clantag"
                                name="clantag"
                                value={formValue.clantag}
                                onChange={onChange}
                                readOnly={true}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label>Email</label>
                            <input
                                type="Email"
                                name="email"
                                value={formValue.email}
                                onChange={onChange}
                                required
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="confirmPassword">Password</label>
                            <input
                                type="password"
                                name="password"
                                value={formValue.password}
                                onChange={onChange}
                                required
                            />
                        </div>

                        <button type="submit">Register</button>
                    </form>
                    <br></br>
                    <Collapse in={Message}>
                            <Alert style={{ textAlign: 'center' }} show={Message} severity="success"
                            >Preferences Saved!</Alert>
                        </Collapse>
                        <Collapse in={error}>
                            <Alert show={error} severity="error"
                            >Oops, something went wrong! - {errorMessage}</Alert>
                        </Collapse>
                </div>
            </>
        );
    }

}

export default RegisterWithClanTag;