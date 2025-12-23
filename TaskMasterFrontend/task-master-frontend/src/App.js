import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Home from './pages/Home';
import About from './pages/About';
import LoginPage from './pages/LoginPage';
import ProfilePage from './pages/ProfilePage';
import Logout from './components/Logout';
import ProtectedRoute from './auth/ProtectedRoute';
function App() {
  return (
    // BrowserRouter wraps the application and enables React Router's routing functionality
    
    <BrowserRouter>
      <Routes>
        <Route path="/logout" element={<Logout />} />
        <Route path="/home" element={<Home />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/profile" element={<ProtectedRoute> <ProfilePage /> </ProtectedRoute>} />
        <Route path="/about" element={<About />} />
        <Route path="*" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}
export default App;
