import React from 'react';
import { useNavigate } from 'react-router-dom';
import './PaymentFailure.css';

export default function PaymentFailure() {
  const navigate = useNavigate();
  return (
    <div className="payment-result-bg">
      <div className="payment-result-card failure">
        <div className="icon-circle failure">
          <span className="crossmark">&#10007;</span>
        </div>
        <h2>Payment Failed</h2>
        <p>We couldn't process your payment.<br />Please try again after a few minutes.</p>
        <button className="result-btn" onClick={() => navigate('/payment')}>Retry Payment</button>
      </div>
    </div>
  );
}
