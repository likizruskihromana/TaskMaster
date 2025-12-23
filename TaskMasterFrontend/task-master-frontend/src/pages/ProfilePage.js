import './ProfilePage.css';
import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import React, { useState } from 'react';
import api from '../api/API';
function ProfilePage() {
    const navigate = useNavigate(); // Hook to programmatically navigate between routes
    const [userProfile, setUserProfile] = useState(null);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');



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
    React.useEffect(() => {
        fetchUserProfile();
    }, []);
    return (
    <div className="ProfilePage">
      <h1>Welcome to the Profile Page!</h1>
        <Button variant="dark" onClick={() => navigate('/about')}>Go to About Page</Button>
        <Button variant="dark" onClick={() => navigate('/home')}>Go to Login Page</Button>
        <Button variant="dark" onClick={() => navigate('/logout')}>Logout</Button>

        {userProfile && (
          <div style={{marginTop: '20px', padding: '15px', background: '#f8f9fa', borderRadius: '5px'}}>
            <h4>Korisnički profil:</h4>
            <p><strong>ID:</strong> {userProfile.userId}</p>
            <p><strong>Email:</strong> {userProfile.email}</p>
            <p><strong>Ime:</strong> {userProfile.name}</p>
          </div>
        )}
    </div>
  );
}
export default ProfilePage;