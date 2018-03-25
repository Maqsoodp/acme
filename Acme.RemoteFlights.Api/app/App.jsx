import React, { Component } from 'react';

import Flights from './components/Flights';
import Search from './components/Search';
import Book from './components/Bookings';
import Check from './components/Check';

const $ = require('jquery');

class App extends Component {
    constructor(props) {
        super(props);

        this.state = {
            route: 'all',
        };
        this.handleLink = this.handleLink.bind(this);
    }

    handleLink(e, route) {
       e.preventDefault()
        this.setState({route});
    }

    render() {
        const { route } = this.state;
        let component;
        switch (route) {
            case "search":
                component = (<Search />)
                break;
            case "book":
                component = (<Book />)
                break;
            case "check":
                component = (<Check />)
                break;
            case "all":
            default:
                component = (<Flights />)
        }
        return (
            <div className="row">
                <div className="col-3 menu">
                    <ul>
                        <li
                            className={route == 'all' ? 'selected' : null}
                            onClick={(e) => this.handleLink(e, 'all')}>All flights</li>
                        <li
                            className={route == 'search' ? 'selected' : null}
                            onClick={(e) => this.handleLink(e, 'search')}>Search bookings</li>
                        <li className={route == 'check' ? 'selected' : null}
                            onClick={(e) => this.handleLink(e, 'check')}>Check availability</li>
                        <li className={route == 'book'? 'selected' : null}
                            onClick={(e) => this.handleLink(e, 'book')}>Book a flight</li>
                    </ul>
                </div>
                <div className="col-9">
                    {component}
                </div>
            </div>

        );
    }
}

export default App;
