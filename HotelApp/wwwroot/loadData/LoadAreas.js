$(document).ready(function () {
    $.ajax({
        url: "/KhuVuc",   // Đường dẫn đến API của bạn
        type: "GET",      // Phương thức GET
        dataType: "json", // Dữ liệu trả về có dạng JSON
        success: function (response) {
            // Kiểm tra dữ liệu trả về và điền vào <select>
            if (response && response.data && response.data.length > 0) {
                let areaHtmlContent = '<option value="">Tất cả</option>';

                // Duyệt qua tất cả các khu vực và tạo option
                response.data.forEach(area => {
                    areaHtmlContent += `
                        <option value="${area.name}">${area.name}</option>
                    `;
                });

                // Gắn HTML vào select 'area'
                $('#area').html(areaHtmlContent);
            } else {
                // Nếu không có dữ liệu
                $('#area').html('<option value="">Không có khu vực nào</option>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu khu vực:", error);
            $('#area').html('<option value="">Không thể tải khu vực</option>');
        }
    });
});
