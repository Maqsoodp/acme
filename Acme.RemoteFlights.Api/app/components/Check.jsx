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

    onSubmit() {

        var formData = {
            'startDate': this.refs['startDate'].value,
            'endDate': this.refs['endDate'].value,
            'numberOfPax': this.refs['numberOfPax'].value,
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

        return false;
    }

    flightData(flights) {

        return (flights && flights.length > 0) ? flights.map(f => {
            var sTime = new Date(f.startTime);
            var eTime = new Date(f.endTime);
            return (<tr key={f.id}>
                <th scope="row">{f.flightNumber}</th>
                <td>{f.departureCity}</td>
                <td>{f.arrivalCity}</td>
                <td scope="row">{f.capacity}</td>
                <td>{sTime.getHours() + ':' + sTime.getMinutes()}</td>
                <td>{eTime.getHours() + ':' + eTime.getMinutes()}</td>
            </tr>)
        })
            : (<tr><td colSpan="6">No flights available for this date</td></tr>);
    }

    render() {
        const { available } = this.state;

        const datesData = (available && available.length > 0) ? available.map((f,i) => {
            var date = new Date(f.date);
            return (
                <div className="row" key={f.id}>
                    <h3 scope="row">{date.toISOString()}</h3>
                    <table className="table" key={i}>
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
                            {this.flightData(f.flights)}
                        </tbody>
                    </table>
                </div>

            )
        }) : null;


        return (
            <div className="container">
                <div className="row">
                    <h3>Select maximum range, about a week </h3>
                </div>
                <div className="row">
                    
                    <form>

                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="">Select Start and End date</span>
                            </div>
                            <input type="date" className="form-control" id="startDate" placeholder="Start Date" ref="startDate" />
                            <input type="date" className="form-control" id="endDate" placeholder="End Date" ref="endDate" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="numberOfPax" className="control-label">Number Of Passengers</label>
                            <input type="text" className="form-control" id="numberOfPax" placeholder="Number Of Passengers" ref="numberOfPax" />
                        </div>
                        <button type="button" onClick={this.onSubmit} className="btn btn-primary">Check Availability</button>
                    </form>
                </div>
                <div className="row">
                    {datesData}
                </div>
            </div>
        )
    }

}

export default Check;
