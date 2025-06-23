import React, { useEffect, useState } from 'react';
import axios from '../services/api';
import ProductCard from './ProductCard';

const openApiCategories = {
  electronics: 'https://fakestoreapi.com/products/category/electronics',
  jewelry: 'https://fakestoreapi.com/products/category/jewelery',
  men: 'https://fakestoreapi.com/products/category/men%27s%20clothing',
  fashion: 'https://fakestoreapi.com/products/category/men%27s%20clothing',
  women: 'https://fakestoreapi.com/products/category/women%27s%20clothing',
  books: 'https://openlibrary.org/search.json?q=subject:fiction',
  groceries: 'https://dummyjson.com/products/category/groceries',
  furniture: 'https://dummyjson.com/products/category/furniture',
  appliances: 'https://dummyjson.com/products/category/home-decoration',
  mobile: 'https://dummyjson.com/products/category/smartphones',
  health: 'https://dummyjson.com/products/category/skincare',
  home: 'https://dummyjson.com/products/category/home-decoration',
  food: 'https://world.openfoodfacts.org',
};

export default function ProductList({ category }) {
  const [products, setProducts] = useState([]);
  const [message, setMessage] = useState('');
  useEffect(() => {
    setMessage('');
    if (category && openApiCategories[category]) {
      fetch(openApiCategories[category])
        .then(res => res.json())
        .then(data => {
          let items = [];
          if (Array.isArray(data)) {
            items = data;
          } else if (data.products) {
            items = data.products.map(p => ({
              ...p,
              imageUrl: p.thumbnail || p.images?.[0] || '/images/default-product.jpg'
            }));
          } else if (data.docs) {
            items = data.docs.map(book => ({
              id: book.key,
              name: book.title,
              description: book.author_name ? book.author_name.join(', ') : '',
              price: 0,
              imageUrl: book.cover_i
                ? `https://covers.openlibrary.org/b/id/${book.cover_i}-M.jpg`
                : '/images/default-product.jpg'
            }));
          }
          setProducts(items);
          if (!items || items.length === 0) setMessage('No products found for this category.');
        })
        .catch(() => setMessage('Failed to fetch products from open API.'));
    } else if (category) {
      setProducts([]);
      setMessage('No open API available for this category.');
    } else {
      const fetchProducts = async () => {
        try {
          const data = await axios.get('/product');
          setProducts(Array.isArray(data) ? data : []);
        } catch (e) {
          setProducts([]); // fallback on error
        }
      };
      fetchProducts();
    }
  }, [category]);
  return (
    <div className="row">
      {message && <div className="alert alert-info w-100">{message}</div>}
      {products.map(p => (
        <div className="col-md-4 mb-3" key={p.id}>
          <ProductCard product={p} />
        </div>
      ))}
    </div>
  );
} 