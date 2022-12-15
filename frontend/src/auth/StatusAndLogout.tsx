import axios from 'axios';

import appState from 'app/appState';
import backend, { replaceBackend } from 'app/backend';


/**
 * Log-out section in nav bar. React component.
 * @returns Component HTML.
 */
function StatusAndLogOut() {
	/**
	 * Handles 'Log-out' command.
	 */
	let onLogOut = () => {
		//send log-out request to the backend
		backend.get(
			"https://localhost:7112/api/auth/logout",
			{
				params : {					
				}
			}
		)
		//logout ok
		.then(resp => {		
			//forget user information and JWT
			appState.userId = "";
			appState.userTitle = "";
			appState.authJwt = "";

			//replace backend connector with axios instance having default configuration
			let defaultBackend = axios.create();
			replaceBackend(defaultBackend);

			//indicate user is logged out
			appState.isLoggedIn.value = false;
		})
		//login failed or backend error, show error message
		.catch(err => {
			//TODO: show some kind of error dialog? assume user is logged out anyway?
		});
	}

	//render component html
	let html = 
		<>
		<span className="d-flex align-items-center">
			<span style= {{color: 'white'}}>Welcome, {appState.userTitle}</span>
			<button 
				type="button"
				className="btn btn-primary btn-sm ms-2" 
				onClick={() => onLogOut()}
				>Log out</button>
		</span>
		</>;

	//
	return html;
}

//
export default StatusAndLogOut;