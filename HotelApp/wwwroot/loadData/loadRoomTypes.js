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
                        <div class="item" style="height: auto; padding: 10px;">
                            <div id="serv_hover" class="room" style="width: 100%; height: 400px; cursor: pointer; box-shadow: 0 4px 8px rgba(0,0,0,0.1); border-radius: 8px; overflow: hidden; display: flex; flex-direction: column; background: white;">
                                <div class="room_img" style="width: 100%; height: 257px; overflow: hidden; flex-shrink: 0;">
                                    <img src="${room.imagePath}" alt="${room.name}" style="width: 100%; height: 100%; object-fit: cover; display: block;" />
                                </div>
                                <div class="bed_room" style="padding: 15px; height: 257px; display: flex; flex-direction: column; overflow: hidden;">
                                    <h3 style="margin: 0 0 12px 0; font-size: 16px; line-height: 1.3; height: 42px; overflow: hidden; display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical;">${room.name}</h3>
                                    <p style="margin: 0; font-size: 13px; line-height: 1.4; overflow: hidden; display: -webkit-box; -webkit-line-clamp: 5; -webkit-box-orient: vertical;">${room.description}</p>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listRoomType').html(owlHtmlContent);

                // Khởi tạo Owl Carousel
                $('#listRoomType').owlCarousel({
                    items: 1,
                    margin: 0,
                    loop: true,
                    autoplay: true,
                    autoplayTimeout: 5000,
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
                $('#listRoomType').html('<p>Không có loại phòng nào.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu phòng:", error);
            $('#listRoomType').html('<p>Không thể tải dữ liệu loại phòng.</p>');
        }
    });
});
