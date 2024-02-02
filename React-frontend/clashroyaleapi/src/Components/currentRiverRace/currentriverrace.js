import { useMemo, useState } from 'react';
import {
    MaterialReactTable,
    useMaterialReactTable,
    MTableBodyRow
} from 'material-react-table';
import { data } from './data'
import { Box } from '@mui/material';
import './currentriverrace.css'
import { CurrentRiverRaceUrl } from '../../BaseUrl';
import axios from 'axios'
import { alignProperty } from '@mui/material/styles/cssUtils';
import Select from 'react-select'
import { sectionOptions, dayOptions, seasonOptions } from '../Layout/SidebarData';

const Example = () => {

    const [data, setData] = useState([]);
    const [formValue, setFormValue] = useState({
        seasonId: 0,
        sectionId: -1,
        dayId: -1
    });

    const onChange = event => {
        setFormValue({
            ...formValue,
            [event.target.name]: event.target.value
        })
    }

    const handleSubmit = (e) => {
        e.preventDefault();

        var config = {
            method: 'get',
            url: CurrentRiverRaceUrl + 'GetCurrentriverrace?seasonId=' + formValue.seasonId + '&sectionId=' + formValue.sectionId + '&dayId=' + formValue.dayId,
            headers: {
                'Content-Type': 'application/json',
            },
        };

        axios(config).then((response) => {
            console.log(response.message)
            setData(response.data)
        }).catch(function (error) {
            console.log(error.response)
        });
    }

    const columns = useMemo(
        //column definitions...
        () => [
            {
                accessorKey: 'name',
                enableColumnOrdering: false,
                header: 'Name',
            },
            {
                accessorKey: 'dateIdentifier',
                enableColumnOrdering: false,
                header: 'Date Identifier',
            },

            {
                accessorKey: 'fame',
                enableColumnOrdering: false,
                header: 'Fame',
            },
            {
                accessorKey: 'decksUsed',
                enableColumnOrdering: false,
                header: 'Decks used',
                Cell: ({ cell }) => (
                    <Box
                        component="span"
                        sx={(theme) => ({
                            backgroundColor:
                                cell.getValue() == 0
                                    ? theme.palette.error.dark
                                    : cell.getValue() > 0 && cell.getValue() <= 2
                                        ? theme.palette.warning.dark
                                        : cell.getValue() == 16 || cell.getValue() == 64 || cell.getValue() == 80
                                            ? theme.palette.success.dark
                                            : "#FFFFF",
                            borderRadius: '0.25rem',
                            color: cell.getValue() == 0
                                ? "#FFFF"
                                : cell.getValue() > 0 && cell.getValue() <= 2
                                    ? "#FFFF"
                                    : cell.getValue() == 16 || cell.getValue() == 64 || cell.getValue() == 80
                                        ? "#FFFF"
                                        : "black",
                            maxWidth: '9ch',
                            p: '0.25rem',

                        })}
                    >
                        {cell.getValue()}
                    </Box>
                ),
            },
            {
                accessorKey: 'decksNotUsed',
                enableColumnOrdering: false,
                header: 'Decks Not Used',
                Cell: ({ cell }) => (
                    <Box
                        component="span"
                        sx={(theme) => ({
                            backgroundColor:
                                cell.getValue() == 0
                                    ? theme.palette.success.dark
                                    : cell.getValue() > 0 && cell.getValue() < 10
                                        ? theme.palette.warning.dark
                                        : theme.palette.error.dark,
                            borderRadius: '0.25rem',
                            color: '#fff',
                            maxWidth: '9ch',
                            p: '0.25rem',
                        })}
                    >
                        {cell.getValue()}
                    </Box>
                ),
            },
            {
                accessorKey: 'time',
                header: 'Time Stamp',
            },
        ],
        [],
        //end
    );

    const table = useMaterialReactTable({
        columns,
        data,
        enableExpanding: true,
        paginateExpandedRows: false,
        filterFromLeafRows: true,
        enableColumnFilterModes: true,
        enableGrouping: true,
        enableColumnPinning: true,
        enableStickyHeader: true,
        enableStickyFooter: true,
        initialState: {
            density: 'compact',
            pagination: { pageIndex: 0, pageSize: 50 },
            sorting: [{ id: 'decksNotUsed', desc: true }], //sort by state by default
        },
        muiToolbarAlertBannerChipProps: { color: 'primary' },
        muiTableContainerProps: { sx: { maxHeight: 700 } },
        muiTableBodyProps: {
            sx: {
                //stripe the rows, make odd rows a darker color
                '& tr:nth-of-type(odd) > td': {
                    backgroundColor: '#CCCCCC',
                },
                '& tr:nth-of-type(even) > td': {
                    backgroundColor: '#f5f5f5',
                },
            },
        },
    });

    const handleDropdownSectionChange = (event) => {
        console.log(event.value)
        setFormValue({
            ...formValue,
            sectionId: event.value
        });
        console.log(formValue)
    };

    const handleDropdownDayChange = (event) => {
        setFormValue({
            ...formValue,
            dayId: event.value
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

    return <>
        <br></br>
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
                    <a>Week Number</a>
                    <Select
                        options={sectionOptions}
                        onChange={handleDropdownSectionChange}
                        placeholder='Select an option'
                        isSearchable={true}
                        styles={{ textAlign: 'center', display: "flex", justifyContent: "center" }}
                    />
                    <br></br>
                    <a>Day </a>
                    <Select
                        options={dayOptions}
                        onChange={handleDropdownDayChange}
                        placeholder='Select an option'
                        isSearchable={true}
                        styles={{ textAlign: 'center', display: "flex", justifyContent: "center" }}
                    />
                    <button type="submit" style={{ textAlign: 'left' }}>Send</button>
                </form>
            </div>
        </div >
        <br></br>
        <br></br>
        <div class="wrapperCurrentriverrace">
            <div className='loginform-container'>

                <br></br>
                <MaterialReactTable table={table} />
            </div>
        </div >

    </>;
};

export default Example;
