import cover from "../assets/cover.jpg"
function About() {
    let html =
    <div>
        <div className="cover">
	    	<img src={cover} alt="Cover"/>
	    </div>
	    <div className="caption">
	        <h1>Automobilių garso aparatūrų varžybų IS</h1>
	    </div>
    </div>
    return html;
}

export default About;