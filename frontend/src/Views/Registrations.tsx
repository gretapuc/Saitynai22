import backend from "app/backend";
import { Competition } from "models/competitions";
import { RegistrationModel } from "models/registration";
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
	registrations: RegistrationModel[] | null = null;
    deletedRegistration: RegistrationModel | null = null;
	/**
	 * Makes a shallow clone. Use this to return new state instance from state updates.
	 * @returns A shallow clone of this instance.
	 */
	shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function Registrations() {
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
        navigate(`./${id}`);
        }

        
    let onDelete = (registration: RegistrationModel) => {
        update(() => {
            //send delete request to backend
            state.deletedRegistration = registration;
            state.isDialogVisible = true;
        });
        }
    let onConfirmedDelete = (id: number) => {
        update(() => {
            //send delete request to backend
            const link = "http://localhost:5226/api/events/" + locationParams["eventId"] + "/competitions/" + locationParams["competitionId"] + "/registrations/" + id;
            backend.delete(link)
            //success
            .then(resp => {
                update(() => {
                    update(() => state.isDialogVisible = false);
                });
                update(() => {
                    location.state = "refresh";
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
       backend.get<RegistrationModel[]>("http://localhost:5226/api/events/" + locationParams["eventId"] + "/competitions/" + locationParams["competitionId"] + "/registrations/")
       .then(resp => {
           update(state =>{
               state.isLoading = false;
               state.isLoaded = true;
               state.registrations = resp.data;
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
				<h2>Dalyviai</h2>
                <button
					type="button"
					className="btn btn-primary mx-1"
					onClick={() => navigate("./Kurti/")}
					><i className="fa-solid"></i> Registruoti dalyvį</button>
                    {state.registrations?.map(registration => 
                        <div>
                            <Card
                            bg="light"
                            text="dark"
                            >
                                <Card.Body>
                                    <Card.Title>
                                        {registration.carNo}
                                    </Card.Title>
                                    <Card.Text>
                                        Markė: {registration.manufacturer}
                                    </Card.Text>
                                    <Card.Text>
                                        Modelis: {registration.model}
                                    </Card.Text>
                                    <Button 
                                    variant="primary"
                                    onClick={() => onEdit(registration.id)}>Redaguoti</Button>
                                    <Button 
                                    variant="danger"
                                    onClick={() => onDelete(registration)}>Trinti</Button>
                                </Card.Body>
                            </Card>
                            
                        </div>
                        )}
                        <Dialog
                        visible={state.isDialogVisible}
                        onHide={() => update(() => state.isDialogVisible = false)}
                        header={<span className="me-2">Ar tikrai norite ištrinti</span>}
                        style={{width: "50ch"}}
                        >
                            <div>
                                <h3>{state.deletedRegistration?.carNo}</h3>                                        
                            </div>
                            <Button 
                            variant="danger"
                            onClick={() => onConfirmedDelete(state.deletedRegistration?.id ?? -1)}>Trinti</Button>
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

export default Registrations;