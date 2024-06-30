import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

function LogDetails() {
    const [logs, setLogs] = useState();
    const { id } = useParams();
    useEffect(() => {
        populateLogData();
    }, );

    const contents = logs === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Date Created</th>
                    <th>Source Context</th>
                    <th>Request Path</th>
                    <th>Request Method</th>
                    <th>Request Id</th>
                    <th>Status Code</th>
                </tr>
            </thead>
            <tbody>
                <tr key={logs.createdAt}>
                    <td>{logs.description}</td>
                    <td>{logs.createdAt}</td>
                    <td>{logs.sourceContext}</td>
                    <td>{logs.requestPath}</td>
                    <td>{logs.requestMethod}</td>
                    <td>{logs.requestId}</td>
                    <td>{logs.statusCode}</td>
                    </tr>
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">User</h1>
            <p>More details on a specific user</p>
            {contents}
        </div>
    );

    async function populateLogData() {
        const response = await fetch(`https://localhost:7084/api/user/logs/${id}`);
        const data = await response.json();
        setLogs(data);
    }

}

export default LogDetails;
