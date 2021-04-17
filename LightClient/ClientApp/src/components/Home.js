import React, { Component } from 'react';
import axios from 'axios'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props, context) {
        super(props, context);

        this.state = {
            departure: "",
            arrival: "",
            geoJson: {}
        }

        this.show = false;
        this.url = ""

        // Bind `this` context to functions of the class
        this.search = this.search.bind(this)
        this.handleDeparture = this.handleDeparture.bind(this)
        this.handleArrival = this.handleArrival.bind(this)
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
        this.url = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/rest/GeoData?start=" + this.state.departure + "&end=" + this.state.arrival

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
            return;
        })
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
                <div className="row justify-content-center align-self-center">
                    <img src="https://cdn.dribbble.com/users/2008861/screenshots/6491348/did-teen-biking-for-dribs_still_2x.gif?compress=1&resize=400x300" alt="biker image" />
                    <div className="container">
                        <form>
                            <div className="form-group">
                                <label for="departure">Departure</label>
                                <input type="text" value={this.state.departure} onChange={(e) => this.handleDeparture(e)} class="form-control" id="departure" aria-describedby="emailHelp" placeholder="Enter a Departure" />
                                <small id="emailHelp" className="form-text text-muted">Write the closest station name or choose auto location.</small>
                            </div>
                            <div className="form-group">
                                <label for="arrival">Arrival</label>
                                <input type="text" value={this.state.arrival} onChange={(e) => this.handleArrival(e)} class="form-control" id="arrival" aria-describedby="emailHelp" placeholder="Enter an Arrival" />
                                <small id="emailHelp" className="form-text text-muted">Arrival point.</small>
                            </div>
                        </form>
                    </div>
                    <button type="submit" onClick={this.search} className="btn btn-primary">Search</button>
                </div>
            </div>
        </div>
    );
  }
}
