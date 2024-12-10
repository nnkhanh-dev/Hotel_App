$(document).ready(function () {
    let allRoomsData = []; // Mảng để lưu trữ tất cả dữ liệu phòng

    $('#filterBtn').click(function () {
        // Lấy giá trị ngày từ input
        const checkIn = $('#CheckIn').val();
        const checkOut = $('#CheckOut').val();
        const roomType = $('#roomType').val();
        const area = $('#area').val();
        const price = $('#price').val();

        // Kiểm tra xem ngày có hợp lệ không
        if (!checkIn || !checkOut || new Date(checkIn) >= new Date(checkOut)) {
            alert('Vui lòng nhập ngày hợp lệ!');
            return;
        }

        const selectedAmenities = [];
        $('input[type="checkbox"]:checked').each(function () {
            selectedAmenities.push($(this).val());  // Lấy giá trị của checkbox (ID của tiện nghi)
        });

        // Khởi tạo URL cơ bản
        let url = `/List/${checkIn}/${checkOut}`;

        // Gửi yêu cầu AJAX
        $.ajax({
            url: url, // Sử dụng URL đã được tạo
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                allRoomsData = response.data; // Lưu trữ dữ liệu phòng
                filterRooms(price); // Lọc phòng ngay sau khi tải dữ liệu
            },
            error: function () {
                alert('Lỗi khi tải danh sách phòng.');
            }
        });
    });

    // Hàm lọc phòng
    function filterRooms(sortOrder) {
        const roomType = $('#roomType').val();
        const area = $('#area').val();
        const selectedAmenities = [];
        $('input[type="checkbox"]:checked').each(function () {
            selectedAmenities.push($(this).val());  // Lấy giá trị của checkbox (ID của tiện nghi)
        });

        // Lọc dữ liệu phòng theo các tiêu chí
        const filteredRooms = allRoomsData.filter(room => {
            const matchesRoomType = roomType ? room.typeName === roomType : true;
            const matchesArea = area ? room.areaName === area : true;
            const matchesAmenities = selectedAmenities.every(amenity => room.amenityNames.includes(amenity));

            return matchesRoomType && matchesArea && matchesAmenities;
        });

        // Sắp xếp theo giá
        if (sortOrder === "1") {
            // Sắp xếp từ thấp đến cao
            filteredRooms.sort((a, b) => a.price - b.price);
        } else if (sortOrder === "2") {
            // Sắp xếp từ cao đến thấp
            filteredRooms.sort((a, b) => b.price - a.price);
        }

        // Hiển thị các phòng đã lọc
        renderRooms(filteredRooms);
    }

    // Hàm render danh sách phòng
    function renderRooms(data) {
        const container = $('#room-container');
        container.empty(); // Xóa nội dung cũ

        data.forEach(room => {
            const firstImageUrl = room.imageUrls.length > 0 ? room.imageUrls[0].replace('~', '') : 'placeholder.jpg';
            const amenitiesHtml = room.amenityNames.map(amenity => `<span class="badge bg-primary me-1">${amenity}</span>`).join('');

            const roomHtml = `
            <div class="col-md-6 mb-4">
                <div class="card shadow-sm">
                    <div class="row g-0">
                        <div class="col-md-4">
                            <img src="${firstImageUrl}" class="d-block w-100 img-fluid rounded-start" style="height: 100%; object-fit: cover; object-position: center;" alt="Room Image">
                        </div>
                        <div class="col-md-8">
                            <div class="card-body d-flex flex-column justify-content-between">
                                <h3 class="card-title">${room.typeName} - ${room.areaName}</h3>
                                <p class="card-text">
                                    <strong>Giá:</strong> ${room.price.toLocaleString()} VND<br>
                                    <strong>Tiện nghi:</strong> ${amenitiesHtml}
                                </p>
                                <a onClick="Detail('/RoomDetail/${room.id}')" class="btn btn-outline-secondary btn-sm align-self-start mt-3">Xem chi tiết</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
            container.append(roomHtml);
        });
    }

});

function Detail(url) {
    // Gửi yêu cầu AJAX để lấy chi tiết phòng
    $.ajax({
        url: url,  // Đường dẫn tới action trong controller (ví dụ: /Detail/123)
        method: 'GET',
        success: function (response) {

            $('#roomDetailModalBody').html(response);
            $("#carouselExampleIndicators").owlCarousel({
                loop: true,               // Quay vòng qua các ảnh
                margin: 10,               // Khoảng cách giữa các ảnh
                nav: true,                // Hiển thị các nút điều khiển (Previous/Next)
                dots: true,               // Hiển thị các dấu chấm
                autoplay: true,           // Tự động chuyển tiếp
                autoplayTimeout: 3000,    // Thời gian giữa các slide
                responsive: {
                    0: {
                        items: 1          // Số ảnh trên màn hình nhỏ
                    },
                    600: {
                        items: 1          // Số ảnh trên màn hình vừa
                    },
                    1000: {
                        items: 1          // Số ảnh trên màn hình lớn
                    }
                }
            });
            $('#roomDetailModal').modal('show');
            $('#bookingBtn').click(function () {
                const checkIn = $('#CheckIn').val();
                const checkOut = $('#CheckOut').val();
                const roomId = $(this).data('roomid'); // Lấy ID phòng từ thuộc tính data-roomId của button
                const userId = $('#userId').val(); // Lấy userId từ trường ẩn

                // Hiển thị thông báo xác nhận
                if (confirm("Bạn có chắc muốn đặt phòng này không?")) {
                    // Điều hướng đến URL để xử lý đặt phòng
                    window.location.href = `/Admin/Booking/${roomId}/${checkIn}/${checkOut}?userId=${userId}`;
                }
            });
        },
        error: function (xhr, status, error) {
            // Hiển thị lỗi chi tiết
            let errorMessage = `Lỗi: ${xhr.status} - ${xhr.statusText}\nChi tiết: ${xhr.responseText || error}`;
            console.error(errorMessage); // Log lỗi chi tiết vào console
            alert(errorMessage); // Hiển thị lỗi cụ thể cho người dùng
        }
    });
}
