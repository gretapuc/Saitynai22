import backend from "app/backend";
import { Competition } from "models/competitions";
import { RegistrationModel } from "models/registration";
import { Calendar } from "primereact/calendar";
import { InputText } from "primereact/inputtext";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

class State {
    carNo: string = "";
    manufacturer: string = "";
    model: string = "";

    isInitialized: boolean = false;
    isCarNrErr: boolean = false;
    isManufacturerErr: boolean = false;
    isModelErr: boolean = false;
    isSaveErr: boolean = false;

    resetErrors() {
        this.isCarNrErr = false;
        this.isManufacturerErr = false;
        this.isModelErr = false;
        this.isSaveErr = false;
    }

    shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function RegistrationsEdit() {
    const [state, setState] = useState(new State());
    const navigate = useNavigate();
    const locationParams = useParams();

    let update = (updater : () => void) => {
		updater();
		setState(state.shallowClone());
	}

	let updateState = (updater : (state : State) => void) => {
		setState(state => {
			updater(state);
			return state.shallowClone();
		})
	}

    if( !state.isInitialized ) {
		//query data
        const link = "http://localhost:5226/api/events/" + locationParams["eventId"] + "/competitions/" + locationParams["competitionId"] + "/registrations/" + locationParams["registrationId"];
		backend.get<RegistrationModel>(
			link
		)
		.then(resp => {
			updateState(state => {

				//store data loaded
				let data = resp.data;

				state.carNo = data.carNo;
				state.manufacturer = data.manufacturer;
                state.model = data.model;
			})
		})

		//indicate data is loading and initialization done
		update(() => {
			state.isInitialized = true;
		});
	}

    let onSave = () => {
		update(() => {
			//reset previous errors
			state.resetErrors();

			//validate form
			if( state.carNo.trim() === "" )
				state.isCarNrErr = true;

			//errors found? abort
			if( state.isCarNrErr )
				return;

            if( state.manufacturer.trim() === "" )
				state.isManufacturerErr = true;

			//errors found? abort
			if( state.isManufacturerErr )
				return;

            if( state.model.trim() === "" )
				state.isModelErr = true;

			//errors found? abort
			if( state.isModelErr )
				return;

			//collect entity data
			let entity = {
                carNo: state.carNo,
                manufacturer: state.manufacturer,
                model: state.model,
            };

			//request entity creation
			backend.put<RegistrationModel>(`http://localhost:5226/api/events/${locationParams["eventId"]}/competitions/${locationParams["competitionId"]}/registrations/${locationParams["registrationId"]}`, entity)
			//success
			.then(resp => {
				//redirect back to entity list on success
				navigate("./../../", { state : "refresh" });
			})
			//failure
			.catch(err => {
				updateState(state => state.isSaveErr = true);
			});
		});
    }

        let html = 
		<>
		<div className="d-flex flex-column h-100 overflow-auto">
			<div className="d-flex justify-content-center">
				<div className="d-flex flex-column align-items-start" style={{width: "80ch"}}>					
					{state.isSaveErr &&
						<div 
							className="alert alert-warning w-100"
							>Saving failed due to backend failure. Please, wait a little and retry.</div>
					}	
                    
					<label htmlFor="name" className="form-label" style={{color: "white"}}>Valstybiniai numeriai:</label>
					<InputText 
						id="name" 
						className={"form-control " + (state.isCarNrErr ? "is-invalid" : "")}
						value={state.carNo}
						onChange={(e) => update(() => state.carNo = e.target.value)}
						/>
					{state.isCarNrErr && 
						<div className="invalid-feedback" >Car number must be non empty and non whitespace.</div>
					}

                    <label htmlFor="description" className="form-label" style={{color: "white"}}>Gamintojas:</label>
					<InputText 
						id="description" 
						className={"form-control " + (state.isManufacturerErr ? "is-invalid" : "")}
						value={state.manufacturer}
						onChange={(e) => update(() => state.manufacturer = e.target.value)}
						/>
					{state.isManufacturerErr && 
						<div className="invalid-feedback">Manufacturer must be non empty and non whitespace.</div>
					}

                    <label htmlFor="rules" className="form-label" style={{color: "white"}}>Modelis:</label>
					<InputText 
						id="lastName" 
						className={"form-control " + (state.isModelErr ? "is-invalid" : "")}
						value={state.model}
						onChange={(e) => update(() => state.model = e.target.value)}
						/>
					{state.isModelErr && 
						<div className="invalid-feedback">Model must be non empty and non whitespace.</div>
					}
				</div>
			</div>

			<div className="d-flex justify-content-center align-items-center w-100 mt-1">
				<button
					type="button"
					className="btn btn-primary mx-1"
					onClick={() => onSave()}
					><i className="fa-solid fa-floppy-disk"></i> Išsaugoti</button>
				<button
					type="button"
					className="btn btn-primary mx-1"
					onClick={() => navigate("./../")}
					><i className="fa-solid fa-xmark"></i> Atšaukti</button>
			</div>
		</div>
		</>;

	//
	return html;
}

export default RegistrationsEdit;