$(document).ready(function () {
    $.ajax({
        url: "/LoaiPhong",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                let roomHtmlContent = '';
                

                // Duyệt qua tất cả các loại phòng và tạo các option cho roomType và people
                response.data.forEach(room => {
                    roomHtmlContent += `
                        <option value="${room.name}">${room.name}</option>
                    `;

                    
                });

                // Thêm option "Tất cả" vào đầu danh sách roomType
                roomHtmlContent = `<option value="">Tất cả</option>` + roomHtmlContent;

                // Gắn HTML vào select 'roomType'
                $('#roomType').html(roomHtmlContent);

                

                // Khởi tạo Owl Carousel nếu có
                let owlHtmlContent = '';
                response.data.forEach(room => {
                    owlHtmlContent += `
                        <div class="item" style="width: 350px; height: 400px;">
                            <div id="serv_hover" class="room" style="width: 100%; height: 100%;">
                                <div class="room_img" style="width: 100%; height: 200px">
                                    <figure><img src="${room.imagePath}" alt="${room.name}" /></figure>
                                </div>
                                <div class="bed_room" style="padding: 20px 10px 10px 10px;">
                                    <h3>${room.name}</h3>
                                    <p>${room.description}</p>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listRoomType').html(owlHtmlContent);

                // Khởi tạo Owl Carousel
                $('#listRoomType').owlCarousel({
                    items: 3,
                    margin: 10,
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
                    nav: true,
                    dots: false,
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
