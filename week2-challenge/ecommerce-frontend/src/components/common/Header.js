import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { isLoggedIn, logout } from '../../utils/auth';
import { useCart } from '../../store/CartContext';

export default function Header() {
  const navigate = useNavigate();
  const { cartItems } = useCart();
  const [showMenu, setShowMenu] = useState(false);

  const handleProfileClick = () => setShowMenu((prev) => !prev);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container">
        <Link className="navbar-brand" to="/">E-Shop</Link>
        <div>
          <span>Cart: {cartItems.length}</span>
          {!isLoggedIn() ? (
            <>
              <Link className="btn btn-outline-light mx-2" to="/login">Login</Link>
              <Link className="btn btn-outline-light" to="/register">Register</Link>
            </>
          ) : (
            <div style={{ position: 'relative', display: 'inline-block' }}>
              <button onClick={handleProfileClick}>👤</button>
              {showMenu && (
                <div style={{
                  position: 'absolute', right: 0, background: '#fff', border: '1px solid #ccc', zIndex: 100
                }}>
                  <Link className="dropdown-item" to="/account">My Account</Link>
                  <Link className="dropdown-item" to="/wishlist">Wishlist</Link>
                  <Link className="dropdown-item" to="/notifications">Notifications</Link>
                  <Link className="dropdown-item" to="/gift-cards">Gift Cards</Link>
                  <button className="dropdown-item" onClick={handleLogout}>Logout</button>
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </nav>
  );
} 