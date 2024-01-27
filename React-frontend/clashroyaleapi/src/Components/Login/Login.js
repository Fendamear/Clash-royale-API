import React, { useState } from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import axios from "axios";
//import Cookies from "universal-cookie";
import Form from 'react-bootstrap/Form'
import Button from '@mui/material/Button';
import './login.css'
import Alert from '@mui/material/Alert'
import { AuthenticationUrl, BaseUrl } from '../../BaseUrl';
import Collapse from '@mui/material/Collapse';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';

export default function Login() {

    let navigate = useNavigate();

    const [error, showError] = useState(false);
    const [errorMessage, setMessage] = useState([]);
    const antd = window.antd;


    const [formValue, setFormValue] = useState({
        email: "",
        password: ""
    });

    const onChange = event => {
        setFormValue({
            ...formValue,
            [event.target.name]: event.target.value
        })
        console.log(formValue)
    }

    const login = (e) => {
        e.preventDefault();
        var data = JSON.stringify({
            "email": formValue.email,
            "password": formValue.password
        });

        var config = {
            method: 'post',
            url: AuthenticationUrl + 'login',
            headers: {
                'Content-Type': 'application/json'
            },
            data: data
        };
        axios(config).then((response) => {
            localStorage.clear("accessToken");
            //console.log("localstorage accessToken: " + localStorage.getItem("accessToken"))
            localStorage.setItem("accessToken", response.data.token)
            //console.log("localstorage accessToken after: " + localStorage.getItem("accessToken"))
            navigate("/");
        }).catch(function (error) {
            showError(true);
            setMessage(error.response.data)
        });
    }

    return (
        <>
            <div className="container">
                <div className="form-box">
                    <div className="header-form">
                        <h4 className="text-primary text-center"><i className="fa fa-user-circle" style={{ fontSize: "110px" }}></i></h4>
                        <div className="image">
                        </div>
                    </div>
                    <h2>Login</h2>
                    <div className="body-form">
                        <form onSubmit={login}>
                            <div className="input-group mb-3">
                                <div className="input-group-prepend">
                                    <span className="input-group-text"><i class="fa fa-user"></i></span>
                                </div>
                                <input type="email" className="form-control" onChange={onChange} placeholder="Email" name="email" />
                            </div>
                            <div className="input-group mb-3">
                                <div className="input-group-prepend">
                                    <span className="input-group-text"><i class="fa fa-lock"></i></span>
                                </div>
                                <input type="password" className="form-control" onChange={onChange} placeholder="Password" name="password" />
                            </div>
                            <br></br>
                            <Collapse in={error}>
                                <Alert
                                    severity="error"
                                    action={
                                        <IconButton
                                            aria-label="close"
                                            color="inherit"
                                            size="small"
                                            onClick={() => {
                                                showError(false);
                                            }}
                                        >
                                            <CloseIcon fontSize="inherit" />
                                        </IconButton>
                                    }
                                    sx={{ mb: 2 }}
                                >
                                    {errorMessage}
                                </Alert>
                            </Collapse>
                            <button type="submit" className="btn btn-secondary btn-block">LOGIN</button>
                            <h2>Or</h2>
                            <NavLink to="/register">
                                <Button variant="danger">Create an account here</Button>{' '}
                            </NavLink>
                        </form>
                    </div>
                </div>
            </div>
        </>
    )
}
