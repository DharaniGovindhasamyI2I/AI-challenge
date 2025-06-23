import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { isLoggedIn } from '../utils/auth';

export default function Navbar() {
  const navigate = useNavigate();
  const [search, setSearch] = useState('');

  const handleLogout = () => {
    localStorage.removeItem('token');
    // Remove any other user info if stored
    navigate('/login');
  };

  const handleSearch = (e) => {
    e.preventDefault();
    if (!isLoggedIn()) {
      navigate('/login');
      return;
    }
    if (search.trim()) {
      navigate(`/main?search=${encodeURIComponent(search)}`);
      setSearch('');
    }
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <div className="container">
        <Link className="navbar-brand d-flex align-items-center" to="/main">
          <span className="bi bi-bag me-2" style={{ fontSize: '1.5em' }}></span>
          E-Shop
        </Link>
        <form className="d-flex mx-3" onSubmit={handleSearch}>
          <input
            className="form-control me-2"
            style={{ width: '300px' }}
            type="search"
            placeholder="Search category or product"
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
          <button className="btn btn-outline-success" type="submit">Search</button>
        </form>
        <div className="d-flex align-items-center justify-content-center gap-3 ">
          <Link className="nav-link" to="/main">Home</Link>
          <span style={{ fontSize: '1.5em', marginRight: '0.5em' }}>🛒</span>
          <Link className="nav-link" to="/cart">Cart</Link>
          {/* Profile Dropdown */}
          <div className="dropdown">
            <button className="btn btn-link nav-link dropdown-toggle d-flex align-items-center" type="button" id="profileDropdown" data-bs-toggle="dropdown" aria-expanded="false">
              <span className="bi bi-person-circle" style={{ fontSize: '1.5em' }}></span>
            </button>
            <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="profileDropdown">
              <li><Link className="dropdown-item" to="/about">About</Link></li>
              <li><button className="dropdown-item" onClick={handleLogout}>Logout</button></li>
            </ul>
          </div>
        </div>
      </div>
    </nav>
  );
} 