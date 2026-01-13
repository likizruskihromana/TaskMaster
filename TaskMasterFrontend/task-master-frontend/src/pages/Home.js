import './Home.css';
import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import Logout from '../components/Logout';
function Home() {
    const navigate = useNavigate(); // Hook to programmatically navigate between routes
    const token = localStorage.getItem('token');
  
    return (
    <div className="App">
      <h1>Welcome to the Home Page!</h1>
        <Button variant="dark" onClick={() => navigate('/about')}>Go to About Page</Button>
        <Button variant="dark" onClick={() => navigate('/login')}>Go to Login Page</Button>
        {token && <Button variant="dark" onClick={() => navigate('/profile')}>Go to Profile Page</Button>}
        <Button variant="dark" onClick={() => navigate('/register')}>Go to Register Page</Button>
        <Logout/>
    </div>
  );
}
export default Home;