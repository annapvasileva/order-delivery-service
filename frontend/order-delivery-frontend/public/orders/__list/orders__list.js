document.addEventListener("DOMContentLoaded", () => {
    const listEl = document.getElementById("orders-list");
    const template = document.getElementById("order-template").content;

    loadRecipes();

    async function loadRecipes() {
        showLoader();

        if (!navigator.onLine) {
            showError("Нет подключения к интернету.");
            hideLoader();
            return;
        }

        const randomUser = Math.floor(Math.random() * 10) + 1;
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 5000);

        try {
            const response = await fetch(
                `https://jsonplaceholder.typicode.com/posts?userId=${randomUser}`,
                { signal: controller.signal }
            );

            clearTimeout(timeoutId);

            if (!response.ok) {
                throw new Error("API is unavailable");
            }

            const recipes = await response.json();

            if (recipes.length === 0) {
                throw new Error("No recipes in this list.");
            }

            await new Promise(resolve => setTimeout(resolve, 1000));

            renderRecipes(recipes.slice(0, 5));

        } catch (err) {
            showError(err.message);
        } finally {
            hideLoader();
        }
    }

    function showLoader() {
        listEl.innerHTML = "";
        const loaderEl = document.getElementById("loader");
        listEl.appendChild(loaderEl);
    }

    function hideLoader() {
        listEl.querySelectorAll(".loader").forEach(el => el.remove());
    }

    function showError(message) {
        listEl.querySelectorAll(".error-message, .loader").forEach(el => el.remove());

        const error = document.createElement("div");
        error.className = "error-message";
        error.textContent = message;
        listEl.appendChild(error);
    }

    function renderRecipes(recipes) {
        listEl.innerHTML = "";

        recipes.forEach(recipe => {
            const clone = template.cloneNode(true);
            const img = clone.querySelector(".order-card__image");
            const title = clone.querySelector(".order-card__title");

            title.textContent = recipe.title;
            img.src = `https://picsum.photos/seed/${recipe.id}/100`;
            img.alt = recipe.title;

            listEl.appendChild(clone);
        });
    }
});