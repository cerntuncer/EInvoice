/*!
    * Start Bootstrap - SB Admin v7.0.7 (https://startbootstrap.com/template/sb-admin)
    * Copyright 2013-2023 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    // 
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

// Global click guard to prevent rapid double-clicks on buttons
(function(){
    const lastClickMap = new WeakMap();
    const DEFAULT_THROTTLE_MS = 700;
    document.addEventListener('click', function(e) {
        const el = e.target.closest('button, input[type="submit"], a.btn');
        if (!el) return;
        if (el.disabled) return;
        const now = Date.now();
        const last = lastClickMap.get(el) || 0;
        const threshold = Number(el.getAttribute('data-guard-ms') || DEFAULT_THROTTLE_MS);
        if (now - last < threshold) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
        lastClickMap.set(el, now);
        if (el.hasAttribute('data-disable-on-click')) {
            el.disabled = true;
            setTimeout(() => { try { el.disabled = false; } catch {} }, threshold);
        }
    }, true);
})();
