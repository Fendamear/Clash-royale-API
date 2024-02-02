import { useEffect, useState } from 'react';
import { BarChart } from '@mui/x-charts/BarChart';
import { testdata } from './currentriverracedata';
import Select from 'react-select'
import { sectionOptions, seasonOptions } from '../Layout/SidebarData';
import { BaseUrl, CurrentRiverRaceUrl } from '../../BaseUrl';
import axios from 'axios';

export default function BasicBars() {

    const defaultData = [  {
        "name": "",
        "fame": 0,
        "decksNotUsed": 0,
        "decksUsed": 0
      }]

    const [decksnotused, setdecksnotused] = useState(defaultData);
    const [fame, setfame] = useState(defaultData);

    const [formValue, setFormValue] = useState({
        seasonId: 0,
        sectionId: -1
    });

    useEffect(() => {
        var config = {
            method: 'get',
            url: CurrentRiverRaceUrl + "GraphData?seasonId=0&sectionId=-1&notUsed=true",
            headers: {
                'Content-Type': 'application/json',
            },
        };
        axios(config).then((response) => {
            setdecksnotused(response.data);
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });

        var config = {
            method: 'get',
            url: CurrentRiverRaceUrl + "GraphData?seasonId=0&sectionId=-1&notUsed=false",
            headers: {
                'Content-Type': 'application/json',
            },
        };
        axios(config).then((response) => {
            console.log(response.data)
            setfame(response.data);
        }).catch(function (error) {
            console.log(error)
        });
    }, [])

    const handleSubmit = (e) => {
        e.preventDefault();

        var config = {
            method: 'get',
            url: CurrentRiverRaceUrl + "GraphData?seasonId=" + formValue.seasonId + "&sectionId=" + formValue.sectionId + "&notUsed=true",
            headers: {
                'Content-Type': 'application/json',
            },
        };

        // axios(config).then((response) => {
        //     console.log(response.data)
        //     setdecksnotused(defaultData)
        //     if (response.data.length == 0)
        //     {
        //         console.log("test")
        //         setdecksnotused(defaultData)
        //         console.log(decksnotused)
        //     }
        //     else
        //     {
        //         setdecksnotused(response.data);
        //     }
        //     console.log(decksnotused)
        // }).catch(function (error) {
        //     console.log(error)
        // });

        // var config = {
        //     method: 'get',
        //     url: CurrentRiverRaceUrl + "GraphData?seasonId=" + formValue.seasonId + "&sectionId=" + formValue.sectionId + "&notUsed=false",
        //     headers: {
        //         'Content-Type': 'application/json',
        //     },
        // };
        // axios(config).then((response) => {
        //     if (response.data.length < 0)
        //     {
        //         setdecksnotused(defaultData)
        //     }
        //     else {
        //         setfame(response.data);
        //     }
        //     console.log(fame)
        // }).catch(function (error) {
        //     console.log(error)
        // });
    }

    const handleDropdownSectionChange = (event) => {
        console.log(event.value)
        setFormValue({
            ...formValue,
            sectionId: event.value
        });
        console.log(formValue)
    };

    const handleDropdownSeasonChange = (event) => {
        setFormValue({
            ...formValue,
            seasonId: event.value
        });
        console.log(formValue)
    };

    return (
        <>
            <div class="wrapperCurrentriverrace">
                <div className='loginform-container'>
                    <form onSubmit={handleSubmit}>
                        <a>Season Id (Required)</a>
                        <Select
                            options={seasonOptions}
                            onChange={handleDropdownSeasonChange}
                            placeholder='Select an option'
                            isSearchable={true}
                            styles={{ textAlign: 'center', display: "flex", justifyContent: "center" }}
                            required
                        />
                        <br></br>
                        <a>Week Number (Optional)</a>
                        <Select
                            options={sectionOptions}
                            onChange={handleDropdownSectionChange}
                            placeholder='Select an option'
                            isSearchable={true}
                            styles={{ textAlign: 'center', display: "flex", justifyContent: "center" }}
                        />
                        <button type="submit" style={{ textAlign: 'left' }}>Send</button>
                    </form>
                </div>
            </div >
            <div class="wrapperCurrentriverrace">
                <div className='loginform-container'>
                    <div class="wrapperCurrentriverrace">
                        <div className='loginform-container'>
                            <h1>Decks Not used</h1>
                            <BarChart
                                xAxis={[{
                                    scaleType: 'band',
                                    categoryGapRatio: 0.6,
                                    barGapRatio: 0.1,
                                    data: decksnotused.map(item => item.name),
                                    tickLabelStyle: {
                                        angle: 45,
                                        textAnchor: 'start',
                                        fontSize: 12,
                                    }
                                }]}
                                series={[{ data: decksnotused.map(item => item.decksNotUsed) }]}
                                width={1200}
                                height={300}
                            />
                        </div>
                    </div>
                    <div class="wrapperCurrentriverrace">
                        <div className='loginform-container'>
                            <h1>Fame</h1>
                            <BarChart
                                xAxis={[{
                                    scaleType: 'band',
                                    categoryGapRatio: 0.6,
                                    barGapRatio: 0.1,
                                    data: fame.map(item => item.name),
                                    tickLabelStyle: {
                                        angle: 90,
                                        textAnchor: 'start',
                                        fontSize: 12,
                                        height: 10
                                    }
                                }]}
                                series={[{ data: fame.map(item => item.fame) }]}
                                width={1200}
                                height={400}                          
                            />
                            <br></br>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}