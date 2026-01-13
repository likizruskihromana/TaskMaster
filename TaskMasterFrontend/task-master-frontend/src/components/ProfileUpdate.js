import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import React, { useState } from 'react';
import api from '../api/API';
import './ProfileUpdate.css';
export default function ProfileUpdate(){
    const navigate = useNavigate(); // Hook to programmatically navigate between routes
    const [userProfile, setUserProfile] = useState(null);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        avatar: ''
    });

    const fetchUserProfile = async () => {
        try {
        const response = await api.get('/api/Users/profile');
        console.log('Profile:', response.data);
        setUserProfile(response.data.user);
        setMessage(response.data.message);
         // Initialize form data with fetched profile data
        setFormData({
                firstName: response.data.user.firstName || '',
                lastName: response.data.user.lastName || '',
                email: response.data.user.email || '',
                avatar: response.data.user.avatar || ''
            });
        } catch (err) {
        console.error('Profile error:', err);
        setError('Greška pri učitavanju profila: ' + (err.response?.data || err.message));
        }
    };
    React.useEffect(() => {
        fetchUserProfile();
    }, []);
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

   const handleSaveChanges = async () => {
        try {
            const response = await api.put('/api/Users/profile', formData);
            console.log('Profile updated:', response.data);
            setMessage('Profil uspješno ažuriran!');
            setError('');
            // Optionally refresh the profile data
            fetchUserProfile();
            navigate('/profile');
        } catch(err) {
            console.error('Profile update error:', err);
            setError('Greška pri ažuriranju profila: ' + (err.response?.data || err.message));
            setMessage('');
        }
    };
    return (
        <div className='ProfileUpdate'>
            <Button variant="dark" onClick={() => navigate('/profile')}>Go to Profile Page</Button>
            {userProfile && (
            <div style={{marginTop: '20px', padding: '200px', background: '#f8f9fa', borderRadius: '5px', display: 'flex', flexDirection: 'column', gap: '10px'}}>
                <h4>Korisnički profil:</h4>
                <p style={{display: 'flex', flexDirection: 'row', alignItems: 'center', gap: '10px'}}>
                    <strong>Email:</strong> 
                    <input type="email" className="form-control" onChange={handleInputChange} name="email" id="emailAddress" placeholder={userProfile.email} required></input>
                </p>
                <p style={{display: 'flex', flexDirection: 'row', alignItems: 'center', gap: '10px'}}>
                    <strong>Ime:</strong>
                    <input type="text" className="form-control" onChange={handleInputChange} name="firstName" id="firstName" placeholder={userProfile.firstName}></input>
                </p>
                <p style={{display: 'flex', flexDirection: 'row', alignItems: 'center', gap: '10px'}}>
                    <strong>Prezime:</strong>
                    <input type="text" className="form-control" onChange={handleInputChange} name="lastName" id="lastName" placeholder={userProfile.lastName}></input>
                </p>
                <p style={{display: 'flex', flexDirection: 'row', alignItems: 'center', gap: '10px'}}>
                    <strong>Avatar:</strong>
                    <input type="text" className="form-control" onChange={handleInputChange} name="avatar" id="avatar" placeholder={userProfile.avatar}></input>
                </p>
                <Button variant="dark" onClick={handleSaveChanges} style={{marginTop: '10px'}}>Sačuvaj promjene</Button>
            </div>
            )}
        </div>
    );
}