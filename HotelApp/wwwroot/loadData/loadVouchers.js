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
                        <div class="col-md-4">
                            <div class="voucher_box" style="width: 350px; height: 400px;"> <!-- Đặt kích thước cố định cho voucher -->
                                <div class="voucher_img" style="height: 200px;"> <!-- Chiều cao cố định cho vùng chứa ảnh -->
                                    <figure><img src="${voucher.imagePath}" alt="#" style="width: 100%; height: 100%; object-fit: cover;" /></figure>
                                </div>
                                <div class="voucher_content" style="padding: 20px;">
                                    <h3>${voucher.name}</h3>
                                    <span>[${voucher.code}]</span>
                                    <p>${voucher.description}</p>
                                </div>
                            </div>
                        </div>
                    `;
                });

                $('#listVoucher').html(htmlContent);

                // Khởi tạo Owl Carousel
                $('#listVoucher').owlCarousel({
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
                $('#listVoucher').html('<p>Không có loại phòng nào.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu phòng:", error);
            $('#listVoucher').html('<p>Không thể tải dữ liệu loại phòng.</p>');
        }
    });
});
