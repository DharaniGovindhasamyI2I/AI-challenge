import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import './Payment.css';
import axios from 'axios';

import { useUser } from '../../store/UserContext';
import { useCart } from '../../store/CartContext';


export default function Payment({ amount = 999 }) {
  const [paymentMode, setPaymentMode] = useState('');
  const [accountNumber, setAccountNumber] = useState('');
  const [holderName, setHolderName] = useState('');
  const [cvv, setCvv] = useState('');
  const [upiId, setUpiId] = useState('');
  const [bank, setBank] = useState('');
  const [errors, setErrors] = useState({});
  const [result, setResult] = useState('');
  const [retry, setRetry] = useState(false);
  const navigate = useNavigate();
  const { user } = useUser();
  const { cartItems } = useCart();
  const cartProduct = cartItems[0];

  if (!user) {
    console.log("User not found");
    return <div>Please log in to proceed with payment.</div>;
  }

  console.log("Cart Items:", cartItems);
  console.log("Cart Product:", cartProduct);
  console.log("User:", user);

  const validate = () => {
    const errs = {};
    if (!paymentMode) {
      errs.paymentMode = 'Please select a payment mode.';
    }
    if (paymentMode === 'credit' || paymentMode === 'debit') {
      if (!accountNumber || !/^\d{12,19}$/.test(accountNumber)) {
        errs.accountNumber = 'Enter a valid card number (12-19 digits).';
      }
      if (!holderName || holderName.length < 3) {
        errs.holderName = 'Enter a valid holder name (min 3 chars).';
      }
      if (!cvv || !/^\d{3,4}$/.test(cvv)) {
        errs.cvv = 'Enter a valid CVV (3 or 4 digits).';
      }
    }
    if (paymentMode === 'upi') {
      if (!upiId || !/^\w+@\w+$/.test(upiId)) {
        errs.upiId = 'Enter a valid UPI ID (e.g., name@bank).';
      }
    }
    if (paymentMode === 'netbanking') {
      if (!bank) {
        errs.bank = 'Please select a bank.';
      }
    }
    return errs;
  };

  const isFormValid = () => {
    const errs = validate();
    return Object.keys(errs).length === 0;
  };

  const handleConfirm = async () => {
    const errs = validate();
    setErrors(errs);
    if (Object.keys(errs).length > 0) return;
    // Simulate payment process
    setResult('');
    setRetry(false);
    try {
      const response = await axios.post('http://localhost:5031/api/payment', {
        email: user.email,
        product: cartProduct.title,
        price: cartProduct.price
      });
      if (response.data.status === 'success') {
        setResult('success');
        navigate('/payment-success');
      } else {
        setResult('failure');
        navigate('/payment-failure');
      }
    } catch (error) {
      console.error('Error confirming payment:', error);
      setResult('failure');
      navigate('/payment-failure');
    }
  };

  const handleRetry = () => {
    setResult('');
    setRetry(false);
    navigate('/payment-failure');
  };

  return (
    <div className="payment-bg">
      <div className="payment-card">
        <h2 className="payment-title">Checkout & Payment</h2>
        <div className="payment-summary">
          <span>Total Amount:</span>
          <span className="payment-amount">₹{amount}</span>
        </div>
        {result === 'success' ? (
          <div className="alert alert-success text-center mt-3">Payment Successful!</div>
        ) : result === 'failure' ? (
          <div className="alert alert-danger text-center mt-3">
            Payment Failed! Please retry after a few minutes.<br/>
            <button className="btn btn-link mt-2" onClick={handleRetry}>Retry</button>
          </div>
        ) : (
        <form className="payment-form" onSubmit={e => { e.preventDefault(); handleConfirm(); }}>
          <label>Choose Payment Mode</label>
          <select
            className="payment-input"
            value={paymentMode}
            onChange={e => { setPaymentMode(e.target.value); setErrors({}); }}
          >
            <option value="">Select</option>
            <option value="credit">Credit Card</option>
            <option value="debit">Debit Card</option>
            <option value="upi">UPI</option>
            <option value="netbanking">Net Banking</option>
            <option value="cod">Cash on Delivery</option>
          </select>
          {errors.paymentMode && <div className="text-danger small">{errors.paymentMode}</div>}
          {/* Card Fields */}
          {(paymentMode === 'credit' || paymentMode === 'debit') && (
            <>
              <input
                className="payment-input"
                type="text"
                placeholder="Card Number"
                value={accountNumber}
                onChange={e => setAccountNumber(e.target.value)}
              />
              {errors.accountNumber && <div className="text-danger small">{errors.accountNumber}</div>}
              <input
                className="payment-input"
                type="text"
                placeholder="Card Holder Name"
                value={holderName}
                onChange={e => setHolderName(e.target.value)}
              />
              {errors.holderName && <div className="text-danger small">{errors.holderName}</div>}
              <input
                className="payment-input"
                type="password"
                placeholder="CVV"
                value={cvv}
                onChange={e => setCvv(e.target.value)}
                maxLength={4}
              />
              {errors.cvv && <div className="text-danger small">{errors.cvv}</div>}
            </>
          )}
          {/* UPI Field */}
          {paymentMode === 'upi' && (
            <>
              <input
                className="payment-input"
                type="text"
                placeholder="Enter UPI ID (e.g. name@bank)"
                value={upiId}
                onChange={e => setUpiId(e.target.value)}
              />
              {errors.upiId && <div className="text-danger small">{errors.upiId}</div>}
            </>
          )}
          {/* Net Banking Field */}
          {paymentMode === 'netbanking' && (
            <>
              <select
                className="payment-input"
                value={bank}
                onChange={e => setBank(e.target.value)}
              >
                <option value="">Select Bank</option>
                <option value="hdfc">HDFC</option>
                <option value="sbi">SBI</option>
                <option value="icici">ICICI</option>
                <option value="axis">Axis</option>
              </select>
              {errors.bank && <div className="text-danger small">{errors.bank}</div>}
            </>
          )}
          {/* COD Info */}
          {paymentMode === 'cod' && (
            <div className="payment-cod-info">
              <span>Cash on Delivery selected. Please confirm your order.</span>
            </div>
          )}
          <button
            className="payment-btn"
            type="submit"
            disabled={!isFormValid()}
          >
            Confirm Payment
          </button>
        </form>
        )}
      </div>
    </div>
  );
}
