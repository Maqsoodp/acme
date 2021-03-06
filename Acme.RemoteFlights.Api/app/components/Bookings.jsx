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

    onSubmit(e) {
        e.preventDefault();
        var flightDate = this.refs['date'].value;
        var passengerName = this.refs['passengerName'].value;
        var flightId = this.state.flightId;

        if (flightDate.length > 0 && passengerName.length > 0 && flightId.length > 0) {
            flightDate = flightDate + 'T00:00:00';
            var formData = {
                'passengerName': passengerName,
                'flightDate': flightDate,
                'flightId': flightId
            };

            const myRequest = new Request("/api/flight/bookings", {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: new Headers({
                    'Content-Type': 'application/json'
                })
            });

            fetch(myRequest)
                .then(response => response.json())
                .then(bookings => {
                    if (bookings.message) {
                        throw new Error(bookings.message);
                    } else {
                        this.setState({ bookings, result: true });
                        alert('Successfully added');
                    }
                }).catch((error, msg) => {
                    this.setState({ result: false });
                    alert(error);
                });
        }
        return false;
    }

    render() {
        const { flights, result } = this.state;

        const flightsData = flights ? flights.map(f => {
            return (
                <a key={f.id}
                    onClick={(e) => this.handleLink(e, f.id)}
                    className="dropdown-item"
                    href="#">{f.departureCity + ' - ' + f.arrivalCity + ' - ' + f.startTime + ' - ' + f.endTime}</a>
            )
        }) : null;

        const minDate = new Date().toJSON().split('T')[0];

        return (
            <div className="container">
                <div className="row">
                    <form>
                        <div className="form-group">
                            <label htmlFor="passengerName" className="control-label">Passenger Name</label>
                            <input type="text" className="form-control" id="passengerName" placeholder="Name" ref="passengerName" required />
                        </div>
                        <div className="form-group">
                            <label htmlFor="date" className="control-label">Flight date</label>
                            <input type="date" min={minDate} className="form-control" id="date" placeholder="Flight Date" ref="date" required />
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
                        <button onClick={this.onSubmit} className="btn btn-primary">Book</button>
                    </form>
                </div>
            </div>
        )
    }

}

export default Bookings;
