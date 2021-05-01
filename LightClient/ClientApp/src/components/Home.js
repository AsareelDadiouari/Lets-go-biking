import React, { Component } from 'react';
import axios from 'axios'
import {Button} from "reactstrap";

export class Home extends Component {
    static displayName = Home.name;

    constructor(props, context) {
        super(props, context);

        this.state = {
            departure: "",
            arrival: "",
            departPosition: null,
            departAdress: "",
            geoJson: {}
        }

        this.show = false;
        this.url = ""

        // Bind `this` context to functions of the class
        this.search = this.search.bind(this)
        this.handleDeparture = this.handleDeparture.bind(this)
        this.handleArrival = this.handleArrival.bind(this)
    }
    
    componentDidMount() {
         navigator.geolocation.getCurrentPosition(
            position => {
                this.setState({
                    departPosition: position
                })
                
                const url = "https://api-adresse.data.gouv.fr/reverse/?lon=" + position.coords.longitude + "&lat=" + position.coords.latitude
                 axios.get(url).then( (result) => {
                    console.log(result)
                    this.setState({
                        departAdress: result.data.features[0].properties.label
                    })
                }).catch(err => console.log(err))
            },
            err => console.log(err)
        );
    }

    config = {
        method: "GET",
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "Accept": "application/json",
        }
    }

    manageSpaces = () => {
        if (/^\s+$/.test(this.state.departure)) {
            for (let i of this.state.departure) {
                if (i === ' ')
                    i = '+'
            }
        }

        if (/^\s+$/.test(this.state.arrival)) {
            for (let i of this.state.arrival) {
                if (i === ' ')
                    i = '+'
            }
        }
    }

    search = async (e) => {
        e.preventDefault();
        this.manageSpaces();
        if (this.state.departAdress.length === 0)
            this.url = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/rest/GeoData?start=" + this.state.departure + "&end=" + this.state.arrival
        else
            this.url = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/rest/GeoData?start=" + this.state.departAdress + "&end=" + this.state.arrival

        console.log(this.state.departure, this.state.arrival)
        await axios.get(this.url, this.config).then((response) => {
            if (response.data.bbox !== null) {
                this.props.history.push({
                    pathname: '/map',
                    state: JSON.stringify(response)
                });
            } else {
                alert("Can't find an adress")
            }
        }).catch(err => {
            alert("An error has occured");
            
        })
    }
    
    clear = () => {
        this.setState({ departure: "" })
        this.setState({ arrival: "" })
        this.setState({ departAdress: "" })
    }

    handleDeparture = (e) => {
        this.setState({ departure: e.target.value });
    };

    handleArrival = (e) => {
        this.setState({ arrival: e.target.value });
    };

  render () {
    return (
        <div>
            <div>
                <h1>Hello, world!</h1>
                <div className="row justify-content-center align-self-center container">
                    <img src="https://cdn.dribbble.com/users/2008861/screenshots/6491348/did-teen-biking-for-dribs_still_2x.gif?compress=1&resize=400x300" alt="biker" />
                    <div className="container">
                        <form>
                            <div className="form-group">
                                <label for="departure">Departure</label>
                                <input disabled={this.state.departAdress.length !== 0} type="text" value={this.state.departure} onChange={(e) => this.handleDeparture(e)} className="form-control" id="departure" aria-describedby="emailHelp" placeholder={this.state.departAdress.length === 0 ? "Enter an Arrival" : this.state.departAdress } />
                                <small id="emailHelp" className="form-text text-muted">Write the closest station name or choose auto location.</small>
                            </div>
                            <div className="form-group">
                                <label for="arrival">Arrival</label>
                                <input type="text" value={this.state.arrival} onChange={(e) => this.handleArrival(e)} className="form-control" id="arrival" aria-describedby="emailHelp" placeholder="Enter an Arrival" />
                                <small id="emailHelp" className="form-text text-muted">Arrival point.</small>
                            </div>
                        </form>
                    </div>
                    <Button type="submit" onClick={this.search} className="btn btn-success">Search</Button>
                    <td>&nbsp; &nbsp; &nbsp;</td>
                    <Button type="submit" onClick={this.clear} className="btn btn-danger">Clear</Button>
                </div>
            </div>
        </div>
    );
  }
}
