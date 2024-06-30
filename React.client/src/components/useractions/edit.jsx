import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

function CreateUser() {
    const { id } = useParams();


    const [formData, setFormData] = useState({
        id: id,
        forename: "",
        surname: "",
        email: "",
        isactive: false,
        dateofbirth: "",
    });

    const [formDatas, setFormDatas] = useState({
        id: id,
        forename: "",
        surname: "",
        email: "",
        isactive: false,
        dateofbirth: "",
    });
    useEffect(() => {
        populateUserData();
    }, );
    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormDatas((prevState) => ({ ...prevState, [name]: value }));
    };

    const handleToggle = ({ target }) =>
        setFormDatas(s => ({ ...s, [target.name]: !s[target.name] }));

    const validateForm = () => {
        const errors = {};

        // Check if forename is empty
        if (!formData.forename) {
            errors.forename = "Forename is required";
        }

        // Check if surname is empty
        if (!formData.surname) {
            errors.surname = "Surname is required";
        }

        // Check if email is empty
        if (!formData.email) {
            errors.email = "email is required";
        }

        setFormDatas((prevState) => ({ ...prevState, errors }));

        // Return true if there are no errors
        return Object.keys(errors).length === 0;
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (validateForm()) {
            // Form is valid, submit data
            console.log(JSON.stringify(formDatas));
            fetch(`https://localhost:7084/api/user/${id}`, {
                method: 'put',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(
                    formDatas
                )
            });
        } else {
            // Form is invalid, do nothing
        }
    };

    return (
        <div>
        <form onSubmit={handleSubmit}>
            <label>
                Forename:
                <input
                    type="text"
                    name="forename"
                        defaultValue={formData.forename || ''}
                    onChange={handleChange}
                />
            </label>
            <label>
                Surname:
                <input
                    type="text"
                        name="surname"
                        defaultValue={formData.surname || ''}
                    onChange={handleChange}
                />
            </label>
            <label>
                Email:
                <input
                    type="text"
                    name="email"
                        defaultValue={formData.email || ''}
                    onChange={handleChange}
                />
            </label>
            <label>
                Is Active:
                <input
                    type="checkbox"
                        name="isactive"
                        defaultChecked={formData.isactive || ''}
                    onChange={handleToggle}
                />
            </label>
            <label>
                Date Of Birth:
                <input
                    type="date"
                    name="dateofbirth"
                        defaultValue={formData.dateofbirth || ''}
                    onChange={handleChange}
                />
            </label>
            <input type="submit" value="Submit" />
            </form>
        </div>
    );

    async function populateUserData() {
        const response = await fetch(`https://localhost:7084/api/user/${id}`);
        const data = await response.json();
        setFormData({
            forename: data.forename,
            surname: data.surname,
            email: data.email,
            isactive: data.isActive,
            dateofbirth: data.dateOfBirth
        });
    }
}

export default CreateUser;
