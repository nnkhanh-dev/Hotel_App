// Floating Chat Widget JavaScript
(function ($) {
    'use strict';

    let connection = null;
    let isConnected = false;

    $(document).ready(function () {
        initializeChat();
    });

    function initializeChat() {
        // Initialize SignalR connection
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/chathub")
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveMessage", (user, message) => {
            hideTypingIndicator();
            addMessage(user, message);
        });

        connection.start()
            .then(() => {
                console.log('SignalR Connected');
                isConnected = true;
            })
            .catch(err => {
                console.error('SignalR Connection Error:', err);
                isConnected = false;
            });

        // Toggle chat window
        $('.floating-chat-btn').on('click', function () {
            $(this).toggleClass('active');
            $('.floating-chat-window').toggleClass('active');
            
            if ($('.floating-chat-window').hasClass('active')) {
                $('#floatingChatInput').focus();
                $('.chat-notification-badge').remove();
            }
        });

        // Minimize chat
        $('.chat-minimize-btn').on('click', function (e) {
            e.stopPropagation();
            $('.floating-chat-window').removeClass('active');
            $('.floating-chat-btn').removeClass('active');
        });

        // Send message on form submit
        $('#floatingChatForm').on('submit', function (e) {
            e.preventDefault();
            sendMessage();
        });

        // Send message on button click
        $('.chat-send-btn').on('click', function (e) {
            e.preventDefault();
            sendMessage();
        });

        // Enter to send, Shift+Enter for new line
        $('#floatingChatInput').on('keypress', function (e) {
            if (e.which === 13 && !e.shiftKey) {
                e.preventDefault();
                sendMessage();
            }
        });
    }

    function sendMessage() {
        const input = $('#floatingChatInput');
        const content = input.val().trim();

        if (!content) return;

        // Clear input immediately and show typing indicator
        input.val('');
        
        // Disable input while sending
        input.prop('disabled', true);
        $('.chat-send-btn').prop('disabled', true);

        // Show typing indicator immediately
        showTypingIndicator();

        // Send message to server
        fetch("/Chat", {
            method: "POST",
            headers: { "Content-Type": "application/x-www-form-urlencoded" },
            body: `content=${encodeURIComponent(content)}`
        })
        .then(response => {
            if (!response.ok) {
                hideTypingIndicator();
                showError('Không thể gửi tin nhắn. Vui lòng thử lại.');
            }
            // Keep typing indicator showing until AI responds
        })
        .catch(err => {
            console.error('Send message error:', err);
            hideTypingIndicator();
            showError('Lỗi kết nối. Vui lòng kiểm tra mạng.');
        })
        .finally(() => {
            input.prop('disabled', false);
            $('.chat-send-btn').prop('disabled', false);
            input.focus();
        });
    }

    function addMessage(user, message) {
        const chatBody = $('.floating-chat-window .chat-window-body');
        const isAI = user === "UTEHotel AI";
        
        // For AI messages, render HTML directly (for images, formatting, etc.)
        // For user messages, escape HTML for security
        const messageContent = isAI ? message : escapeHtml(message);
        
        const messageHtml = `
            <div class="chat-message-wrapper ${isAI ? 'ai' : 'client'}">
                <div class="message ${isAI ? 'ai-message' : 'client-message'}">${messageContent}</div>
            </div>
        `;
        
        chatBody.append(messageHtml);
        scrollToBottom();

        // Show notification if chat is closed
        if (!$('.floating-chat-window').hasClass('active') && isAI) {
            showNotification();
        }
    }

    function showTypingIndicator() {
        $('.typing-indicator').addClass('active');
        scrollToBottom();
    }

    function hideTypingIndicator() {
        $('.typing-indicator').removeClass('active');
    }

    function scrollToBottom() {
        const chatBody = $('.floating-chat-window .chat-window-body');
        chatBody.animate({ scrollTop: chatBody[0].scrollHeight }, 300);
    }

    function showNotification() {
        if ($('.chat-notification-badge').length === 0) {
            $('.floating-chat-btn').append('<span class="chat-notification-badge">1</span>');
        }
    }

    function showError(message) {
        const errorHtml = `
            <div class="chat-message-wrapper ai">
                <div class="message ai-message" style="background: #fee; color: #c00;">
                    <strong>Lỗi:</strong> ${message}
                </div>
            </div>
        `;
        $('.floating-chat-window .chat-window-body').append(errorHtml);
        scrollToBottom();
    }

    function escapeHtml(text) {
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, m => map[m]);
    }

    // Click on images to view larger
    $(document).on('click', '.ai-message img', function() {
        const imgSrc = $(this).attr('src');
        const imgAlt = $(this).attr('alt') || 'Image';
        
        // Create modal to show image
        const modal = `
            <div class="image-modal" style="position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(0,0,0,0.9); z-index: 10000; display: flex; align-items: center; justify-content: center; cursor: pointer;">
                <div style="position: relative; max-width: 90%; max-height: 90%;">
                    <img src="${imgSrc}" alt="${imgAlt}" style="max-width: 100%; max-height: 90vh; border-radius: 10px; box-shadow: 0 10px 50px rgba(0,0,0,0.5);">
                    <button style="position: absolute; top: -40px; right: 0; background: white; border: none; border-radius: 50%; width: 35px; height: 35px; cursor: pointer; font-size: 20px; line-height: 1;">×</button>
                </div>
            </div>
        `;
        
        $('body').append(modal);
        
        // Close modal on click
        $('.image-modal').on('click', function() {
            $(this).fadeOut(200, function() {
                $(this).remove();
            });
        });
    });

})(jQuery);
