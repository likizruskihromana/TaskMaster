import React from 'react';
import { Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import api from '../api/API';

function Logout() {
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await api.post('/api/Auth/logout');
    } catch (err) {
      console.error('Logout error:', err);
    } finally {
      localStorage.removeItem('token');
      delete api.defaults.headers.common['Authorization'];
      navigate('/login');
    }
  };

  return (
    <Button variant="outline-danger" onClick={handleLogout}>
      Logout
    </Button>   
  );
}

export default Logout;