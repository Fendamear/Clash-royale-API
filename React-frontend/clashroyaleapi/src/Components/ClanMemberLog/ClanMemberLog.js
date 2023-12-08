import { useMemo, useState, useEffect } from 'react';
import {
    MaterialReactTable,
    useMaterialReactTable,
    MTableBodyRow
} from 'material-react-table';
import '../currentRiverRace/currentriverrace.css'
import { ClanMemberUrl } from '../../BaseUrl';
import axios from 'axios';
import {
    Box,
    Button,
    CircularProgress,
    IconButton,
    Tooltip,
    Typography,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';

const ClanMemberLog = () => {

    const [LastUpdated, SetLastUpdated] = useState("");
    const [data, SetData] = useState([]);

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
            url: ClanMemberUrl + "GetClanMemberLog",
            headers: {
                'Content-Type': 'application/json',
            },
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
        };
        axios(config).then((response) => {
            SetLastUpdated(formatDate(response.data.time));
        }).catch(function (error) {
            console.log(error)
            //setErrorMessage(error.message + " - " + error.response.data)
            //showError(true);
        });
    }, [])

    const openDeleteConfirmModal = (row) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            console.log(row.original.guid);
            deleteLog(row.original.guid);
        }
    };

    const deleteLog = (id) => {
        var config = {
            method: 'delete',
            url: ClanMemberUrl + "DeleteClanMemberLog?guid=" + id,
            headers: {
                'Content-Type': 'application/json',
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

    const columns = useMemo(
        //column definitions...
        () => [
            {
                accessorKey: 'guid',
                enableColumnOrdering: false,
                header: 'Id',
            },
            {
                accessorKey: 'name',
                enableColumnOrdering: false,
                header: 'Name',
            },
            {
                accessorKey: 'time',
                enableColumnOrdering: false,
                header: 'Date Logged',
                Cell: ({ cell }) => formatDate(cell.getValue())
            },
            {
                accessorKey: 'status',
                enableColumnOrdering: false,
                header: 'Status',
            },
            {
                accessorKey: 'oldValue',
                enableColumnOrdering: false,
                header: 'Old Value',
            },
            {
                accessorKey: 'newValue',
                enableColumnOrdering: false,
                header: 'New Value',
            },
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
        enableRowActions: true,
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
                <h1 style={{ textAlign: 'left' }}>Last Updated at {LastUpdated} </h1>
                <br></br>
                <MaterialReactTable table={table} />
            </div>
        </div >

    </>;
};

export default ClanMemberLog;
