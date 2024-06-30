import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'

function Users() {
    const [users, setUsers] = useState();

    useEffect(() => {
        populateUserData();
    }, []);

    const contents = users === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Firstname</th>
                    <th>Surname</th>
                    <th>Email</th>
                    <th>Is Active?</th>
                    <th>Date Of Birth</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user =>
                    <tr key={user.dateofbirth}>
                        <td>{user.forename}</td>
                        <td>{user.surname}</td>
                        <td>{user.email}</td>
                        <td>{user.isActive.toString()}</td>
                        <td>{user.dateOfBirth.toLocaleString()}</td>
                        <td><Link to={'/users/details/' + user.id}>Details | </Link></td>
                        <td><Link to={'/users/edit/' + user.id}>Edit | </Link></td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">React</h1>
            <p>I am a working React frontend</p>
            {contents}
        </div>
    );

    async function populateUserData() {
        const response = await fetch('https://localhost:7084/api/user/all');
        const data = await response.json();
        setUsers(data);
    }

}

export default Users;
