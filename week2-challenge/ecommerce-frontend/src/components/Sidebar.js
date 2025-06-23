import React from 'react';

const categories = [
  { key: 'electronics', label: 'Electronics', icon: 'bi-tv', logo: '💻' },
  { key: 'fashion', label: 'Fashion', icon: 'bi-bag', logo: '👗' },
  { key: 'mobile', label: 'Mobiles', icon: 'bi-phone', logo: '📱' },
  { key: 'appliances', label: 'Appliances', icon: 'bi-plug', logo: '🔌' },
  { key: 'books', label: 'Books', icon: 'bi-book', logo: '📚' },
  { key: 'furniture', label: 'Furniture', icon: 'bi-house', logo: '🛋️' },
  { key: 'groceries', label: 'Groceries', icon: 'bi-basket', logo: '🛒' },
  { key: 'health', label: 'Health', icon: 'bi-heart-pulse', logo: '💊' },
  { key: 'jewelry', label: 'Jewelry', icon: 'bi-gem', logo: '💍' },
];

export default function Sidebar({ onCategorySelect }) {
  return (
    <div className="bg-light p-3" style={{ minWidth: 200 , position: 'fixed', height: '520px', backgroundColor: '#9f9fe6'}}>
      <h5>Categories</h5>
      <ul className="list-unstyled">
        {categories.map(cat => (
          <li key={cat.key}>
            <button className="btn btn-link p-0 d-flex align-items-center" onClick={() => { console.log(`Category selected: ${cat.key}`); onCategorySelect(cat.key); }}>
              <span className={`me-2 bi ${cat.icon}`}></span>
              <span className="me-2" style={{ fontSize: '1.2em' }}>{cat.logo}</span>
              {cat.label}
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
} 