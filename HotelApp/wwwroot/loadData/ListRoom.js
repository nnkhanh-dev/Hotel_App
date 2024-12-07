$(document).ready(function () {
    $('#RoomTableAdmin').DataTable({
        ajax: {
            url: '/Room/ListRoom',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            {
                data: null, // Sử dụng null để cho phép DataTable tự động tạo số thứ tự
                render: (data, type, row, meta) => meta.row + 1, // meta.row bắt đầu từ 0
            },
            {
                data: 'roomTypeName'
            },
            {
                data: 'areaName',
            },
            {
                data: 'price', 
                render: (data) => new Intl.NumberFormat('vi-VN').format(data) + " VNĐ"
            },
            {
                data: 'discount',
                render: (data) => Math.round(data * 100) + " %"
            },
            {
                data: 'status',
                render: (data) => data == 0 ? 'Hoạt dộng' :
                                data == -1 ? 'Bảo trì' : 'Ngưng hoạt động'
            },
            {
                data: 'id', // Chức năng (nút)
                render: (data) => `
                    <a href="/Room/Details/${data}" class="btn btn-info btn-sm">Xem</a>
                    <a href="/Room/Edit/${data}" class="btn btn-warning btn-sm">Sửa</a>
                    <button onClick="DeleteRoom(${data})" class="btn btn-danger btn-sm">Xóa</button>
                `
            }
        ],
        columnDefs: [
            { width: "5%", targets: 0 },
            { width: "15%", targets: 1 },
            { width: "10%" , targets: 2 },
            { width: "15%", targets: 3 },
            { width: "10%", targets: 4 },
            { width: "15%", targets: 5 },
            { width: "20%", targets: 6 }
        ]
    });
});
// Hàm gọi AJAX để xóa
function DeleteRoom(id) {
    if (confirm('Bạn có chắc chắn muốn xóa phòng này?')) {
        $.ajax({
            url: '/Room/Delete/' + id, // URL của Controller
            type: 'POST', // Phương thức POST
            success: function (response) {
                if (response.success) {
                    // Hiển thị Toast thông báo thành công
                    toastr.success(response.message);  // Hiển thị thông báo thành công

                    // Tải lại DataTable và giữ lại trang hiện tại
                    var table = $('#RoomTableAdmin').DataTable();
                    table.ajax.reload(function () {
                        table.page(currentPage).draw('page');
                    });
                } else {
                    // Hiển thị Toast thông báo lỗi
                    toastr.error(response.message);  // Hiển thị thông báo lỗi
                }
            },
            error: function (xhr, status, error) {
                // Hiển thị Toast thông báo lỗi kết nối
                toastr.error("Lỗi kết nối: " + error);
            }
        });
    }
}

    $(document).ready(function () {
        // Thêm input file mới
        $('#addImage').on('click', function () {
            $('#imageInputs').append('<input type="file" name="NewImages" class="form-control mt-2" />');
        });
                // Carousel ảnh
        const track = document.querySelector('.carousel-track');

        $(document).ready(function () {
            const imageCount = $("#carouselExampleIndicators .item").length; // Đếm số ảnh

            $("#carouselExampleIndicators").owlCarousel({
                loop: imageCount > 6,       // Bật vòng lặp nếu số ảnh > 6
                margin: 18,                // Khoảng cách giữa các ảnh
                nav: true,                 // Hiển thị các nút điều khiển (Previous/Next)
                dots: true,                // Hiển thị các dấu chấm
                autoplay: imageCount > 6,  // Bật tự động chuyển tiếp nếu số ảnh > 6
                autoplayTimeout: 3000,     // Thời gian giữa các slide
                responsive: {
                    0: {
                        items: 1           // Số ảnh trên màn hình nhỏ
                    },
                    600: {
                        items: 2           // Số ảnh trên màn hình vừa
                    },
                    1000: {
                        items: 6           // Số ảnh trên màn hình lớn
                    }
                }
            });
        });


    });
function deleteImage(imageId) {
    if (confirm("Bạn có chắc chắn muốn xóa ảnh này không?")) {
        $.ajax({
            url: '/Room/DeleteImage',
            type: 'POST',
            data: { id: imageId },
            success: function (response) {
                if (response.success) {
                    $('#image-' + imageId).remove();
                    toastr.success("Xóa ảnh thành công");
                } else {
                    toastr.error("Xóa ảnh thất bại");
                }
            }
        });
    }
}

function editImage(imageId, caption) {
    const newCaption = prompt("Nhập caption mới:", caption);
    if (newCaption) {
        $.ajax({
            url: '/Room/EditImageCaption',
            type: 'POST',
            data: { id: imageId, caption: newCaption },
            success: function (response) {
                if (response.success) {
                    toastr.success("Cập nhật ảnh thành công");
                    // Cập nhật caption trực tiếp trên giao diện nếu cần
                } else {
                    toastr.error("Cập nhật ảnh thát bại");
                }
            }
        });
    }
}