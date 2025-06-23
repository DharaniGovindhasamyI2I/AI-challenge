import React, { useContext } from 'react';
import { CartContext } from '../../store/CartContext';
import { useNavigate } from 'react-router-dom';
import './Cart.css';
import { useCart, CartProvider } from '../../store/CartContext';

export default function Cart() {
  const { cartItems } = useCart();
  const navigate = useNavigate();

  const totalAmount = cartItems.reduce((sum, item) => sum + (item.price || 0), 0);

  const handleProceed = () => {
    if (window.confirm('Do you want to proceed to payment?')) {
      navigate('/payment');
    }
  };

  const handleCancel = () => {
    navigate('/main');
  };

  if (cartItems.length === 0) {
    return <div className="cart-container"><h3>Your cart is empty.</h3></div>;
  }

  return (
    <div className="cart-container">
      <h2>Your Cart</h2>
      <ul className="cart-list">
        {cartItems.map((item, idx) => (
          <li key={idx} className="cart-item">
            <img src={item.imageUrl} alt={item.name} className="cart-img" />
            <div>
              <h5>{item.name}</h5>
              <p>₹{item.price}</p>
            </div>
          </li>
        ))}
      </ul>
      <div className="cart-total">Total: <strong>₹{totalAmount}</strong></div>
      <div className="cart-actions">
        <button className="btn btn-success" onClick={handleProceed}>Proceed to Payment</button>
        <button className="btn btn-outline-secondary" onClick={handleCancel}>Cancel</button>
      </div>
    </div>
  );
}
