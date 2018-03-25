import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Bookings extends Component {

    constructor(props) {
        super(props);

        this.handleLink = this.handleLink.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

        this.state = {
            flights: [],
            result: null
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
                console.error(error);
            });
    }

    handleLink(e, flightId) {
        e.preventDefault()
        this.setState({ flightId });
    }

    onSubmit() {

        var date1 = this.refs['date'].value;
        date1 = date1 + 'T00:00:00';
        var formData = {
            'passengerName': this.refs['passengerName'].value,
            'flightDate': date1,
            'flightId': this.state.flightId
        };

        const myRequest = new Request("/api/flight/bookings", {
            method: 'POST',
            body: JSON.stringify(formData),
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });

        fetch(myRequest)
            .then(response => {
                if (response && response.status === 200) {
                    return response.json();
                } else {
                    throw new Error('Something went wrong on api server!');
                }
            }).then(bookings => {
                this.setState({ bookings, result: true });
                alert('Successfully added');
            }).catch(error => {
                this.setState({ result: false });
                alert(`Error - ${error.message}`);
            });

        return false;
    }

    render() {
        const { flights, result } = this.state;

        const flightsData = flights ? flights.map(f => {
            return (
                <a key={f.id}
                    onClick={(e) => this.handleLink(e, f.id)}
                    className="dropdown-item"
                    href="#">{f.departureCity + ' - ' + f.arrivalCity}</a>
            )
        }) : null;

        return (
            <div className="container">
                <div className="row">
                    <form>
                        <div className="form-group">
                            <label htmlFor="passengerName" className="control-label">Passenger Name</label>
                            <input type="text" className="form-control" id="passengerName" placeholder="Name" ref="passengerName" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="date" className="control-label">Flight date</label>
                            <input type="date" className="form-control" id="date" placeholder="Flight Date" ref="date" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="dropdownMenuButton" className="control-label">Select Flight</label>
                            <div className="dropdown">
                                <button className="btn btn-secondary dropdown-toggle btn-lg" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Flights
                                </button>
                                <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                    {flightsData}
                                </div>
                            </div>
                        </div>

                        <div className="form-group">

                        </div>
                        <button type="button" onClick={this.onSubmit} className="btn btn-primary">Book</button>
                    </form>
                </div>
            </div>
        )
    }

}

export default Bookings;
