import { useEffect, useMemo, useState } from 'react';
import {
    MaterialReactTable,
    useMaterialReactTable,
    MTableBodyRow
} from 'material-react-table';
import { Box } from '@mui/material';
import '../currentRiverRace/currentriverrace.css'
import { ClanMemberUrl } from '../../BaseUrl'
import axios from "axios";
import Alert from '@mui/material/Alert'
import Button from 'react-bootstrap/Button'

const ClanMembers = () => {

    const [data, SetData] = useState([]);
    const [LastUpdated, SetLastUpdated] = useState("");

    function formatDate(string) {
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
        }
        return new Date(string).toLocaleDateString([], options);
    }

    useEffect(() => {
        var config = {
            method: 'get',
            url: ClanMemberUrl + "GetClanMembers",
            headers: {
                'Content-Type': 'application/json',
            },
            //data: data
        };
        axios(config).then((response) => {
            SetData(response.data);
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });

        var config = {
            method: 'get',
            url: ClanMemberUrl + "GetLatestLogTime",
            headers: {
                'Content-Type': 'application/json',
            },
            //data: data
        };
        axios(config).then((response) => {
            SetLastUpdated(formatDate(response.data.time));
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });
    }, [])

    const SuperCell = (e) => {
        var config = {
            method: 'get',
            url: ClanMemberUrl + "GetClanMembersFromRemote",
            headers: {
                'Content-Type': 'application/json',
            },
            //data: data
        };
        axios(config).then((response) => {
            window.location.reload();
        }).catch(function (error) {
            console.log(error)
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
                accessorKey: 'role',
                enableColumnOrdering: false,
                header: 'role',
            },

            {
                accessorKey: 'lastSeen',
                enableColumnOrdering: false,
                header: 'Last Online',
                Cell: ({ cell }) => formatDate(cell.getValue())
            },
            {
                accessorKey: 'isActive',
                enableColumnOrdering: false,
                header: 'Active Player',
                Cell: ({ cell }) => (
                    <Box
                        component="span"
                        sx={(theme) => ({
                            backgroundColor:
                                cell.getValue() == true
                                    ? theme.palette.success.dark
                                    : theme.palette.error.dark,
                            borderRadius: '0.25rem',
                            maxWidth: '9ch',
                            p: '0.25rem',
                            color: "#FFFF"
                        })}
                    >
                        {cell.getValue() == true
                            ? "Yes"
                            : "NO"}
                    </Box>
                ),
            },
            {
                accessorKey: 'isInClan',
                enableColumnOrdering: false,
                header: 'Is Currently In Clan',
                Cell: ({ cell }) => (
                    <Box
                        component="span"
                        sx={(theme) => ({
                            backgroundColor:
                                cell.getValue() == true
                                    ? theme.palette.success.dark
                                    : theme.palette.error.dark,
                            borderRadius: '0.25rem',
                            maxWidth: '9ch',
                            p: '0.25rem',
                            color: "#FFFF"
                        })}
                    >
                        {cell.getValue() == true
                            ? "Yes"
                            : "NO"}
                    </Box>
                ),
            }
        ],
        [],
        //end
    );

    const table = useMaterialReactTable({
        columns,
        data,
        enableRowNumbers: true,
        paginateExpandedRows: false,
        filterFromLeafRows: true,
        enableColumnFilterModes: true,
        enableGrouping: true,
        enableColumnPinning: true,
        enableStickyHeader: true,
        enableStickyFooter: true,
        initialState: {
            density: 'compact', //sort by state by default
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

    return <>
        <br></br>
        <div class="wrapperCurrentriverrace">
            <div className='loginform-container'>
                <br></br>
                <MaterialReactTable table={table} />
            </div>
        </div >
        <div class="wrapperCurrentriverrace">
            <div className='loginform-container'>
                <Alert style={{ textAlign: 'center', display: "flex", justifyContent: "center" }} severity="warning"
                >warning! With the following button you can update the clan member data based on supercell data - Last Updated at {LastUpdated}</Alert>
                <br></br>
                <div style={{display: "flex", justifyContent: "center"}}>
                    <Button style={{ textAlign: 'center', display: "flex", justifyContent: "center" }} onClick={SuperCell}>
                        Get Data From Supercell
                    </Button></div>
            </div>
        </div>
    </>;
};

export default ClanMembers;
