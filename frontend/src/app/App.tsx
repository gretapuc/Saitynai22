import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'

import './App.scss';
import NavBar from './NavBar';
import Footer from './Footer';
import About from './About';
import { useRef, useState } from 'react';
import appState from './appState';
import { Toast } from 'primereact/toast';
import Events from 'Views/Events';
import EventsCreate from 'Views/EventsCreate';
import EventsEdit from 'Views/EventsEdit';
import Competitions from 'Views/Competitions';
import CompetitionsCreate from 'Views/CompetitionsCreate';
import CompetitionsEdit from 'Views/CompetitionsEdit';
import Registrations from 'Views/Registrations';
import RegistrationsCreate from 'Views/RegistrationsCreate';
import RegistrationsEdit from 'Views/RegistrationsEdit';

/**
 * Application. React component.
 * @returns Component HTML.
 */
 class State {
	isInitialized : boolean = false;

	/**
	 * Makes a shallow clone. Use this to return new state instance from state updates.
	 * @returns A shallow clone of this instance.
	 */
	shallowClone() : State {
		return Object.assign(new State(), this);
	}
}

function App() {
	const [state, setState] = useState(new State());

	const toastRef = useRef<Toast>(null);

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

	if( !state.isInitialized )
	{
		//subscribe to app state changes
		appState.when(appState.isLoggedIn, () => {
			//this will force component re-rendering
			updateState(state => {});
		});

		//subscribe to user messages
		appState.msgs.subscribe(msg => {
			update(() => toastRef.current?.show(msg));
		});

		//indicate initialization is done
		updateState(state => state.isInitialized = true);
	}

	let html =
		<Router>
			<NavBar />
			<Toast ref={toastRef} position="top-right"/>
			<Routes>
				<Route path="/" element={<About />}></Route>
				<Route path="/Renginiai" element={<Events />}></Route>
				<Route path="/Renginiai/:id/Redaguoti" element={<EventsEdit />}></Route>
				<Route path="/Renginiai/Kurti" element={<EventsCreate />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys/:competitionId/Redaguoti" element={<CompetitionsEdit />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys/Kurti" element={<CompetitionsCreate />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys" element={<Competitions />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys/:competitionId/Registracijos/:registrationId/Redaguoti" element={<RegistrationsEdit />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys/:competitionId/Registracijos/Kurti" element={<RegistrationsCreate />}></Route>
				<Route path="/Renginiai/:eventId/Rungtys/:competitionId/Registracijos" element={<Registrations />}></Route>
			</Routes>
			<Footer />
		</Router>;
	//
	return html;
}

export default App;