import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Search extends Component {

    constructor(props) {
        super(props);
        this.onSubmit = this.onSubmit.bind(this);
        this.state = {
            bookings: []
        }
    }

    onSubmit() {

        var formData = {
            'passengerName': this.refs['passengerName'].value,
            'date': this.refs['date'].value,
            'departureCity': this.refs['departureCity'].value,
            'arrivalCity': this.refs['arrivalCity'].value,
            'flightNumber': this.refs['flightNumber'].value,
        };

        const myRequest = new Request("/api/flight/search", {
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
                this.setState({ bookings });
            }).catch(error => {
                alert(`Error - ${error.message}`);
            });

        return false;
    }

    render() {
        const { bookings } = this.state;

        const bookingsData = (bookings && bookings.length > 0) ? bookings.map(f => {

            var flightDate = new Date(f.flightDate);
            var flight = f.flight;
            var sTime = new Date(flight.startTime);
            var eTime = new Date(flight.endTime);
            return (<tr>
                <th scope="row">{f.passengerName}</th>

                <td>{flight.departureCity}</td>
                <td>{flight.arrivalCity}</td>
                <th scope="row">{flight.flightNumber}</th>

                <td>{flightDate.toISOString()}</td>
                <td>{sTime.getHours() + ':' + sTime.getMinutes()}</td>
                <td>{eTime.getHours() + ':' + eTime.getMinutes()}</td>
            </tr>)
        })
            : (<tr><td colSpan="7">No Results found</td></tr>);


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
                            <label htmlFor="departureCity" className="control-label">departureCity</label>
                            <input type="text" className="form-control" id="departureCity" placeholder="departure City" ref="departureCity" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="arrivalCity" className="control-label">arrivalCity</label>
                            <input type="text" className="form-control" id="arrivalCity" placeholder="arrivalCity" ref="arrivalCity" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="flightNumber" className="control-label">flight Number</label>
                            <input type="text" className="form-control" id="flightNumber" placeholder="flight Number" ref="flightNumber" />
                        </div>
                        <button type="button" onClick={this.onSubmit} className="btn btn-primary">Search</button>
                    </form>
                </div>
                <div className="row">
                    <table className="table">
                        <thead>
                            <tr>
                                <th scope="col">Passenger Name</th>
                                <th scope="col">Departure City</th>
                                <th scope="col">Arrival City</th>

                                <th scope="col">Flight Number</th>
                                <th scope="col">Flight Date</th>

                                <th scope="col">Start Time</th>
                                <th scope="col">End Time</th>
                            </tr>
                        </thead>
                        <tbody>
                            {bookingsData}
                        </tbody>
                    </table>
                </div>
            </div>
        )
    }

}

export default Search;
