import './App.css';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/navbar";
import Home from "./components/home";
import Users from "./components/users";

function App() {

    return (
        <Router>
            <Navbar />
            <main className="main-content">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/users" element={<Users/>} />
                </Routes>
            </main>
        </Router>
    );

}

export default App;
