$(document).ready(function () {
    $('#bookingsTable').DataTable({
        ajax: {
            url: '/Client/Bookings', 
            type: 'GET',
            dataSrc: 'data' 
        },
        columns: [
            {
                data: null,
                render: (data, type, row, meta) => meta.row + 1,
                width: "5%" 
            }, // Số thứ tự
            {
                data: 'typeName',
                width: "10%" 
            },
            {
                data: 'areaName',
                width: "10%" 
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
                data: 'status',
                render: (data) => data == 0 ? 'Chờ nhận' :
                    data == 1 ? 'Đang sử dụng' :
                        data == 2 ? 'Chờ thanh toán' :
                            data == 3 ? 'Hoàn thành' :
                                data == -1 ? 'Đã hủy' : 'Không xác định',
                width: "15%" 
            },
            {
                data: 'payType',
                render: (data) => data == 0 ? 'Thanh toán khi nhận phòng' : 'Thanh toán VnPay',
                width: "15%" 
            },
            {
                data: 'total',
                render: function (data) {
                    return data ? data.toLocaleString() + ' VND' : ''; // Hiển thị giá trị kèm VND
                },
                width: "10%"
            },
            {
                data: 'createAt',
                render: (data) => {
                    if (!data) return ''; // Kiểm tra giá trị null hoặc undefined
                    const date = new Date(data);
                    return `${String(date.getDate()).padStart(2, '0')}-${String(date.getMonth() + 1).padStart(2, '0')}-${date.getFullYear()}`;
                },
                width: "10%"
            },
            {
                data: 'id',
                render: (data, type, row) => {
                    if (row.status === 0) { // Chỉ hiển thị nút "Hủy" khi trạng thái là "Chờ nhận" (status == 0)
                        return `
                            <a onClick="Cancel('/Client/Cancel/${data}')" class="btn btn-danger btn-sm">Hủy</a>
                        `;
                    }
                    return ''; // Nếu không phải trạng thái "Chờ nhận", không hiển thị nút "Hủy"
                },
                width: "5%"
            }

        ]
    });
});

function Cancel(url) {
    // Lưu lại trang hiện tại của DataTable
    var currentPage = $('#bookingsTable').DataTable().page();

    // Gọi API hủy đơn đặt phòng
    $.ajax({
        url: url, // URL hủy đơn đặt phòng
        type: 'POST',
        success: function (response) {
            // Kiểm tra nếu hủy thành công (tuỳ vào phản hồi của API)
            if (response.success) {
                // Hiển thị Toast thông báo thành công
                toastr.success(response.message);  // Hiển thị thông báo thành công

                // Tải lại DataTable và giữ lại trang hiện tại
                var table = $('#bookingsTable').DataTable();
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
