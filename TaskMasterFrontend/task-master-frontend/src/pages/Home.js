import './Home.css';
import { useNavigate } from 'react-router-dom';
import { Button } from 'react-bootstrap';
function Home() {
    const navigate = useNavigate(); // Hook to programmatically navigate between routes

    return (
    <div className="App">
      <h1>Welcome to the Home Page!</h1>
        <Button variant="dark" onClick={() => navigate('/about')}>Go to About Page</Button>
        <Button variant="dark" onClick={() => navigate('/login')}>Go to Login Page</Button>
        <Button variant="dark" onClick={() => navigate('/profile')}>Go to Profile Page</Button>
    </div>
  );
}
export default Home;