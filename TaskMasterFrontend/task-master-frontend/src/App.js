import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Home from './pages/Home';
import About from './pages/About';
import LoginPage from './pages/LoginPage';
import ProfilePage from './pages/ProfilePage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './auth/ProtectedRoute';
import Logout from './components/Logout';
import UpdateProfile from './components/ProfileUpdate';
import PasswordChange from './components/PasswordChange';
function App() {
  return (
    // BrowserRouter wraps the application and enables React Router's routing functionality
    <BrowserRouter>
      <Routes>
        <Route path="/logout" element={<Logout />} />
        <Route path="/home" element={<Home />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/password-change" element={<ProtectedRoute> <PasswordChange /> </ProtectedRoute>} />
        <Route path="/update-profile" element={<ProtectedRoute> <UpdateProfile /> </ProtectedRoute>} />
        <Route path="/profile" element={<ProtectedRoute> <ProfilePage /> </ProtectedRoute>} />
        <Route path="/about" element={<About />} />
        <Route path="*" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}
export default App;
