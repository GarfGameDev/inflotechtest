import { useState } from "react";

function CreateUser() {
    const [formData, setFormData] = useState({
        forename: "",
        surname: "",
        email: "",
        isactive: false,
        dateofbirth: "",
        errors: {},
    });

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prevState) => ({ ...prevState, [name]: value }));
    };

    const handleToggle = ({ target }) =>
        setFormData(s => ({ ...s, [target.name]: !s[target.name] }));

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

        setFormData((prevState) => ({ ...prevState, errors }));

        // Return true if there are no errors
        return Object.keys(errors).length === 0;
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (validateForm()) {
            // Form is valid, submit data
            console.log(JSON.stringify(formData));
            fetch('https://localhost:7084/api/user/create', {
                method: 'post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(
                    formData
                )
            });
        } else {
            // Form is invalid, do nothing
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Forename:
                <input
                    type="text"
                    name="forename"
                    value={formData.forename}
                    onChange={handleChange}
                />
                {formData.errors.forename && (
                    <p style={{ color: "red" }}>{formData.errors.forename}</p>
                )}
            </label>
            <label>
                Surname:
                <input
                    type="text"
                    name="surname"
                    value={formData.surname}
                    onChange={handleChange}
                />
                {formData.errors.surname && (
                    <p style={{ color: "red" }}>{formData.errors.surname}</p>
                )}
            </label>
            <label>
                Email:
                <input
                    type="text"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                />
                {formData.errors.email && (
                    <p style={{ color: "red" }}>{formData.errors.email}</p>
                )}
            </label>
            <label>
                Is Active:
                <input
                    type="checkbox"
                    name="isactive"
                    checked={formData.isactive}
                    onChange={handleToggle}
                />
            </label>
            <label>
                Date Of Birth:
                <input
                    type="date"
                    name="dateofbirth"
                    value={formData.dateofbirth}
                    onChange={handleChange}
                    />
            </label>
            <input type="submit" value="Submit" />
        </form>
    );
}

export default CreateUser;
