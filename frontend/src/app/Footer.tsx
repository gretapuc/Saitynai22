function Footer() {
    let html =
    <footer className="bg-dark text-center text-lg-start" style={{color: "white"}}>
        <div className="container p-4">
          <div className="row">
                <div className="col-lg-6 col-md-12 mb-4 mb-md-0">
                  <h5 className="text-uppercase">Informacija</h5>
                  <p>
                    Lorem ipsum dolor sit amet consectetur, adipisicing elit. Iste atque ea quis
                    molestias. Fugiat pariatur maxime quis culpa corporis vitae repudiandae
                    aliquam voluptatem veniam, est atque cumque eum delectus sint!
                  </p>
                </div>
                <div className="col-lg-6 col-md-12 mb-4 mb-md-0">
                  <h5 className="text-uppercase">Kontaktai</h5>
                  <p>
                    Lorem ipsum dolor sit amet consectetur, adipisicing elit. Iste atque ea quis
                    molestias. Fugiat pariatur maxime quis culpa corporis vitae repudiandae
                    aliquam voluptatem veniam, est atque cumque eum delectus sint!
                  </p>
                </div>
          </div>
        </div>
        <div className="text-center p-3" style={{backgroundColor: "rgba(0, 0, 0, 0.2);"}}>
          © 2022 Copyright:
          <a href="#" className="text-dark">Illegal Sound Mafia</a>
        </div>
    </footer>

    return html;
}

export default Footer;