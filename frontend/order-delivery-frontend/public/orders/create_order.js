const { useState } = React;

function CreateOrderForm() {

    const [form, setForm] = useState({
        sendersCity: "",
        sendersAddress: "",
        recipientsCity: "",
        recipientsAddress: "",
        cargoWeight: "",
        cargoCollectionDate: ""
    });

    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState(null);
    const [error, setError] = useState(null);

    function handleChange(e) {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    }

    async function handleSubmit(e) {
        e.preventDefault();

        setLoading(true);
        setError(null);
        setMessage(null);

        try {

            const response = await fetch(
                "http://localhost:5002/v1/orders",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        ...form,
                        cargoWeight: Number(form.cargoWeight),
                        cargoCollectionDate: new Date(form.cargoCollectionDate).toISOString()
                    })
                }
            );

            if (!response.ok) {
                throw new Error("Ошибка создания заказа");
            }

            const data = await response.json();

            setMessage(`Заказ успешно создан. Номер заказа: ${data.order.orderId}`);

            setForm({
                sendersCity: "",
                sendersAddress: "",
                recipientsCity: "",
                recipientsAddress: "",
                cargoWeight: "",
                cargoCollectionDate: ""
            });

        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    return React.createElement(
        "form",
        { className: "order-form", onSubmit: handleSubmit },

        React.createElement("input", {
            name: "sendersCity",
            placeholder: "Город отправителя",
            value: form.sendersCity,
            onChange: handleChange,
            required: true
        }),

        React.createElement("input", {
            name: "sendersAddress",
            placeholder: "Адрес отправителя",
            value: form.sendersAddress,
            onChange: handleChange,
            required: true
        }),

        React.createElement("input", {
            name: "recipientsCity",
            placeholder: "Город получателя",
            value: form.recipientsCity,
            onChange: handleChange,
            required: true
        }),

        React.createElement("input", {
            name: "recipientsAddress",
            placeholder: "Адрес получателя",
            value: form.recipientsAddress,
            onChange: handleChange,
            required: true
        }),

        React.createElement("input", {
            type: "number",
            name: "cargoWeight",
            placeholder: "Вес груза",
            value: form.cargoWeight,
            onChange: handleChange,
            required: true
        }),

        React.createElement("input", {
            type: "date",
            name: "cargoCollectionDate",
            value: form.cargoCollectionDate,
            onChange: handleChange,
            required: true
        }),

        React.createElement(
            "button",
            { type: "submit", disabled: loading },
            loading ? "Создание..." : "Создать заказ"
        ),

        message &&
        React.createElement(
            "div",
            { className: "success-message" },
            message
        ),

        error &&
        React.createElement(
            "div",
            { className: "error-message" },
            error
        )
    );
}

const root = ReactDOM.createRoot(
    document.getElementById("create-order-root")
);

root.render(React.createElement(CreateOrderForm));