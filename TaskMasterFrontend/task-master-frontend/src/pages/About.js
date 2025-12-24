import './About.css';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import api from '../api/API';
import { Button } from 'react-bootstrap';
function About() {
    const navigate = useNavigate();
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const [users, setUsers] = useState('');

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
      const fetchAllUsers = () => {
        api
            .get("/api/Users/all")
            .then((response) => {
                setMessage(JSON.stringify(response.data.message)); 
                setUsers(JSON.stringify(response.data.users)); 
                setError(''); 
            })
            .catch((err) => {
                console.error("error", err);
                setError("Greška pri učitavanju podataka: " + err.message);
                
            });
    };
const printUsers = (users) => {
    const parsedUsers = JSON.parse(users);
    return parsedUsers.map((user, index) => (
        <div key={user.id ?? index} style={{marginBottom: '15px', padding: '10px', border: '1px solid #ddd', borderRadius: '5px', textAlign: 'left'}}>
            <strong style={{fontSize: '16px'}}>{index + 1}. {user.firstName} {user.lastName}</strong>

            <div style={{display: 'flex', gap: '12px', alignItems: 'center', marginTop: '8px'}}>
                {user.avatar
                    ? <img src={user.avatar} alt="avatar" style={{width: 64, height: 64, objectFit: 'cover', borderRadius: 6, border: '1px solid #ccc'}} />
                    : <div style={{width: 64, height: 64, display:'flex', alignItems:'center', justifyContent:'center', background:'#f0f0f0', borderRadius:6, color:'#666'}}>No avatar</div>
                }
                <div style={{flex: 1}}>
                    <p style={{margin: '4px 0'}}>📧 Email: {user.email}</p>
                    <p style={{margin: '4px 0'}}>👤 Username: {user.userName}</p>
                    <p style={{margin: '4px 0'}}>🔤 Normalized Username: {user.normalizedUserName}</p>
                    <p style={{margin: '4px 0'}}>✉️ Normalized Email: {user.normalizedEmail}</p>
                </div>
            </div>

            <hr style={{margin: '10px 0', border: 'none', borderTop: '1px solid #eee'}} />

            <p style={{margin: '4px 0'}}>📅 Created: {user.createdAt ? new Date(user.createdAt).toLocaleString() : 'N/A'}</p>
            <p style={{margin: '4px 0'}}>🕒 Last Login: {user.lastLoginAt ? new Date(user.lastLoginAt).toLocaleString() : 'Never'}</p>
            <p style={{margin: '4px 0'}}>↩️ Last Logout: {user.lastLogoutAt ? new Date(user.lastLogoutAt).toLocaleString() : 'N/A'}</p>

            <p style={{margin: '4px 0'}}>🔑 ID: {user.id}</p>
            <p style={{margin: '4px 0'}}>🔒 Lockout End: {user.lockoutEnd ? new Date(user.lockoutEnd).toLocaleString() : 'N/A'}</p>
            <p style={{margin: '4px 0'}}>🔓 Lockout Enabled: {user.lockoutEnabled ? 'Yes' : 'No'}</p>
            <p style={{margin: '4px 0'}}>⚠️ Access Failed Count: {user.accessFailedCount}</p>

            <hr style={{margin: '10px 0', border: 'none', borderTop: '1px solid #eee'}} />

            <p style={{margin: '4px 0'}}>📞 Phone: {user.phoneNumber ?? 'N/A'}</p>
            <p style={{margin: '4px 0'}}>📲 Phone Confirmed: {user.phoneNumberConfirmed ? 'Yes' : 'No'}</p>
            <p style={{margin: '4px 0'}}>✅ Email Confirmed: {user.emailConfirmed ? 'Yes' : 'No'}</p>
            <p style={{margin: '4px 0'}}>🔐 2FA Enabled: {user.twoFactorEnabled ? 'Yes' : 'No'}</p>

            <hr style={{margin: '10px 0', border: 'none', borderTop: '1px solid #eee'}} />

            <p style={{margin: '4px 0'}}>📁 Owned Projects ({user.ownedProjects?.length ?? 0}): {user.ownedProjects && user.ownedProjects.length ? JSON.stringify(user.ownedProjects, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>📝 Created Tasks ({user.createdTasks?.length ?? 0}): {user.createdTasks && user.createdTasks.length ? JSON.stringify(user.createdTasks, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>📌 Assigned Tasks ({user.assignedTasks?.length ?? 0}): {user.assignedTasks && user.assignedTasks.length ? JSON.stringify(user.assignedTasks, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>💬 Comments ({user.comments?.length ?? 0}): {user.comments && user.comments.length ? JSON.stringify(user.comments, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>👥 Project Memberships ({user.projectMemberships?.length ?? 0}): {user.projectMemberships && user.projectMemberships.length ? JSON.stringify(user.projectMemberships, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>📎 Attachments ({user.attachments?.length ?? 0}): {user.attachments && user.attachments.length ? JSON.stringify(user.attachments, null, 2) : '[]'}</p>
            <p style={{margin: '4px 0'}}>🧾 Activity Logs ({user.activityLogs?.length ?? 0}): {user.activityLogs && user.activityLogs.length ? JSON.stringify(user.activityLogs, null, 2) : '[]'}</p>

            <hr style={{margin: '10px 0', border: 'none', borderTop: '1px solid #eee'}} />

            <p style={{margin: '4px 0'}}>🔐 Password Hash: {user.passwordHash}</p>
            <p style={{margin: '4px 0'}}>🛡️ Security Stamp: {user.securityStamp}</p>
            <p style={{margin: '4px 0'}}>🔁 Concurrency Stamp: {user.concurrencyStamp}</p>
        </div>
    ));
}

    return (
        <div className="App">
            <h1>Welcome to the About Page!</h1>
            <Button variant="dark" onClick={fetchPublicUsers}>Fetch Public Endpoint</Button>
            <Button variant="dark" onClick={() => navigate('/home')}>Go to Home Page</Button>
            <Button variant="dark" onClick={fetchAllUsers}>Fetch All Users Endpoint</Button>
            {error && <p style={{color: 'red'}}>{error}</p>}
            {message && <pre style={{textAlign: 'left', background: '#f4f4f4', padding: '10px', borderRadius: '5px', marginTop: '10px'}}>{message}</pre>}
            {users && <pre style={{textAlign: 'left', background: '#f4f4f4', padding: '10px', borderRadius: '5px', marginTop: '10px'}}>{printUsers(users)}</pre>}

        </div>
    );
}

export default About;