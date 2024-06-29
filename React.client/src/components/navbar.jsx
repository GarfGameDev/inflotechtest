import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import "./navbar.css";

const Navbars = () => {
    return (
        <>
            <header>
            <Navbar bg="primary" data-bs-theme="dark" fixed="top">
                <Container>
                    <Navbar.Brand href="/">Navbar</Navbar.Brand>
                    <Nav className="me-auto">
                            <Nav.Link href="/">Home</Nav.Link>
                            <NavDropdown title="Users" id="basic-nav-dropdown">
                                <NavDropdown.Item href="users">View Users</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item href="users/create">
                                    Create User
                                </NavDropdown.Item>
                            </NavDropdown>
                    </Nav>
                </Container>
            </Navbar>
            </header>
        </>
    );
};

export default Navbars;
