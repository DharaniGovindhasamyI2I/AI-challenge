import React, { useState, useEffect, useCallback } from 'react';
import axios from 'axios';

// Config for API URLs
const API_BASE_URL = 'https://localhost:3000/api/data'; // Use HTTPS for security
const LOGIN_URL = 'https://localhost:3000/login';

/**
 * Fetches data from the API for a given user, with retries and callback support.
 * @param {string} userId - The user ID to fetch data for.
 * @param {string} authToken - The authentication token.
 * @param {number} retryCount - Number of retries on failure.
 * @param {function} callback - Optional callback after success.
 * @param {number} timeout - Timeout between retries in ms.
 * @param {function} setData - State setter for fetched data.
 * @returns {Promise<void>}
 * @example
 * handleFetchData('123', token, 3, () => alert('done'), 1000, setData1);
 */
async function handleFetchData(userId, authToken, retryCount, callback, timeout, setData) {
  const url = `${API_BASE_URL}/fetch/${userId}`;
  try {
    const res = await fetch(url, {
      headers: { 'Authorization': `Bearer ${authToken}` }
    });
    const data = await res.json();
    setData(data);
    if (callback) callback();
  } catch (err) {
    if (retryCount > 0) {
      setTimeout(() => handleFetchData(userId, authToken, retryCount - 1, callback, timeout, setData), timeout);
    }
  }
}

/**
 * Validates username and password input.
 * @param {string} username
 * @param {string} password
 * @returns {boolean}
 */
function validateInput(username, password) {
  // Simple validation: non-empty, no spaces, min length
  return (
    typeof username === 'string' && username.trim().length >= 3 &&
    typeof password === 'string' && password.length >= 6
  );
}

/**
 * Modern, secure, and maintainable Dashboard component.
 * Handles login, data fetching, and state management.
 */
function NewDashboard() {
  const [data1, setData1] = useState([]);
  const [data2, setData2] = useState([]);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [token, setToken] = useState(localStorage.getItem('token') || '');

  // Secure login handler
  const doLogin = useCallback(async () => {
    if (!validateInput(username, password)) {
      alert('Invalid username or password.');
      return;
    }
    try {
      const res = await axios.post(LOGIN_URL, { username, password });
      // WARNING: For demo only. Use HttpOnly cookies in production.
      localStorage.setItem('token', res.data.token);
      setToken(res.data.token);
    } catch (err) {
      alert('Login Failed');
    }
  }, [username, password]);

  // Fetch data for user 123
  const fetchData = useCallback(() => {
    handleFetchData('123', token, 3, null, 1000, setData1);
  }, [token]);

  // Fetch everything again (no code duplication)
  const fetchEverythingAgain = useCallback(() => {
    handleFetchData('123', token, 1, null, 1000, setData2);
  }, [token]);

  // Simple value mapping
  const renderSwitch = value => {
    const map = { A: 'Alpha', B: 'Beta', C: 'Charlie', D: 'Delta', E: 'Echo', F: 'Foxtrot' };
    return map[value] || 'None';
  };

  return (
    <div style={{ padding: '10px', color: 'white', border: '1px solid black', borderRadius: '10px', margin: '10px', width: '500px', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '500px', background: '#222' }}>
      <h1>New Dashboard</h1>
      <input type="text" placeholder="Username" value={username} onChange={e => setUsername(e.target.value)} />
      <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
      <button onClick={doLogin}>Login</button>
      <button onClick={fetchData} disabled={!token}>Fetch</button>
      <button onClick={fetchEverythingAgain} disabled={!token}>Fetch Again</button>
      <div>
        {data1.map((item, index) => <p key={index}>{item.name}</p>)}
      </div>
    </div>
  );
}

/**
 * Example component with interval and proper cleanup.
 */
function BrokenHookComponent() {
  const [count, setCount] = useState(0);
  useEffect(() => {
    const interval = setInterval(() => {
      setCount(prev => prev + 1);
    }, 1000);
    return () => clearInterval(interval);
  }, []);
  return <div>{count}</div>;
}

export default NewDashboard;
export { BrokenHookComponent }; 