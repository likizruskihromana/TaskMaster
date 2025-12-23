import './About.css';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import api from '../api/API';
import { Button } from 'react-bootstrap';
function About() {
    const navigate = useNavigate();
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const fetchPublicUsers = () => {
        api
            .get("/api/Users/public")
            .then((response) => {
                setMessage(JSON.stringify(response.data.message)); 
                setError(''); 
            })
            .catch((err) => {
                console.error("error", err);
                setError("Greška pri učitavanju podataka: " + err.message);
                
            });
    };
  
    return (
        <div className="App">
            <h1>Welcome to the About Page!</h1>
            <Button variant="dark" onClick={fetchPublicUsers}>Fetch Public Endpoint</Button>
            <Button variant="dark" onClick={() => navigate('/home')}>Go to Home Page</Button>
            
            {error && <p style={{color: 'red'}}>{error}</p>}
            {message && <pre style={{textAlign: 'left', background: '#f4f4f4', padding: '10px', borderRadius: '5px', marginTop: '10px'}}>{message}</pre>}
        </div>
    );
}

export default About;