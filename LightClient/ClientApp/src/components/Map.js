import React, { Component,  } from 'react';
//import PropTypes from 'prop-types';
import { MapContainer, TileLayer, Marker, Popup, GeoJSON } from 'react-leaflet'
import {Table} from 'react-bootstrap'

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

    backToHome = (e) => {
        e.preventDefault();
        this.props.history.push({
            pathname: '/',
        })
    }
    
    render() {
        return (
            <div id="map" style={this.container} >
                <button type="button" onClick={this.backToHome} className="btn btn-primary">Back</button>
                {
                    this.state.station1.length === 0 ? 
                        <React.Fragment>
                            <th>Total Distance: {(this.state.coords[0].features[0].properties.summary.distance/1000).toFixed(2)} km</th>
                        </React.Fragment>
                        :
                        <React.Fragment>
                            <div className="d-flex justify-content-between">
                                <th><span role="img" aria-label="donut">🔴</span>Origin to Departure Station: {(this.state.coords[0].features[0].properties.summary.distance/1000).toFixed(2)} km</th>
                                <th><span role="img" aria-label="donut">🟡</span>Departure Station to Arrival Station: {(this.state.coords[1].features[0].properties.summary.distance/1000).toFixed(2)} km</th>
                                <th><span role="img" aria-label="donut">🟢</span>Arrival Station to Destination: {(this.state.coords[2].features[0].properties.summary.distance/1000).toFixed(2)} km</th>
                            </div>
                        </React.Fragment>
                }
                
                  <MapContainer fullscreenControl={true} style={{ height: "70vh", width: "100%" }} center={this.state.departure} zoom={13} scrollWheelZoom={true}>
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
                                        Departure Station [{this.state.coords[0].station.address}]
                                    </Popup>
                                </Marker>
                                <Marker position={this.state.station2}>
                                    <Popup>     
                                        Arrival Station [{this.state.coords[this.state.coords.length - 1].station.address}]
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
                
                <div className="d-flex justify-content-center" >
                    <div>
                        <p className="text-center h5">Departure Station</p>
                        
                        {
                            this.state.station1.length === 0 ? 'Unavailable' :
                                <React.Fragment>
                                    <Table striped bordered hover variant="dark">
                                        <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Address</th>
                                            <th>Town</th>
                                            <th>Bikes</th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        <tr>
                                            <td>{this.state.coords[0].station.name}</td>
                                            <td>{this.state.coords[0].station.address}</td>
                                            <td>{this.state.coords[0].station.contractName}</td>
                                            <td>{this.state.coords[0].station.mainStands.availabilities.bikes}</td>
                                        </tr>
                                        </tbody>
                                    </Table>
                                </React.Fragment>
                        }
                    </div>
                    <td>&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <div>
                        <p className=" text-center h5">Arrival Station</p>

                        {
                            this.state.station2.length === 0 ? 'Unavailable' : 
                                <React.Fragment>
                                    <Table striped bordered hover variant="dark">
                                        <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Address</th>
                                            <th>Town</th>
                                            <th>Capacity</th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        <tr>
                                            <td>{this.state.coords[this.state.coords.length - 1].station.name}</td>
                                            <td>{this.state.coords[this.state.coords.length - 1].station.address}</td>
                                            <td>{this.state.coords[this.state.coords.length - 1].station.contractName}</td>
                                            <td>{this.state.coords[this.state.coords.length - 1].station.mainStands.capacity}</td>
                                        </tr>
                                        </tbody>
                                    </Table>
                                </React.Fragment>
                        }
                    </div>
                </div>

            </div>
            )
    }
}

export  { Map };