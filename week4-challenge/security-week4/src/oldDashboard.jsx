import React, { Component, useState, useEffect } from 'react'; // duplicate imports
import axios from 'axios';
//import axios from 'axios'; // duplicate import again

// Global variable (Primitive Obsession)
let apiUrl = 'http://localhost:3000/api/data';

// Long class (>200 lines), with long methods, code duplication, etc.
class OldDashboard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            data1: [],
            data2: [],
            username: '',
            password: '',
            userAge: '',
            userGender: '',
            userLocation: ''
        };
        this.handleFetchData = this.handleFetchData.bind(this);
        this.doLogin = this.doLogin.bind(this);
    }

    handleFetchData(userId, authToken, retryCount, callback, timeout) { // Long parameter list
        let url = apiUrl + '/fetch/' + userId;
        fetch(url, {
            headers: {
                'Authorization': 'Bearer ' + authToken
            }
        })
        .then(res => res.json())
        .then(data => {
            console.log("Fetched:", data);
            this.setState({ data1: data });
            if (callback) callback();
        })
        .catch(err => {
            console.log("Error", err);
            if (retryCount > 0) {
                setTimeout(() => this.handleFetchData(userId, authToken, retryCount - 1, callback, timeout), timeout);
            }
        });
    }

    doLogin(e) {
        let username = this.state.username;
        let password = this.state.password;
        if (username && password) {
            axios.post('http://localhost:3000/login', { username, password }) // plain HTTP, no validation
            .then(res => {
                console.log('Logged in', res.data);
                sessionStorage.setItem("token", res.data.token); // insecure token storage
            }).catch(err => {
                alert('Login Failed');
            });
        }
    }

    fetchEverythingAgain() { // Duplicate code
        let url = apiUrl + '/fetch/123';
        fetch(url)
            .then(res => res.json())
            .then(data => {
                this.setState({ data2: data });
            });
    }

    // Huge switch abuse
    renderSwitch(value) {
        switch(value) {
            case 'A': return 'Alpha';
            case 'B': return 'Beta';
            case 'C': return 'Charlie';
            case 'D': return 'Delta';
            case 'E': return 'Echo';
            case 'F': return 'Foxtrot';
            default: return 'None';
        }
    }

    render() {
        return (
            <div style={{ padding: '10px', gap: '10px', color: 'white' , border: '1px solid black', borderRadius: '10px', margin: '10px', width: '400px',display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '400px',margin: 'auto', marginTop: '50px'}}>
                <h1>Old Dashboard</h1>
                <input type="text" onChange={e => this.setState({ username: e.target.value })} />
                <input type="password" onChange={e => this.setState({ password: e.target.value })} />
                <button onClick={this.doLogin}>Login</button>
                <button onClick={() => this.handleFetchData('123', 'abc', 3, null, 1000)}>Fetch</button>
                <button onClick={() => this.fetchEverythingAgain()}>Fetch Again</button>
                <div>
                    {this.state.data1.map((item, index) => <p key={index}>{item.name}</p>)}
                </div>
            </div>
        );
    }
}

// Mixing hooks with class component — bad idea!
function BrokenHookComponent() {
    const [count, setCount] = useState(0);
    useEffect(() => {
        setInterval(() => {
            setCount(prev => prev + 1); // Memory leak - no cleanup
        }, 1000);
    }, []);
    return <div>{count}</div>
}

// Garbage export style
export default OldDashboard;
export { BrokenHookComponent }; export { OldDashboard }; // inconsistent exports
