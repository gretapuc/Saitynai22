import backend from "app/backend";
import { Event } from "models/events";
import { Dialog } from "primereact/dialog";
import { useState } from "react";
import { Button } from "react-bootstrap";
import Card from "react-bootstrap/Card";
import { useLocation, useNavigate, useParams } from "react-router-dom";

class State {
	isInitialized : boolean = false;
	isLoaded : boolean = false;
	isLoading : boolean = true;
    isDialogVisible: boolean = false;
	events: Event[] | null = null;
    deletedEvent: Event | null = null;
	/**
	 * Makes a shallow clone. Use this to return new state instance from state updates.
	 * @returns A shallow clone of this instance.
	 */
	shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function Events() {
    const [state, setState] = useState(new State());
    const locationParams = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    let update = (updater : (state : State) => void) => {
       updateState(state => {
           updater(state);
           return state.shallowClone();
       })
   }

   let onEdit = (id : number) => {
    navigate(`./${id}/Redaguoti/`);
    }

    let onContestantsView = (id : number) => {
        navigate(`./${id}/Rungtys`);
        }

        
    let onDelete = (event: Event) => {
        update(() => {
            //send delete request to backend
            state.deletedEvent = event;
            state.isDialogVisible = true;
        });
        }
    let onConfirmedDelete = (id: number) => {
        update(() => {
            //send delete request to backend
            const link = "http://localhost:5226/api/events/" + id;
            backend.delete(link)
            //success
            .then(resp => {
                //force reloading of entity list
                update(() => {
                    update(() => {
                        state.isDialogVisible = false;
                    });
                    update(() => {
                        location.state = "refresh";
                    });
                });
    
            })
            //failure
            .catch(err => {
                //notify about operation failure
                let msg = 
                    `Deletion of entity '${id}' has failed. ` +
                    `either entity is not deletable or there was backend failure.`;
            })
        });
    }
   let updateState = (updater : (state : State) => void) => {
       setState(state => {
           updater(state);
           return state.shallowClone();
       })
   }

   if( !state.isInitialized || location.state === "refresh") {
       backend.get<Event[]>("http://localhost:5226/api/events/")
       .then(resp => {
           update(state =>{
               state.isLoading = false;
               state.isLoaded = true;
               state.events = resp.data;
           })
       });

       location.state = null;

       update(state => {
           state.isLoading = true;
           state.isLoaded = false;
           state.isInitialized = true;
       })
   }

   let html =
    <div className="content">
				{state.isLoaded && <>
				<h2>Renginiai</h2>
                {/*<button
					type="button"
					className="btn btn-primary mx-1"
					onClick={() => navigate("./Kurti/")}
					><i className="fa-solid"></i> Kurti naują renginį</button>*/}
                    <div className='row'>
                    {state.events?.map(event => 
                        <div>
                            <Card
                            bg="light"
                            text="dark"
                            onClick={() => onContestantsView(event.id)}
                            >
                                <Card.Body>
                                    <Card.Title>
                                        {event.name}
                                    </Card.Title>
                                    <Card.Text>
                                        Aprašymas: {event.description}
                                    </Card.Text>
                                    <Card.Text>
                                        Data: {event.date.toString()}
                                    </Card.Text>
                                    {/* <Button 
                                    variant="primary"
                                    onClick={() => onEdit(event.id)}>Redaguoti</Button>
                                    <Button 
                                    variant="danger"
                                    onClick={() => onDelete(event)}>Trinti</Button>*/}
                                </Card.Body>
                            </Card>
                            
                        </div>
                        )}
                    </div>
                        <Dialog
                        visible={state.isDialogVisible}
                        onHide={() => update(() => state.isDialogVisible = false)}
                        header={<span className="me-2">Ar tikrai norite ištrinti</span>}
                        style={{width: "50ch"}}
                        >
                            <div>
                                <h3>{state.deletedEvent?.name}</h3>                                        
                            </div>
                            <Button 
                            variant="danger"
                            onClick={() => onConfirmedDelete(state.deletedEvent?.id ?? -1)}>Trinti</Button>
                            <button
                            type="button"
                            className="btn btn-primary"
                            onClick={() => update(() => state.isDialogVisible = false)}
                            >Atšaukti</button>
                        </Dialog>
				</>}	
			</div>
        
        return html;
}

export default Events;