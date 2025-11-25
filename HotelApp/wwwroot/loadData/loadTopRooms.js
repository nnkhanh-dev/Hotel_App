$(document).ready(function () {
    loadTopRooms();
});

function loadTopRooms() {
    $.ajax({
        url: "/GetListTopRoom",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                let roomHtmlContent = '';

                // Duyệt qua danh sách top 20 phòng
                response.data.forEach(room => {
                    // Lấy ảnh đầu tiên hoặc ảnh mặc định
                    let imageUrl = room.imageUrls && room.imageUrls.length > 0 
                        ? room.imageUrls[0] 
                        : '/images/default-room.jpg';

                    // Tính giá sau giảm giá
                    let finalPrice = room.price - (room.discount * room.price);

                    roomHtmlContent += `
                        <div class="item" style="height: auto; padding: 10px;">
                            <div id="serv_hover" class="room" style="width: 100%; height: 514px; cursor: pointer; box-shadow: 0 4px 8px rgba(0,0,0,0.1); border-radius: 8px; overflow: hidden; display: flex; flex-direction: column; background: white;">
                                <div class="room_img" style="width: 100%; height: 257px; overflow: hidden; flex-shrink: 0;">
                                    <img src="${imageUrl}" alt="${room.typeName}" style="width: 100%; height: 100%; object-fit: cover; display: block;" />
                                </div>
                                <div class="bed_room" style="padding: 15px; height: 257px; display: flex; flex-direction: column; overflow: hidden;">
                                    <h3 style="margin: 0 0 8px 0; font-size: 16px; line-height: 1.3; height: 42px; overflow: hidden; display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical;">${room.typeName}</h3>
                                    <p style="margin: 0 0 4px 0; font-size: 13px;"><i class="fa fa-map-marker" aria-hidden="true"></i> ${room.areaName}</p>
                                    <p style="margin: 0 0 8px 0; font-size: 13px;"><i class="fa fa-star" aria-hidden="true"></i> Đã đặt: <strong>${room.bookingCount}</strong> lần</p>
                                    <div style="height: 40px; margin-bottom: 8px;">
                                        ${room.discount > 0 ? 
                                            `<p style="text-decoration: line-through; color: gray; margin: 0 0 4px 0; font-size: 13px;">${room.price.toLocaleString('vi-VN')}đ</p>
                                             <p style="color: red; font-weight: bold; margin: 0; font-size: 13px;">Giảm ${(room.discount * 100).toFixed(0)}%</p>` 
                                            : ''}
                                    </div>
                                    <h4 style="color: red; font-weight: bold; margin: 0 0 12px 0; font-size: 18px;">${finalPrice.toLocaleString('vi-VN')}đ/đêm</h4>
                                    <button class="btn btn-primary w-100" style="margin-top: auto; padding: 8px; font-size: 13px;" onclick="viewTopRoomDetail(${room.id})">
                                        <i class="fa fa-info-circle" aria-hidden="true"></i> Xem chi tiết
                                    </button>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listTopRooms').html(roomHtmlContent);

                // Khởi tạo Owl Carousel
                $('#listTopRooms').owlCarousel({
                    items: 1,
                    margin: 0,
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    autoHeight: false,
                    nav: true,
                    dots: false,
                    stagePadding: 0,
                    navText: [
                        '<i class="fa fa-chevron-left"></i>',
                        '<i class="fa fa-chevron-right"></i>'
                    ],
                    responsive: {
                        0: { 
                            items: 1,
                            margin: 0,
                            stagePadding: 0
                        },
                        600: { 
                            items: 1,
                            margin: 0,
                            stagePadding: 0
                        },
                        1000: { 
                            items: 2,
                            margin: 0,
                            stagePadding: 0
                        },
                        1200: { 
                            items: 3,
                            margin: 0,
                            stagePadding: 0
                        }
                    }
                });
            } else {
                $('#listTopRooms').html('<p class="text-center">Chưa có dữ liệu phòng nổi bật.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu phòng nổi bật:", error);
            $('#listTopRooms').html('<p class="text-center text-danger">Không thể tải dữ liệu phòng nổi bật.</p>');
        }
    });
}

// Hàm xem chi tiết phòng
function viewTopRoomDetail(roomId) {
    $.ajax({
        url: `/RoomDetail/${roomId}`,
        type: "GET",
        success: function (response) {
            // Load nội dung modal
            $('#topRoomDetailContent').html(response);
            
            // Hiển thị modal bằng jQuery Bootstrap 4
            $('#topRoomDetailModal').modal('show');
            
            // Gắn sự kiện cho nút đặt phòng
            $('#bookingBtn').off('click').on('click', function() {
                handleBooking(roomId);
            });
            
            // Cập nhật min date cho checkOut khi checkIn thay đổi
            $('#checkInDate').on('change', function() {
                var checkInDate = new Date($(this).val());
                checkInDate.setDate(checkInDate.getDate() + 1);
                var minCheckOut = checkInDate.toISOString().split('T')[0];
                $('#checkOutDate').attr('min', minCheckOut);
            });
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải chi tiết phòng:", error);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Không thể tải thông tin chi tiết phòng.'
            });
        }
    });
}

// Hàm xử lý đặt phòng
function handleBooking(roomId) {
    var checkIn = $('#checkInDate').val();
    var checkOut = $('#checkOutDate').val();
    
    // Kiểm tra đã chọn ngày chưa
    if (!checkIn || !checkOut) {
        Swal.fire({
            icon: 'warning',
            title: 'Chưa chọn ngày!',
            text: 'Vui lòng chọn ngày đến và ngày đi.'
        });
        return;
    }
    
    // Kiểm tra ngày hợp lệ
    if (new Date(checkIn) >= new Date(checkOut)) {
        Swal.fire({
            icon: 'warning',
            title: 'Ngày không hợp lệ!',
            text: 'Ngày đến phải trước ngày đi.'
        });
        return;
    }
    
    // Hiển thị loading
    Swal.fire({
        title: 'Đang kiểm tra...',
        text: 'Vui lòng đợi',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Gọi API kiểm tra phòng trống
    $.ajax({
        url: `/CheckRoomAvailability/${roomId}`,
        type: "GET",
        data: { checkIn: checkIn, checkOut: checkOut },
        success: function (response) {
            if (response.success) {
                // Phòng còn trống, chuyển đến trang đặt phòng
                Swal.fire({
                    icon: 'success',
                    title: 'Phòng còn trống!',
                    text: 'Đang chuyển đến trang đặt phòng...',
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    window.location.href = `/Booking/${roomId}/${checkIn}/${checkOut}`;
                });
            } else {
                // Phòng đã được đặt
                Swal.fire({
                    icon: 'error',
                    title: 'Phòng không khả dụng!',
                    text: response.message
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi kiểm tra phòng:", error);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Không thể kiểm tra trạng thái phòng. Vui lòng thử lại.'
            });
        }
    });
}
