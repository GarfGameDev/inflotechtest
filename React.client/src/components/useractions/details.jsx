import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';

function Users() {
    const [users, setUsers] = useState();
    const { id } = useParams();
    useEffect(() => {
        populateUserData();
    }, );

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
                    <tr key={users.dateofbirth}>
                        <td>{users.forename}</td>
                        <td>{users.surname}</td>
                        <td>{users.email}</td>
                        <td>{users.isActive.toString()}</td>
                    <td>{users.dateOfBirth.toLocaleString()}</td>
                    <td><Link to={'/users/edit/' + id}>Edit |</Link></td>
                    <td><Link to={'/users/delete/' + id}>Delete |</Link></td>
                    </tr>
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
        const response = await fetch(`https://localhost:7084/api/user/${id}`);
        const data = await response.json();
        setUsers(data);
    }

}

export default Users;
