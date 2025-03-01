$(document).ready(function () {
    // Show input fields when the button is clicked
    $('#addCCCDImage').on('click', function () {
        var newInputs = $('#cccdImageInputs').clone().removeAttr('id').show();
        $('#cccdImageInputsContainer').append(newInputs);
    });

    // Initialize DataTable
    $('#BookingTableAdmin').DataTable({
        ajax: {
            url: '/admin/booking/bookinglist',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            {
                data: null,
                render: (data, type, row, meta) => meta.row + 1,
                width: "1%"
            }, // Số thứ tự
            {
                data: 'userEmail',
                width: "12%"
            },
            {
                data: 'userFullName',
                width: "12%"
            },
            {
                data: 'roomCode',
                width: "4%"
            },
            {
                data: 'checkIn',
                render: (data) => {
                    if (!data) return ''; // Kiểm tra giá trị null hoặc undefined
                    const date = new Date(data);
                    return `${String(date.getDate()).padStart(2, '0')}-${String(date.getMonth() + 1).padStart(2, '0')}-${date.getFullYear()}`;
                },
                width: "10%"
            },
            {
                data: 'checkOut',
                render: (data) => {
                    if (!data) return ''; // Kiểm tra giá trị null hoặc undefined
                    const date = new Date(data);
                    return `${String(date.getDate()).padStart(2, '0')}-${String(date.getMonth() + 1).padStart(2, '0')}-${date.getFullYear()}`;
                },
                width: "10%"
            },
            {
                data: 'total',
                render: function (data) {
                    return data ? data.toLocaleString() + ' VND' : ''; // Hiển thị giá trị kèm VND
                },
                width: "12%"
            },
            {
                data: 'status',
                render: (data) => data == 0 ? 'Chờ nhận' :
                    data == 1 ? 'Đang sử dụng' :
                        data == 2 ? 'Chờ thanh toán' :
                            data == 3 ? 'Hoàn thành' : 'Không xác định',
                width: "10%"
            },
            {
                data: null,
                render: (data, type, row) => {
                    if (row.status == 3) {
                        return `<button class="btn btn-info btn-sm" onclick="viewDetails(${row.id})">Xem chi tiết</button>`;
                    } else {
                        return `
                            <button class="btn btn-info btn-sm mb-2" onclick="viewDetails(${row.id})">Chi tiết</button>
                            <button class="btn btn-warning btn-sm" onclick="editBooking(${row.id})">Cập nhật</button>
                        `;
                    }
                },
                width: "8%"
            }
        ]
    });
});

function viewDetails(id) {
    // Implement view details logic here
    window.location.href = `/admin/booking/details/${id}`;
}

function editBooking(id) {
    // Implement edit booking logic here
    window.location.href = `/admin/booking/edit/${id}`;
}

function deleteCCCDImage(imageId) {
    if (confirm("Bạn có chắc chắn muốn xóa ảnh này không?")) {
        $.ajax({
            url: '/Admin/Booking/DeleteImage',
            type: 'POST',
            data: { id: imageId },
            success: function (response) {
                if (response.success) {
                    $('#cccd-' + imageId).remove();
                    toastr.success("Xóa ảnh thành công");
                } else {
                    toastr.error("Xóa ảnh thất bại");
                }
            },
            error: function (xhr, status, error) {
                toastr.error("Lỗi kết nối: " + error);
            }
        });
    }
}

$(document).ready(function () {
    // Carousel ảnh
    const imageCount = $("#carouselExampleIndicators .item").length; // Count the number of images

    $("#carouselExampleIndicators").owlCarousel({
        loop: imageCount > 6,       // Enable loop if more than 6 images
        margin: 18,                 // Adjust space between images
        nav: true,                  // Enable navigation arrows
        dots: true,                 // Show dots for navigation
        autoplay: imageCount > 6,   // Enable autoplay if more than 6 images
        autoplayTimeout: 3000,      // Time interval for each slide
        responsive: {
            0: {
                items: 1            // Display 1 image on small screens
            },
            600: {
                items: 2            // Display 2 images on medium screens
            },
            1000: {
                items: 6            // Display 6 images on larger screens
            }
        }
    });
});
