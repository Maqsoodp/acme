import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Flights extends Component {

    constructor(props) {
        super(props);
        this.state = {
            flights: []
        }
    }

    componentDidMount() {
        const myRequest = new Request("/api/flight");
        fetch(myRequest)
            .then(response => {
                if (response && response.status === 200) {
                    return response.json();
                } else {
                    throw new Error('Something went wrong on api server!');
                }
            }).then(flights => {
                this.setState({ flights });
            }).catch(error => {
                alert(`Error - ${error.message}`);
            });
    }

    render() {
        const { flights } = this.state;

        const flightsData = flights ? flights.map(f => {
            return (<tr key={f.id}>
                <th scope="row">{f.flightNumber}</th>
                <td>{f.departureCity}</td>
                <td>{f.arrivalCity}</td>
                <td>{f.capacity}</td>
                <td>{f.startTime}</td>
                <td>{f.endTime}</td>
            </tr>)
        })
            : null;

        return (
            <div className="row">
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">Flight Number</th>
                            <th scope="col">Departure City</th>
                            <th scope="col">Arrival City</th>
                            <th scope="col">Capacity</th>
                            <th scope="col">Start Time</th>
                            <th scope="col">End Time</th>
                        </tr>
                    </thead>
                    <tbody>
                        {flightsData}
                    </tbody>
                </table>

            </div>
        );
    }
}


export default Flights;
