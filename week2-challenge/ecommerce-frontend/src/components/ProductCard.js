import React, { useState } from 'react';
import { useCart } from '../store/CartContext';
import { useNavigate } from 'react-router-dom';
import { isLoggedIn } from '../utils/auth';

export default function ProductCard({ product }) {
  const { addToCart } = useCart();
  const [added, setAdded] = useState(false);
  const [showMsg, setShowMsg] = useState(false);
  const navigate = useNavigate();

  const handleAddToCart = () => {
    if (!isLoggedIn()) {
      navigate('/login');
      return;
    }
    addToCart(product);
    setAdded(true);
    setShowMsg(true);
    setTimeout(() => setShowMsg(false), 1500); // Hide message after 1.5s
  };

  const handleBuyNow = () => {
    if (!isLoggedIn()) {
      navigate('/login');
      return;
    }
    navigate('/payment', { state: { amount: product.price, product } });
  };

  return (
    <div className="card h-100">
      {console.log('Rendering ProductCard for product:', product)}
      <img src={product.imageUrl || '/images/default-product.jpg'} alt={product.name} className="product-img" />
      <div className="card-body">
        <h5 className="card-title">{product.name}</h5>
        <p className="card-text">{product.description}</p>
        <p className="card-text"><strong>${product.price}</strong></p>
        <div className="d-flex justify-content-between">
          <button
            className="btn"
            style={{
              backgroundColor: added ? '#28a745' : '#007bff',
              color: '#fff'
            }}
            onClick={handleAddToCart}
            disabled={added}
          >
            <i className="bi bi-cart-plus"></i> {added ? 'Added' : 'Add to Cart'}
          </button>
          <button className="btn btn-secondary" onClick={() => handleBuyNow(product)}>
            Buy Now
          </button>
        </div>
        {showMsg && (
          <div className="alert alert-success mt-2 p-1 text-center" style={{ fontSize: '0.9em' }}>
            Added to cart successfully!
          </div>
        )}
      </div>
    </div>
  );
} 