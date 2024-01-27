import { useEffect, useMemo, useState } from 'react';
import {
    MaterialReactTable,
    useMaterialReactTable,
    MTableBodyRow
} from 'material-react-table';
import '../currentRiverRace/currentriverrace.css'
import { ClanMemberUrl, CurrentRiverRaceUrl } from '../../BaseUrl'
import axios from "axios";
import DeleteIcon from '@mui/icons-material/Delete';
import {jwtDecode} from 'jwt-decode'

import {
    Box,
    Button,
    CircularProgress,
    IconButton,
    Tooltip,
    Typography,
} from '@mui/material';

const RiverRaceLog = () => {

    const [data, SetData] = useState([]);
    const [admin, setAdmin] = useState(false);
    const role = jwtDecode(localStorage.getItem("accessToken")).Admin

    const openDeleteConfirmModal = (row) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            console.log(row.id);
            deleteLog(row.id);
        }
    };

    const deleteLog = (id) => {
        var config = {
            method: 'delete',
            url: CurrentRiverRaceUrl + "DeleteRiverRaceLog?id=" + id,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'bearer ' + localStorage.getItem("accessToken")
            },
        };
        axios(config).then((response) => {
            console.log(response.status)
            window.location.reload();
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });
    }

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
            url: CurrentRiverRaceUrl + "GetRiverRaceLog",
            headers: {
                'Content-Type': 'application/json',
            },
            //data: data
        };
        axios(config).then((response) => {
            SetData(response.data);
            console.log(role);
            console.log(response.data)
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });
    }, [])

    const columns = useMemo(
        //column definitions...
        () => [
            {
                accessorKey: 'guid',
                enableColumnOrdering: false,
                header: 'Id',
            },
            {
                accessorKey: 'seasonId',
                enableColumnOrdering: false,
                header: 'Season ID',
            },
            {
                accessorKey: 'sectionId',
                enableColumnOrdering: false,
                header: 'Week Nr',
            },
            {
                accessorKey: 'type',
                enableColumnOrdering: false,
                header: 'Week Type',
            },
            {
                accessorKey: 'timeStamp',
                enableColumnOrdering: false,
                header: 'Time Stamp',
                Cell: ({ cell }) => formatDate(cell.getValue())
            }
        ],
        [],
        //end
    );

    const table = useMaterialReactTable({
        columns,
        data,
        createDisplayMode: 'row', // ('modal', and 'custom' are also available)
        editDisplayMode: 'table',
        enableColumnFilterModes: true,
        enableGrouping: true,
        enableColumnPinning: true,
        enableStickyHeader: true,
        enableStickyFooter: true,
        enableRowNumbers: true,
        enableRowActions: role === 'True' ? true : false,
        positionActionsColumn: 'last',
        initialState: {
            density: 'compact',
            pagination: { pageIndex: 0, pageSize: 50 },
            columnVisibility: { guid: false }
        },
        getRowId: (row) => row.guid,
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
        renderRowActions: ({ row }) => (
            <Box sx={{ display: 'flex', gap: '1rem' }}>
                <Tooltip title="Delete">
                    <IconButton color="error" onClick={() => openDeleteConfirmModal(row)}>
                        <DeleteIcon />
                    </IconButton>
                </Tooltip>
            </Box>
        ),
    });

    return <>
    <br></br>
    <div class="wrapperCurrentriverrace">
        <div className='loginform-container'>
            <br></br>
            <MaterialReactTable table={table} />
        </div>
    </div >
    </>;
    
};

export default RiverRaceLog;