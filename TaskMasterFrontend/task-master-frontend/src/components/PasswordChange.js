import { useState } from "react";
import api from '../api/API';
import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';

export default function PasswordChange(){
    const navigate = useNavigate();
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: '',
    });
    
    const handleChangePassword = async () => {
        // Validation: check if passwords match
        if(formData.newPassword !== formData.confirmNewPassword){
            setError('New passwords do not match!');
            setMessage('');
            return;
        }
        
        // Validation: check if all fields are filled
        if(!formData.currentPassword || !formData.newPassword || !formData.confirmNewPassword){
            setError('All fields are required!');
            setMessage('');
            return;
        }
        
        try {
            const response = await api.put('/api/Users/password', formData);
            console.log('Password change response:', response.data);
            
            // Check if the response indicates success
            if(response.data.updatedUser || response.status === 200) {
                setMessage('Password changed successfully!');
                setError('');
                // Clear form
                setFormData({
                    currentPassword: '',
                    newPassword: '',
                    confirmNewPassword: '',
                });
                // Navigate to logout after a short delay
                setTimeout(() => {
                    navigate('/logout');
                }, 1500);
            } else {
                setError(response.data.message || 'Failed to change password');
                setMessage('');
            }
        } catch(err) {
            console.error('Change password error:', err);
            setError(err.response?.data?.message || err.response?.data || 'Failed to change password. Please try again.');
            setMessage('');
        }
    };

    return(
        <div>            
            <Button variant="dark" onClick={() => navigate('/profile')}>Go to Profile Page</Button>
            <div className="PasswordChange" style={{marginTop: '20px', padding: '200px', background: '#f8f9fa', borderRadius: '5px', display: 'flex', flexDirection: 'column', gap: '10px'}}>
                <h4>Password Change Component</h4>
                <div style={{display: 'flex', flexDirection: 'column', gap: '5px'}}>
                    <label htmlFor="currentPassword"><strong>Current Password:</strong></label>
                    <input 
                        id="currentPassword"
                        className="form-control" 
                        type="text" 
                        placeholder="Current Password" 
                        value={formData.currentPassword} 
                        onChange={(e) => setFormData({...formData, currentPassword: e.target.value})} 
                    />
                </div>
                
                <div style={{display: 'flex', flexDirection: 'column', gap: '5px'}}>
                    <label htmlFor="newPassword"><strong>New Password:</strong></label>
                    <input 
                        id="newPassword"
                        className="form-control" 
                        type="text" 
                        placeholder="New Password" 
                        value={formData.newPassword} 
                        onChange={(e) => setFormData({...formData, newPassword: e.target.value})} 
                    />
                </div>
                
                <div style={{display: 'flex', flexDirection: 'column', gap: '5px'}}>
                    <label htmlFor="confirmNewPassword"><strong>Confirm New Password:</strong></label>
                    <input 
                        id="confirmNewPassword"
                        className="form-control" 
                        type="text" 
                        placeholder="Confirm New Password" 
                        value={formData.confirmNewPassword} 
                        onChange={(e) => setFormData({...formData, confirmNewPassword: e.target.value})} 
                    />
                </div>
                
                <Button variant="dark" onClick={handleChangePassword} style={{marginTop: '10px'}}>
                    Sačuvaj promjene
                </Button>
                
                {message && (
                    <div className="alert alert-success" style={{marginTop: '10px'}}>
                        {message}
                    </div>
                )}
                {error && (
                    <div className="alert alert-danger" style={{marginTop: '10px'}}>
                        {error}
                    </div>
                )}
            </div>
        </div>
       
    );
}