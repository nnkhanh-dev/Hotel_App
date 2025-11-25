// Client Navbar JavaScript
(function ($) {
    'use strict';

    $(document).ready(function () {
        
        // Ensure dropdown stays within viewport
        $('.user-dropdown').on('show.bs.dropdown', function () {
            var $dropdown = $(this);
            var $menu = $dropdown.find('.dropdown-menu');
            
            // Reset any inline styles
            $menu.css({
                'left': '',
                'right': '',
                'transform': ''
            });

            // Wait for dropdown to be shown
            setTimeout(function() {
                var menuWidth = $menu.outerWidth();
                var menuOffset = $menu.offset();
                var windowWidth = $(window).width();
                
                // Check if dropdown goes off right edge
                if (menuOffset && menuOffset.left + menuWidth > windowWidth) {
                    var adjustedRight = windowWidth - menuOffset.left - menuWidth - 10;
                    $menu.css({
                        'right': '0',
                        'left': 'auto'
                    });
                }
            }, 10);
        });

        // Close dropdown when clicking outside
        $(document).on('click', function(e) {
            if (!$(e.target).closest('.user-dropdown').length) {
                $('.user-dropdown .dropdown-menu').removeClass('show');
                $('.user-dropdown .btn-user-menu').attr('aria-expanded', 'false');
            }
        });

        // Prevent dropdown from closing when clicking inside
        $('.user-dropdown .dropdown-menu').on('click', function(e) {
            e.stopPropagation();
        });

        // Close dropdown on link click
        $('.user-dropdown .dropdown-item').on('click', function() {
            $('.user-dropdown .dropdown-menu').removeClass('show');
        });

    });

})(jQuery);
