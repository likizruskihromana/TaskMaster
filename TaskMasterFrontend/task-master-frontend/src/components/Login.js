import React, { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import api from '../api/API';
import { useNavigate } from 'react-router-dom';
function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();
  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setMessage('');
    
    if(!email || !password) {
      setError('Unesi sva polja!');
      return;
    }

    try {
      // Poziv login endpointa
      const response = await api.post('/api/Auth/login', {
        email: email,
        password: password
      });

      console.log('Login response:', response.data);

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
/*
  const [userProfile, setUserProfile] = useState(null);

**
  const fetchUserProfile = async () => {
    try {
      const response = await api.get('/api/Users/profile');
      console.log('Profile:', response.data);
      setUserProfile(response.data);
      setMessage(response.data.message);
    } catch (err) {
      console.error('Profile error:', err);
      setError('Greška pri učitavanju profila: ' + (err.response?.data || err.message));
    }
  };

  **
   {userProfile && (
          <div style={{marginTop: '20px', padding: '15px', background: '#f8f9fa', borderRadius: '5px'}}>
            <h4>Korisnički profil:</h4>
            <p><strong>ID:</strong> {userProfile.userId}</p>
            <p><strong>Email:</strong> {userProfile.email}</p>
            <p><strong>Ime:</strong> {userProfile.name}</p>
          </div>
        )}
*/
  return (
    <div className="login-wrapper">
      <div className="login-form-container">
        <h2 className="login-title">Login</h2>
        
        {error && <Alert variant="danger">{error}</Alert>}
        {message && <Alert variant="success">{message}</Alert>}
        
        <Form onSubmit={handleSubmit} className="login-form">
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Email adresa</Form.Label>
            <Form.Control
              type="email"
              placeholder="Unesi email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </Form.Group>
          
          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Šifra</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
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

export default Login;