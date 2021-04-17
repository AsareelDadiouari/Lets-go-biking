import React, { Component } from 'react';
//import PropTypes from 'prop-types';
import { MapContainer, TileLayer, Marker, Popup, GeoJSON } from 'react-leaflet'

class Map extends Component {
    constructor(props, context) {
        super(props, context)

        const coords = JSON.parse(this.props.location.state).data;
        const coordLength = coords.length - 1;
        console.log(coords)
        const start = [coords[0].waypoints[0][1], coords[0].waypoints[0][0]]
        const end = [coords[coordLength].waypoints[coords[coordLength].waypoints.length - 1][1], coords[coordLength].waypoints[coords[coordLength].waypoints.length - 1][0]]

        const station1 = coords[0].station
        const station2 = coords[coordLength].station
        console.log(station1, station2)
        this.state = {
            zoom: 18,
            departure: start,
            arrival: end,
            station1: station1 === null ? [] : [station1.position.latitude, station1.position.longitude],
            station2: station2 === null ? [] : [station2.position.latitude, station2.position.longitude],
            coords: coords
        }

    }

    handleClick = (e) => {
        e.preventDefault();
        this.props.history.push({
            pathname: '/',
        })
    }

    render() {
        return (
            <div className="container-fluid">
                <button type="button" onClick={this.handleClick} className="btn btn-primary">Back</button>
                  <MapContainer center={this.state.departure} zoom={13} scrollWheelZoom={false}>
                    <TileLayer
                        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
                    {<GeoJSON style={{ color: 'red' }} key={`start->station1`} data={this.state.coords[0]} />}
                    {<GeoJSON style={{ color: 'yellow' }} key={`station1->station2`} data={this.state.coords[1]} />}
                    {<GeoJSON style={{ color: 'green' }} key={`station2->end`} data={this.state.coords[2]} />}

                    <Marker position={this.state.arrival}>
                        <Popup>
                            Arrival
                        </Popup>
                    </Marker>

                    { 
                        this.state.station1.length !== 0 ?
                            <React.Fragment>
                                < Marker position={this.state.station1}>
                                    <Popup>
                                        Departure Station
                                    </Popup>
                                </Marker>
                                <Marker position={this.state.station2}>
                                    <Popup>
                                        Arrival Station
                                    </Popup>
                                </Marker>
                            </React.Fragment>
                            : (<div></div>)
                    }

                    <Marker position={this.state.departure}>
                        <Popup>
                            Departure
                        </Popup>
                    </Marker>
                </MapContainer>

            </div>
            )
    }
}

export  { Map };