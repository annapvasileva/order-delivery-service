const { useState, useEffect } = React;

function OrdersList() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadOrders();
    }, []);

    async function loadOrders() {
        try {
            const response = await fetch(
                "http://localhost:5002/v1/orders?pageSize=10"
            );

            if (!response.ok) {
                throw new Error("Ошибка загрузки заказов");
            }

            const data = await response.json();

            setOrders(data.orders);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    if (loading) {
        return React.createElement("div", { className: "loader"}, "Loading...");
    }

    if (error) {
        return React.createElement("div", { className: "error-message" }, error);
    }

    return React.createElement(
        "ol",
        { className: "orders__list" },
        orders.map(order =>
            React.createElement(
                "li",
                { key: order.orderId },
                React.createElement(
                    "div",
                    { className: "order-card" },
                    React.createElement("h3", null, `Order #${order.orderId}`),
                    React.createElement("p", null, `Sender: ${order.sendersCity}`),
                    React.createElement("p", null, `Recipient: ${order.recipientsCity}`),
                    React.createElement("p", null, `Weight: ${order.cargoWeight}`),
                    React.createElement(
                        "p",
                        null,
                        `Date: ${new Date(order.cargoCollectionDate).toLocaleDateString()}`
                    )
                )
            )
        )
    );
}

const root = ReactDOM.createRoot(document.getElementById("orders-root"));
root.render(React.createElement(OrdersList));