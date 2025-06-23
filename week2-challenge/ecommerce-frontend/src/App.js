import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Header from './components/common/Header';
import Footer from './components/common/Footer';
import Login from './components/pages/Login';
import Register from './components/pages/Register';
import MainPage from './components/pages/MainPage';
import Payment from './components/Payment/Payment';
import Cart from './components/Cart/Cart';
import PaymentSuccess from './components/PaymentSucess/PaymentSuccess';
import PaymentFailure from './components/PaymentFailure/PaymentFailure';
import { isLoggedIn } from './utils/auth';
import { useCart, CartProvider } from './store/CartContext';
import { UserProvider } from './store/UserContext';

function ProtectedRoute({ children }) {
  if (!isLoggedIn()) {
    return <Navigate to="/login" />;
  }
  return children;
}

function App() {
  return (
    <UserProvider>
      <CartProvider>
        <Router>
          <div className="d-flex flex-column min-vh-100">
            <Header />
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/main" element={<MainPage />} />
              <Route path="/" element={<MainPage />} />
              <Route path="/cart" element={<Cart />} />
              <Route path="/payment" element={<ProtectedRoute><Payment /></ProtectedRoute>} />
              <Route path="/payment-success" element={<PaymentSuccess />} />
              <Route path="/payment-failure" element={<PaymentFailure />} />
            </Routes>
            <Footer />
          </div>
        </Router>
      </CartProvider>
    </UserProvider>
  );
}

export default App;
