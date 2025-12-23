import './LoginPage.css';
import { useNavigate } from 'react-router-dom';
import Login from '../components/Login';
import { Button } from 'react-bootstrap';
function LoginPage() {
    const navigate = useNavigate(); // Hook to programmatically navigate between routes

  return (

    <div className="LoginPage">
        <h1>Welcome to the Login Page!</h1>
        <Button variant="dark" onClick={() => navigate('/home')}>Go to Home Page</Button>
        <Login/>
    </div>
  );
}
export default LoginPage;