$(document).ready(function () {
    $.ajax({
        url: "/LoaiPhong",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                let htmlContent = '';

                response.data.forEach(room => {
                    htmlContent += `
                        <div class="item" style="width: 350px; height: 400px;"> <!-- Đặt kích thước cố định cho item -->
                            <div id="serv_hover" class="room" style="width: 100%; height: 100%;">
                                <div class="room_img" style="width: 100%; height: 200px"> <!-- Chiều cao cố định cho vùng chứa ảnh -->
                                    <figure><img src="${room.imagePath}" alt="${room.name}" /></figure>
                                </div>
                                <div class="bed_room" style="padding: 20px 10px 10px 10px; ">
                                    <h3>${room.name}</h3>
                                    <p>${room.description}</p>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listRoomType').html(htmlContent);

                // Khởi tạo Owl Carousel
                $('#listRoomType').owlCarousel({
                    items: 3,
                    margin: 10,
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    nav: true,            // Hiển thị nút điều hướng
                    dots: false,           // Ẩn các dấu chấm
                    navText: [
                        '<span style="font-size: 24px; cursor: pointer;">&#9664;</span>',
                        '<span style="font-size: 24px; cursor: pointer;">&#9654;</span>'
                    ],
                    responsive: {
                        0: { items: 1 },
                        600: { items: 2 },
                        1000: { items: 3 }
                    }
                });
            } else {
                $('#listRoomType').html('<p>Không có loại phòng nào.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu phòng:", error);
            $('#listRoomType').html('<p>Không thể tải dữ liệu loại phòng.</p>');
        }
    });
});
