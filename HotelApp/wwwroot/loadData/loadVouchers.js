$(document).ready(function () {
    $.ajax({
        url: "/UuDai",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                let htmlContent = '';

                response.data.forEach(voucher => {
                    htmlContent += `
                        <div class="item" style="height: auto; padding: 10px;">
                            <div id="serv_hover" class="voucher_box" style="width: 100%; height: 400px; cursor: pointer; box-shadow: 0 4px 8px rgba(0,0,0,0.1); border-radius: 8px; overflow: hidden; display: flex; flex-direction: column; background: white;">
                                <div class="voucher_img" style="width: 100%; height: 257px; overflow: hidden; flex-shrink: 0;">
                                    <img src="${voucher.imagePath}" alt="${voucher.name}" style="width: 100%; height: 100%; object-fit: cover; display: block;" />
                                </div>
                                <div class="voucher_content" style="padding: 15px; height: 257px; display: flex; flex-direction: column; overflow: hidden;">
                                    <h3 style="margin: 0 0 8px 0; font-size: 16px; line-height: 1.3; height: 42px; overflow: hidden; display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical;">${voucher.name}</h3>
                                    <p style="margin: 0 0 8px 0; font-size: 13px; color: #007bff; font-weight: bold;"><i class="fa fa-tag" aria-hidden="true"></i> Mã: ${voucher.code}</p>
                                    <p style="margin: 0; font-size: 13px; line-height: 1.4; overflow: hidden; display: -webkit-box; -webkit-line-clamp: 3; -webkit-box-orient: vertical;">${voucher.description}</p>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listVoucher').html(htmlContent);

                // Khởi tạo Owl Carousel
                $('#listVoucher').owlCarousel({
                    items: 1,
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
                        600: { items: 1 },
                        1000: { items: 2 },
                        1200: { items: 3 }   
                    }
                });
            } else {
                $('#listVoucher').html('<p>Không có loại phòng nào.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu phòng:", error);
            $('#listVoucher').html('<p>Không thể tải dữ liệu loại phòng.</p>');
        }
    });
});
