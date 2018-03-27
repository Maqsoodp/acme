import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Check extends Component {

    constructor(props) {
        super(props);
        this.onSubmit = this.onSubmit.bind(this);
        this.state = {
            available: []
        }
    }

    onSubmit(e) {

        e.preventDefault();
        var startDate = this.refs['startDate'].value;
        var endDate = this.refs['endDate'].value;
        var numberOfPax = this.refs['numberOfPax'].value;

        if (startDate.length > 0 && endDate.length > 0 && numberOfPax.length > 0) {

            var formData = {
                'startDate': startDate,
                'endDate': endDate,
                'numberOfPax': numberOfPax,
            };

            const myRequest = new Request("/api/flight/availability", {
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
                }).then(available => {
                    this.setState({ available });
                }).catch(error => {
                    alert(`Error - ${error.message}`);
                });

        }
        return false;
    }

    flightData(flights) {

        return (flights && flights.length > 0) ? flights.map(f => {
            return (<tr key={f.id}>
                <th scope="row">{f.flightNumber}</th>
                <td>{f.departureCity}</td>
                <td>{f.arrivalCity}</td>
                <td scope="row">{f.capacity}</td>
                <td>{f.startTime}</td>
                <td>{f.endTime}</td>
            </tr>)
        })
            : (<tr><td colSpan="6">No flights available for this date</td></tr>);
    }

    render() {
        const { available } = this.state;

        const datesData = (available && available.length > 0) ? available.map((f,i) => {
            var date = new Date(f.date).toJSON().split('T')[0];
            return (
                <div className="row" key={f.id}>
                    <h3 scope="row">{date.toString()}</h3>
                    <table className="table" key={i}>
                        <thead>
                            <tr>
                                <th scope="col">Flight Number</th>
                                <th scope="col">Departure City</th>
                                <th scope="col">Arrival City</th>
                                <th scope="col">Seats available</th>
                                <th scope="col">Start Time</th>
                                <th scope="col">End Time</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.flightData(f.flights)}
                        </tbody>
                    </table>
                </div>

            )
        }) : null;

        const maxLimit = new Date(new Date().setDate(new Date().getDate() + 30)).toJSON().split('T')[0];
        const minDate = new Date().toJSON().split('T')[0];

        return (
            <div className="container">
                <div className="row">
                    <h3>Search is limited to a maximunm of 30 Days </h3>
                </div>
                <div className="row">
                    <form>
                        <div className="input-group">
                            <div className="input-group-prepend">
                                <span className="input-group-text" id="">Select Start and End date</span>
                            </div>
                            <input type="date" min={minDate} className="form-control" id="startDate" placeholder="Start Date" ref="startDate" required />
                            <input type="date" min={minDate} max={maxLimit} className="form-control" id="endDate" placeholder="End Date" ref="endDate" required />
                        </div>
                        <div className="form-group">
                            <label htmlFor="numberOfPax" className="control-label">Number Of Passengers</label>
                            <input type="text" className="form-control" id="numberOfPax" placeholder="Number Of Passengers" ref="numberOfPax" required/>
                        </div>
                        <button type="submit" onClick={this.onSubmit} className="btn btn-primary">Check Availability</button>
                    </form>
                </div>
                <div className="row" key="result">
                    {datesData}
                </div>
            </div>
        )
    }

}

export default Check;
