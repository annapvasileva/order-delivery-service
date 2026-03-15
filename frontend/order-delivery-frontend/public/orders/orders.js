const { useState, useEffect } = React;

function OrdersList() {
    const [orders, setOrders] = useState([]);
    const [selectedOrder, setSelectedOrder] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadOrders();
    }, []);

    const API_BASE =
        window.location.hostname === "localhost"
            ? "http://localhost:5002"
            : "http://order-delivery-service-backend:5002";

    async function loadOrders() {
        try {
            const response = await fetch(
                `${API_BASE}/v1/orders?pageSize=10`
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
        "div",
        { className: "orders" },

        React.createElement(
            "ol",
            { className: "orders__list" },

            orders.map(order =>
                React.createElement(
                    "li",
                    {
                        key: order.orderId,
                        className: "orders__item",
                        onClick: () => setSelectedOrder(order)
                    },
                    `Order #${order.orderId}`
                )
            )
        ),

        selectedOrder &&
        React.createElement(
            "section",
            { className: "order-view" },

            React.createElement(
                "h3", 
                { className: "order-view__title" },
                `Order #${selectedOrder.orderId}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Город отправителя: ${selectedOrder.sendersCity}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Адрес отправителя: ${selectedOrder.sendersAddress}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Город получателя: ${selectedOrder.recipientsCity}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Адрес получателя: ${selectedOrder.recipientsAddress}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Вес груза: ${selectedOrder.cargoWeight}`
            ),

            React.createElement(
                "p",
                { className: "order-view__item" },
                `Дата забора: ${new Date(selectedOrder.cargoCollectionDate).toLocaleDateString()}`
            ),

            React.createElement(
                "button",
                {
                    className: "order-view__button",
                    onClick: () => setSelectedOrder(null)
                },
                "Закрыть"
            )
        )
    );
}

const root = ReactDOM.createRoot(document.getElementById("orders-root"));
root.render(React.createElement(OrdersList));