const { useState, useEffect } = React;

function OrdersList() {
    const [orders, setOrders] = useState([]);
    const [selectedOrder, setSelectedOrder] = useState(null);
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
        "div",
        null,

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

            React.createElement("h3", null, `Order #${selectedOrder.orderId}`),

            React.createElement("p", null,
                `Город отправителя: ${selectedOrder.sendersCity}`
            ),

            React.createElement("p", null,
                `Адрес отправителя: ${selectedOrder.sendersAddress}`
            ),

            React.createElement("p", null,
                `Город получателя: ${selectedOrder.recipientsCity}`
            ),

            React.createElement("p", null,
                `Адрес получателя: ${selectedOrder.recipientsAddress}`
            ),

            React.createElement("p", null,
                `Вес груза: ${selectedOrder.cargoWeight}`
            ),

            React.createElement(
                "p",
                null,
                `Дата забора: ${new Date(selectedOrder.cargoCollectionDate).toLocaleDateString()}`
            ),

            React.createElement(
                "button",
                {
                    onClick: () => setSelectedOrder(null)
                },
                "Закрыть"
            )
        )
    );
}

const root = ReactDOM.createRoot(document.getElementById("orders-root"));
root.render(React.createElement(OrdersList));