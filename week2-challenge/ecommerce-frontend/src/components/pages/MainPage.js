import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Navbar from '../Navbar';
import Sidebar from '../Sidebar';
import ProductList from '../ProductList';
import { useCart } from '../../store/CartContext';
import api from '../../services/api';

const categories = [
  { key: 'electronics', label: 'Electronics', logo: '💻' },
  { key: 'fashion', label: 'Fashion', logo: '👗' },
  { key: 'mobile', label: 'Mobiles', logo: '📱' },
  { key: 'appliances', label: 'Appliances', logo: '🔌' },
  { key: 'books', label: 'Books', logo: '📚' },
  { key: 'furniture', label: 'Furniture', logo: '🛋️' },
  { key: 'groceries', label: 'Groceries', logo: '🛒' },
  { key: 'health', label: 'Health', logo: '💊' },
  { key: 'jewelry', label: 'Jewelry', logo: '💍' },
];

export default function MainPage() {
  const location = useLocation();
  const [category, setCategory] = useState(null);
  const { addToCart } = useCart();
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);

  // Read 'search' query from URL
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const search = params.get('search');
    if (search) {
      setCategory(search.toLowerCase());
    }
  }, [location.search]);

  useEffect(() => {
    const fetchProducts = async () => {
      const products = await api.get('product');
      setProducts(products);
    };
    fetchProducts();
  }, []);

  // Handler for clicking category buttons
  const handleCategoryClick = (catKey) => {
    setCategory(catKey);
    window.history.replaceState(null, '', `/main?search=${encodeURIComponent(catKey)}`);
  };

  const handleAddToCart = (product) => {
    addToCart(product);
    navigate('/cart');
  };

  return (
    <>
      <Navbar />
      <div className="container-fluid mt-3">
        <div className="row">
          <div className="col-md-2">
            <Sidebar onCategorySelect={setCategory} />
          </div>
          <div className="col-md-10">
            <ProductList category={category} />
          </div>
        </div>
      </div>
    </>
  );
} 