import { useEffect, useState } from 'react';
import ReactPaginate from 'react-paginate';
import { Link } from 'react-router-dom';

function Logs() {
    const [logs, setLogs] = useState([]);

    const [currentPage, setCurrentPage] = useState(0);
    const [totalPages, setTotalPages] = useState(0);

    const itemsPerPage = 10

    useEffect(() => {
        populateLogsData();
    }, []);

    const startIndex = currentPage * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const subset = logs.slice(startIndex, endIndex);

    const handlePageChange = (selectedPage) => {
        setCurrentPage(selectedPage.selected);
    };

    const contents = subset === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Date Created</th>
                </tr>
            </thead>
            <tbody>
                {subset.map(log =>
                    <tr key={log.createdAt}>
                        <td>{log.description}</td>
                        <td>{log.createdAt}</td>
                        <td><Link to={'/logs/' + log.createdAt}>Details |</Link></td>
                    </tr>
                )}

            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Logs</h1>
            <p>A list of logs from the backend</p>
            {contents}
            <ReactPaginate
                pageCount={totalPages}
                onPageChange={handlePageChange}
                forcePage={currentPage}
                breakClassName={'page-item'}
                breakLinkClassName={'page-link'}
                containerClassName={'pagination'}
                pageClassName={'page-item'}
                pageLinkClassName={'page-link'}
                previousClassName={'page-item'}
                previousLinkClassName={'page-link'}
                nextClassName={'page-item'}
                nextLinkClassName={'page-link'}
                activeClassName={'active'}
            />
        </div>
    );

    async function populateLogsData() {
        const response = await fetch('https://localhost:7084/api/user/logs');
        const data = await response.json();
        setLogs(data);
        setTotalPages(Math.ceil(data.length / itemsPerPage));
    }

}


export default Logs;
