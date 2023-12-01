import React, { useState, useEffect } from "react";
import { DragDropContext, Draggable, Droppable } from "@hello-pangea/dnd";
import axios from "axios";
import './CallList.css'
import { CallListUrl } from "../../BaseUrl";
import Button from 'react-bootstrap/Button'
import Alert from '@mui/material/Alert'
import Collapse from '@mui/material/Collapse';

function CallList() {
    const [columns, setColumns] = useState([]);
    const [Message, showMessage] = useState(false);
    const [error, showError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        var config = {
            method: 'get',
            url: CallListUrl,
            headers: {
                'Content-Type': 'application/json',
            },
            //data: data
        };
        axios(config).then((response) => {
            setColumns(response.data);
            console.log(response.data);
        }).catch(function (error) {
            console.log(error)
            setErrorMessage(error.message + " - " + error.response.data)
            showError(true);
        });
    }, [])

    const handleSubmit = (e) => {
        e.preventDefault();

        const jsonarray = Object.keys(columns).map((key) => columns[key]);
        var data = JSON.stringify(jsonarray)
        var config = {
            method: 'post',
            url: CallListUrl,
            headers: {
                'Content-Type': 'application/json',
            },
            data: data
        };

        console.log(data);
        axios(config).then((response) => {
            showError(false)
            showMessage(true);
        }).catch(function (error) {
            console.log(error)
            setErrorMessage(error.message + " - " + error.response.data)
            showError(true);
        });
    }

    const onDragEnd = (result, columns, setColumns) => {
        if (!result.destination) return;

        console.log(result)


        const sourceColumn = columns[result.source.droppableId];
        const destinationColumn = columns[result.destination.droppableId];

        if (sourceColumn === destinationColumn) {
            // If the source and destination columns are the same
            const newItems = Array.from(sourceColumn.members);
            const [movedItem] = newItems.splice(result.source.index, 1);
            newItems.splice(result.destination.index, 0, movedItem);

            const newColumn = {
                ...sourceColumn,
                members: newItems,
            };

            setColumns((prevColumns) => ({
                ...prevColumns,
                [result.source.droppableId]: newColumn,
            }));
        } else {
            // If moving between different columns
            const sourceItems = Array.from(sourceColumn.members);
            const destinationItems = Array.from(destinationColumn.members);

            const [movedItem] = sourceItems.splice(result.source.index, 1);
            destinationItems.splice(result.destination.index, 0, movedItem);

            const newColumns = {
                ...columns,
                [result.source.droppableId]: {
                    ...sourceColumn,
                    members: sourceItems,
                },
                [result.destination.droppableId]: {
                    ...destinationColumn,
                    members: destinationItems,
                },
            };

            setColumns(newColumns);
            console.log(newColumns);
        }
    };

    return (
        <div style={{ paddingLeft: "50px" }}>
            <h1 style={{ textAlign: "center" }}>Call list</h1>
            <br></br>
            <Button onClick={handleSubmit}> Save</Button>
            <br></br>
            <Collapse in={Message}>
                <Alert style={{ textAlign: 'center', display: "flex", justifyContent: "center"}} show={Message} severity="success"
                >Preferences Saved!</Alert>
            </Collapse>
            <Collapse in={error}>
                <Alert style={{ textAlign: 'center', display: "flex", justifyContent: "center"}} show={error} severity="error"
                >Oops, something went wrong! - {errorMessage}</Alert>
            </Collapse>
            <div
                style={{
                    display: "flex",
                    flexDirection: "row",
                    flexWrap: "wrap",
                    alignItems: "center",
                    margin: "auto",
                    marginTop: "50px",
                    marginRight: "5px",
                    border: "5px solid #ddd",
                    borderRadius: "15px",
                    padding: "10px"
                }}
            >
                <DragDropContext
                    onDragEnd={(result) => onDragEnd(result, columns, setColumns)}
                >
                    {Object.entries(columns).map(([columnId, column], index) => (
                        <React.Fragment key={columnId}>
                            {(index > 0 && index % 4 === 0) && <div style={{ width: "100%" }} />}
                            <div
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    marginBottom: "20px",
                                    paddingLeft: "200px",
                                    width: "calc(25% - 100px)" // Adjust the width to fit 4 columns per row
                                }}
                            >
                                <h2>{column.coLeader}</h2>
                                <div style={{ margin: 12 }}>
                                    <Droppable droppableId={columnId} key={columnId}>
                                        {(provided, snapshot) => (
                                            <div
                                                {...provided.droppableProps}
                                                ref={provided.innerRef}
                                                style={{
                                                    background: snapshot.isDraggingOver
                                                        ? "lightblue"
                                                        : "lightgrey",
                                                    padding: 6,
                                                    width: 250,
                                                    minHeight: 420,
                                                }}
                                            >
                                                {column.members.map((member, itemIndex) => (
                                                    <Draggable
                                                        key={member.clanTag}
                                                        draggableId={member.clanTag}
                                                        index={itemIndex}
                                                    >
                                                        {(provided, snapshot) => (
                                                            <div
                                                                ref={provided.innerRef}
                                                                {...provided.draggableProps}
                                                                {...provided.dragHandleProps}
                                                                style={{
                                                                    userSelect: "none",
                                                                    padding: 4,
                                                                    margin: "0 0 6px 0",
                                                                    minHeight: "20px",
                                                                    maxHeight: "50px",
                                                                    backgroundColor: snapshot.isDragging
                                                                        ? "#263B4A"
                                                                        : "#456C86",
                                                                    color: "white",
                                                                    ...provided.draggableProps.style,
                                                                    textAlign: "center"
                                                                }}
                                                            >
                                                                {member.clanName}
                                                            </div>
                                                        )}
                                                    </Draggable>
                                                ))}
                                                {provided.placeholder}
                                            </div>
                                        )}
                                    </Droppable>
                                </div>
                            </div>
                        </React.Fragment>
                    ))}
                </DragDropContext>
            </div>
        </div>
    );
}

export default CallList;
