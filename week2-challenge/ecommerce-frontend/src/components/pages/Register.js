import React, { useState } from 'react';
import axios from '../../services/api';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';

export default function Register() {
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: { username: '', email: '', passwordHash: '' },
    validationSchema: Yup.object({
      username: Yup.string().required('Required'),
      email: Yup.string().email('Invalid email').required('Required'),
      passwordHash: Yup.string().min(6, 'Min 6 chars').required('Required'),
    }),
    onSubmit: async (values) => {
      try {
        const response = await axios.post('http://localhost:5031/api/auth/register', values);
        console.log("Registration successful", response);
        navigate('/login');
      } catch (err) {
        setError('Registration failed');
      }
    }
  });

  return (
    <div className="container mt-5" style={{ maxWidth: 400 }}>
      <h2>Register</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={formik.handleSubmit}>
        <input className="form-control mb-2" name="username" placeholder="Username" onChange={formik.handleChange} onBlur={formik.handleBlur} value={formik.values.username} required />
        <input className="form-control mb-2" name="email" placeholder="Email" onChange={formik.handleChange} onBlur={formik.handleBlur} value={formik.values.email} required />
        <input className="form-control mb-2" name="passwordHash" type="password" placeholder="Password" onChange={formik.handleChange} onBlur={formik.handleBlur} value={formik.values.passwordHash} required />
        <button className="btn btn-primary w-100" type="submit" disabled={!formik.isValid}>Register</button>
      </form>
    </div>
  );
} 