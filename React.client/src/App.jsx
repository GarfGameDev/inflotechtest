import './App.css';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/navbar";
import Home from "./components/home";
import Users from "./components/users";
import UserCreate from './components/useractions/create';
import UserDetails from './components/useractions/details';
import UserEdit from './components/useractions/edit';

function App() {

    return (
        <Router>
            <Navbar />
            <main className="main-content">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/users" element={<Users />} />
                    <Route path="/users/create" element={<UserCreate />} />
                    <Route path="/users/details/:id" element={<UserDetails />} />
                    <Route path="/users/edit/:id" element={<UserEdit />} />
                </Routes>
            </main>
        </Router>
    );

}

export default App;
