$(document).ready(function () {
    $('#AreaTableAdmin').DataTable({
        ajax: {
            url: '/Area/ListArea',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            {
                data: null, // Sử dụng null để cho phép DataTable tự động tạo số thứ tự
                render: (data, type, row, meta) => meta.row + 1, // meta.row bắt đầu từ 0
            },
            {
                data: 'name', // Tên khu vực
            },
            {
                data: 'id', // Chức năng (nút)
                render: (data) => `
                    <a href="/Area/Details/${data}" class="btn btn-info btn-sm">Xem</a>
                    <a href="/Area/Edit/${data}" class="btn btn-warning btn-sm">Sửa</a>
                    <button onClick="DeleteArea(${data})" class="btn btn-danger btn-sm">Xóa</button>
                `,
            }
        ],
        columnDefs: [
            { width: "20%", targets: 0 },
            { width: "40%", targets: 1 },
            { width: "40%", targets: 2 }
        ]
    });
});
// Hàm gọi AJAX để xóa
function DeleteArea(id) {
    if (confirm('Bạn có chắc chắn muốn xóa khu vực này?')) {
        $.ajax({
            url: '/Area/Delete/' + id, // URL của Controller
            type: 'POST', // Phương thức POST
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Lấy token anti-forgery
            },
            success: function (response) {
                if (response.success) {
                    toastr.success('Xóa khu vực thành công!');
                    // Reload lại DataTable sau khi xóa thành công
                    $('#AreaTableAdmin').DataTable().ajax.reload();
                } else {
                    toastr.error('Xóa khu vực thất bại!');
                }
            },
            error: function () {
                toastr.error('Đã có lỗi xảy ra khi xóa khu vực!');
            }
        });
    }
}