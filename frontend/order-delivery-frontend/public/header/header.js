(function () {
    'use strict';

    const pageLoadStart = performance.now();

    window.addEventListener('load', () => {
        const loadTime = performance.now() - pageLoadStart;
        const footer = document.querySelector('.footer');

        if (footer) {
            const info = document.createElement('p');
            info.classList.add('footer__load-time');
            info.textContent = `Page loaded in ${(loadTime / 1000).toFixed(2)} s`;
            footer.appendChild(info);
        }
    });

    document.addEventListener('DOMContentLoaded', () => {
        const links = document.querySelectorAll('.sidebar__link');
        const currentUrl = window.location.pathname;

        links.forEach(link => {
            const linkUrl = new URL(link.href, window.location.origin);
            const linkPath = linkUrl.pathname;

            const normalizedCurrent = currentUrl.endsWith('/') ? currentUrl.slice(0, -1) : currentUrl;
            const normalizedLink = linkPath.endsWith('/') ? linkPath.slice(0, -1) : linkPath;

            if (normalizedLink === '' || normalizedLink === '/') {
                if (normalizedCurrent === '' || normalizedCurrent === '/') {
                    link.classList.add('sidebar__link_active');
                }
            }
            else if (normalizedCurrent === normalizedLink) {
                link.classList.add('sidebar__link_active');
            }
        });
    });

})();