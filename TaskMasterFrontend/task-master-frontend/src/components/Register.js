import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import api from '../api/API';
import { useNavigate } from 'react-router-dom';
function Register() {
  const [registerData, setRegisterData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();
  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setMessage('');
    
    if(!registerData.email || !registerData.password || !registerData.firstName || !registerData.lastName) {
      setError('Unesi sva polja!');
      return;
    }

    try {
      // Poziv register endpointa
      const response = await api.post('/api/Auth/register', {
        email: registerData.email,
        password: registerData.password,
        firstName: registerData.firstName,
        lastName: registerData.lastName
      });


      if (response.data.success) {
        // Sačuvaj token u localStorage
        localStorage.setItem('token', response.data.token);
        
        // Postavi token u axios header za sve buduće zahtjeve
        api.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
        
        setMessage('Uspješna prijava!');
        navigate('/profile');
      } else {
        setError(response.data.message || 'Login nije uspio');
      }
    } catch (err) {
      console.error('Login error:', err);
      setError(err.response?.data?.message || 'Greška pri prijavljivanju');
    }
  };
  return (
    <div className="login-wrapper">
      <div className="login-form-container">
        <h2 className="login-title">Register</h2>
        
        {error && <Alert variant="danger">{error}</Alert>}
        {message && <Alert variant="success">{message}</Alert>}
        
        
        <Form onSubmit={handleSubmit} className="login-form">

          <Form.Group className="mb-3" controlId="formFirstName">
            <Form.Label>Ime</Form.Label>
            <Form.Control
              type="text"
              placeholder="Unesi ime"
              value={registerData.firstName}
              onChange={(e) => setRegisterData({...registerData, firstName: e.target.value})}
            />
          </Form.Group>
          <Form.Group className="mb-3" controlId="formFirstName">
            <Form.Label>Prezime</Form.Label>
            <Form.Control
              type="text"
              placeholder="Unesi prezime"
              value={registerData.lastName}
              onChange={(e) => setRegisterData({...registerData, lastName: e.target.value})}
            />
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Email adresa</Form.Label>
            <Form.Control
              type="email"
              placeholder="Unesi email"
              value={registerData.email}
              onChange={(e) => setRegisterData({...registerData, email: e.target.value})}
            />
          </Form.Group>
          
          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Šifra</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={registerData.password}
              onChange={(e) => setRegisterData({...registerData, password: e.target.value})}
            />
          </Form.Group>
          
          <Button variant="primary" type="submit" className="login-button">
            Login
          </Button>
        </Form>

       
      </div>
    </div>
  );
}

export default Register;