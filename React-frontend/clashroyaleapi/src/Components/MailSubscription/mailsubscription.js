import React, { useEffect, useState } from 'react';
import { MailSubscription } from './MailSubscriptionData'
import { Link, NavLink, useNavigate } from "react-router-dom";
import './MailSubscription.css';
import '../style/Global.css'
import { MailPreferenceUrl } from '../../BaseUrl'
import { Button } from 'react-bootstrap';
import axios from 'axios';
import Alert from '@mui/material/Alert'
import Collapse from '@mui/material/Collapse';
 
const MailSettingsMatrix = () => {

    let navigate = useNavigate();

    useEffect(() => {
        console.log(localStorage.getItem("accessToken"))


        var config = {
            method: 'get',
            url: MailPreferenceUrl + 'GetMailSubscription',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'bearer ' + localStorage.getItem("accessToken")
            },
        };
        axios(config).then((response) => {
            setMailSettings(response.data)
        }).catch(function (error) {
            console.log(error)
            console.log(error.message)
            if (error.message === "Network Error")
            {
                navigate("/login")
            }
        });
    }, [])

    const [mailSettings, setMailSettings] = useState([]);
    const [Message, showMessage] = useState(false);
    const [error, showError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const uniqueMailTypes = [...new Set(mailSettings.map(setting => setting.mailType))];
    const uniqueSchedulerTimes = [...new Set(mailSettings.map(setting => setting.schedulerTime))];

    const handleToggle = (mailType, schedulerTime) => {
        const updatedMailSettings = [...mailSettings];
        const setting = updatedMailSettings.find(s => s.mailType === mailType && s.schedulerTime === schedulerTime);
        setting.isEnabled = !setting.isEnabled;
        setMailSettings(updatedMailSettings);
    }

    const handleSubmit = (e) => {
        // Send the updated data to your server or perform any necessary actions.
        e.preventDefault();
        var config = {
            method: 'post',
            url: MailPreferenceUrl + 'UpdateMailSubscriptions',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'bearer ' + localStorage.getItem("accessToken")
            },
            data: mailSettings
        };

        axios(config).then((response) => {
            showError(false)
            showMessage(true);
            console.log(response);
        }).catch(function (error) {
            showMessage(false);
            setErrorMessage(error.message + " - " + error.response.data);
            showError(true);
            console.log(error.response.status);
        });

        console.log("Updated Mail Settings:", mailSettings);
    }

    return (
        <>
            <br></br>
            <div class="wrapperMailSubscription">
                <div className='loginform-container'>
                    <h2>Change your Mail Subscription Preferences</h2>
                    <br></br>
                    <form onSubmit={handleSubmit} className='loginform-container'>
                        <table id="customers">
                            <thead>
                                <tr>
                                    <th></th>
                                    {uniqueMailTypes.map((mailType, index) => (
                                        <th key={index}>{mailType}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {uniqueSchedulerTimes.map((schedulerTime, rowIndex) => (
                                    <tr key={rowIndex}>
                                        <td>{schedulerTime}</td>
                                        {uniqueMailTypes.map((mailType, colIndex) => (
                                            <td key={colIndex}>
                                                <div className="form-check">
                                                    <input
                                                        type="checkbox"
                                                        className="form-check-input"
                                                        checked={mailSettings.find(s => s.mailType === mailType && s.schedulerTime === schedulerTime).isEnabled}
                                                        onChange={() => handleToggle(mailType, schedulerTime)}
                                                    />
                                                </div>
                                            </td>
                                        ))}
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        <br></br>
                        <Collapse in={Message}>
                            <Alert style={{textAlign:'center'}} show={Message} severity="success"
                            >Preferences Saved!</Alert>
                        </Collapse>
                        <Collapse in={error}>
                            <Alert show={error} severity="error"
                            >Oops, something went wrong! - {errorMessage}</Alert>
                        </Collapse>
                        <button type="submit" class='btn btn-primary'>Submit</button>
                    </form>
                </div>
            </div >
        </>
    );
}

export default MailSettingsMatrix;