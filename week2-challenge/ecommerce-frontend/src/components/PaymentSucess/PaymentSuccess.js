import React from 'react';
import { useNavigate } from 'react-router-dom';
import './PaymentSuccess.css';

export default function PaymentSuccess() {
  const navigate = useNavigate();
  return (
    <div className="payment-result-bg">
      <div className="payment-result-card success">
        <div className="icon-circle success">
          <span className="checkmark">&#10003;</span>
        </div>
        <h2>Payment Successful!</h2>
        <p>Your order has been placed and payment was received.</p>
        <button className="result-btn" onClick={() => navigate('/main')}>Continue Shopping</button>
      </div>
    </div>
  );
}
