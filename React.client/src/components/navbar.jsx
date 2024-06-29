import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
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
                            <Nav.Link href="users">News</Nav.Link>
                    </Nav>
                </Container>
            </Navbar>
            </header>
        </>
    );
};

export default Navbars;
