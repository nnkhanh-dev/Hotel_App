// Pure JavaScript Navbar - No Bootstrap or jQuery

document.addEventListener('DOMContentLoaded', function() {
    const mobileMenuToggle = document.getElementById('mobileMenuToggle');
    const mainNav = document.getElementById('mainNav');
    const navLinks = document.querySelectorAll('.nav-link');
    const body = document.body;
    
    // Create overlay for mobile menu
    const overlay = document.createElement('div');
    overlay.className = 'nav-overlay';
    body.appendChild(overlay);
    
    // Toggle mobile menu
    if (mobileMenuToggle) {
        mobileMenuToggle.addEventListener('click', function() {
            mobileMenuToggle.classList.toggle('active');
            mainNav.classList.toggle('active');
            overlay.classList.toggle('active');
            body.style.overflow = mainNav.classList.contains('active') ? 'hidden' : '';
        });
    }
    
    // Close menu when overlay is clicked
    overlay.addEventListener('click', function() {
        mobileMenuToggle.classList.remove('active');
        mainNav.classList.remove('active');
        overlay.classList.remove('active');
        body.style.overflow = '';
    });
    
    // Close menu when nav link is clicked (mobile) - except dropdown links
    navLinks.forEach(function(link) {
        link.addEventListener('click', function(e) {
            // Don't close menu if it's a dropdown toggle
            if (link.parentElement.classList.contains('nav-item-dropdown')) {
                return;
            }
            
            if (window.innerWidth <= 992) {
                mobileMenuToggle.classList.remove('active');
                mainNav.classList.remove('active');
                overlay.classList.remove('active');
                body.style.overflow = '';
            }
        });
    });
    
    // Set active link based on current page
    const currentPath = window.location.pathname.toLowerCase();
    
    navLinks.forEach(function(link) {
        const linkPath = link.getAttribute('href').toLowerCase();
        
        // Remove active class from all links first
        link.classList.remove('active');
        
        // Check if current path matches link path
        if (currentPath === linkPath || 
            currentPath.includes(linkPath) && linkPath !== '/') {
            link.classList.add('active');
        }
        
        // Special case for home page
        if ((currentPath === '/' || currentPath === '/hotel' || currentPath === '/hotel/index') && 
            (linkPath === '/hotel/index' || linkPath === '/')) {
            link.classList.add('active');
        }
    });
    
    // Handle window resize
    let resizeTimer;
    window.addEventListener('resize', function() {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function() {
            if (window.innerWidth > 992) {
                mobileMenuToggle.classList.remove('active');
                mainNav.classList.remove('active');
                overlay.classList.remove('active');
                body.style.overflow = '';
            }
        }, 250);
    });
    
    // Add shadow to header on scroll
    const header = document.querySelector('.main-header');
    window.addEventListener('scroll', function() {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    });
    
    // User dropdown menu toggle (Desktop)
    const userMenuButton = document.getElementById('userMenuButton');
    const userDropdownMenu = document.getElementById('userDropdownMenu');
    
    if (userMenuButton && userDropdownMenu) {
        userMenuButton.addEventListener('click', function(e) {
            e.stopPropagation();
            userMenuButton.classList.toggle('active');
            userDropdownMenu.classList.toggle('show');
        });
        
        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!userMenuButton.contains(e.target) && !userDropdownMenu.contains(e.target)) {
                userMenuButton.classList.remove('active');
                userDropdownMenu.classList.remove('show');
            }
        });
        
        // Close dropdown when clicking on a menu item
        const dropdownItems = userDropdownMenu.querySelectorAll('.dropdown-item');
        dropdownItems.forEach(function(item) {
            item.addEventListener('click', function() {
                userMenuButton.classList.remove('active');
                userDropdownMenu.classList.remove('show');
            });
        });
    }
    
    // Mobile nav dropdown toggle
    const navItemDropdowns = document.querySelectorAll('.nav-item-dropdown');
    
    navItemDropdowns.forEach(function(dropdown) {
        const link = dropdown.querySelector('.nav-link');
        
        if (link) {
            link.addEventListener('click', function(e) {
                e.preventDefault();
                
                // Close other dropdowns
                navItemDropdowns.forEach(function(other) {
                    if (other !== dropdown) {
                        other.classList.remove('active');
                    }
                });
                
                // Toggle current dropdown
                dropdown.classList.toggle('active');
            });
        }
    });
});
