import { useNavigate } from 'react-router-dom';
import Register from '../components/Register';
import { Button } from 'react-bootstrap';
function RegisterPage() {
    const navigate = useNavigate(); // Hook to programmatically navigate between routes
  return (
    <div className="RegisterPage">
        <h1>Welcome to the Register Page!</h1>
        <Button variant="dark" onClick={() => navigate('/home')}>Go to Home Page</Button>
        <Register/>
    </div>
  );
}
export default RegisterPage;