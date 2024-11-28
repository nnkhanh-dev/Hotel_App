$(document).ready(function () {
    $.ajax({
        url: "/TienNghi",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                let amenitiesHtml = '';  // Biến chứa HTML của các checkbox

                // Duyệt qua tất cả tiện nghi và tạo checkbox cho mỗi tiện nghi
                response.data.forEach(amenity => {
                    amenitiesHtml += `
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="checkbox" value="${amenity.name}" id="amenity-${amenity.name}">
                            <label class="form-check-label" for="amenity-${amenity.name}">
                                ${amenity.name}
                            </label>
                        </div>
                    `;
                });

                // Gắn HTML vào phần tử có id 'listAmenities'
                $('#listAmenities').html(amenitiesHtml);
            } else {
                $('#listAmenities').html('<p>Không có tiện nghi nào.</p>');
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi tải dữ liệu tiện nghi:", error);
            $('#listAmenities').html('<p>Không thể tải dữ liệu tiện nghi.</p>');
        }
    });
});
