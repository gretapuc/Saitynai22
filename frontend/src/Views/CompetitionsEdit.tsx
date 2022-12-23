import backend from "app/backend";
import { Competition } from "models/competitions";
import { Calendar } from "primereact/calendar";
import { InputText } from "primereact/inputtext";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

class State {
    name: string = "";
    description: string = "";
    rules: string = "";

    isInitialized: boolean = false;
    isNameErr: boolean = false;
    isDescriptionErr: boolean = false;
    isRulesErr: boolean = false;
    isSaveErr: boolean = false;

    resetErrors() {
        this.isNameErr = false;
        this.isDescriptionErr = false;
        this.isRulesErr = false;
        this.isSaveErr = false;
    }

    shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function CompetitionsEdit() {
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
        const link = "http://localhost:5226/api/events/" + locationParams["eventId"] + "/competitions/" + locationParams["competitionId"];
		backend.get<Competition>(
			link
		)
		.then(resp => {
			updateState(state => {

				//store data loaded
				let data = resp.data;

				state.name = data.name;
				state.description = data.description;
                state.rules = data.rules;
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

            if( state.rules.trim() === "" )
				state.isRulesErr = true;

			//errors found? abort
			if( state.isRulesErr )
				return;

			//collect entity data
			let entity = {
                name: state.name,
                description: state.description,
                rules: state.rules,
            };

			//request entity creation
			backend.put<Competition>(`http://localhost:5226/api/events/${locationParams["eventId"]}/competitions/${locationParams["competitionId"]}`, entity)
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

                    <label htmlFor="description" className="form-label" style={{color: "white"}}>Aprašymas:</label>
					<InputText 
						id="description" 
						className={"form-control " + (state.isDescriptionErr ? "is-invalid" : "")}
						value={state.description}
						onChange={(e) => update(() => state.description = e.target.value)}
						/>
					{state.isDescriptionErr && 
						<div className="invalid-feedback">Description must be non empty and non whitespace.</div>
					}

                    <label htmlFor="rules" className="form-label" style={{color: "white"}}>Taisyklės:</label>
					<InputText 
						id="lastName" 
						className={"form-control " + (state.isRulesErr ? "is-invalid" : "")}
						value={state.rules}
						onChange={(e) => update(() => state.rules = e.target.value)}
						/>
					{state.isDescriptionErr && 
						<div className="invalid-feedback">Rules must be non empty and non whitespace.</div>
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

export default CompetitionsEdit;