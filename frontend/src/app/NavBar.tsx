import { mdiMusicNoteOutline } from "@mdi/js";
import Icon from "@mdi/react";
import Auth from "auth/Auth";
import { NavLink } from "react-router-dom";

function NavBar() {
    let html = 
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <NavLink className="navbar-brand" to="/">Illegal Sound Mafia</NavLink>
        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
            <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
            <div className="navbar-nav">
                <NavLink className="nav-item nav-link" to="/Renginiai"><Icon path={mdiMusicNoteOutline} size={1} color="white" />Renginiai<span className="sr-only">(current)</span></NavLink>
                <Auth />
            </div>
        </div>
    </nav>

    return html;
}

export default NavBar;