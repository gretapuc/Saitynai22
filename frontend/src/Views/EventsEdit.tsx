import backend from "app/backend";
import { Calendar } from "primereact/calendar";
import { InputText } from "primereact/inputtext";
import { useState } from "react";
import { Event } from "models/events"; 
import { useNavigate, useParams } from "react-router-dom";

class State {
    name: string = "";
    description: string = "";
    date: Date = new Date(Date.now());

    isInitialized: boolean = false;
    isNameErr: boolean = false;
    isDescriptionErr: boolean = false;
    isSaveErr: boolean = false;

    resetErrors() {
        this.isNameErr = false;
        this.isDescriptionErr = false;
        this.isSaveErr = false;
    }

    shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function EventsEdit() {
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
        const link = "http://localhost:5226/api/events/" + locationParams["id"];
		backend.get<Event>(
			link
		)
		.then(resp => {
			updateState(state => {

				//store data loaded
				let data = resp.data;

				state.name = data.name;
				state.description = data.description;
                state.date = new Date(data.date);
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
			if( state.name.trim() === "" )
				state.isNameErr = true;

			//errors found? abort
			if( state.isNameErr )
				return;

            if( state.description.trim() === "" )
				state.isDescriptionErr = true;

			//errors found? abort
			if( state.isDescriptionErr )
				return;

			//collect entity data
			let entity = {
                name: state.name,
                description: state.description,
                date: state.date.toISOString
            };

			//request entity creation
			backend.put<Event>("http://localhost:5226/api/events/" + locationParams["id"], entity)
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
                    
					<label htmlFor="name" className="form-label" style={{color: "white"}}>Pavadinimas:</label>
					<InputText 
						id="name" 
						className={"form-control " + (state.isNameErr ? "is-invalid" : "")}
						value={state.name}
						onChange={(e) => update(() => state.name = e.target.value)}
						/>
					{state.isNameErr && 
						<div className="invalid-feedback" >Name must be non empty and non whitespace.</div>
					}

                    <label htmlFor="lastName" className="form-label" style={{color: "white"}}>Aprašymas:</label>
					<InputText 
						id="lastName" 
						className={"form-control " + (state.isDescriptionErr ? "is-invalid" : "")}
						value={state.description}
						onChange={(e) => update(() => state.description = e.target.value)}
						/>
					{state.isDescriptionErr && 
						<div className="invalid-feedback">Description must be non empty and non whitespace.</div>
					}

					<label htmlFor="date" className="form-label" style={{color: "white"}}>Data:</label>
					<Calendar
						id="date"
						className="form-control"
						value={state.date}		
						onChange={(e) => update(() => state.date = e.value as Date)}				
						dateFormat="yy-mm-dd"
						/>
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
					onClick={() => navigate("./../../")}
					><i className="fa-solid fa-xmark"></i> Atšaukti</button>
			</div>
		</div>
		</>;

	//
	return html;
}

export default EventsEdit;