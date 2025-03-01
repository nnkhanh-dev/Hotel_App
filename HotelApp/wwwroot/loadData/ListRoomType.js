$(document).ready(function () {
    $('#RoomTypeTableAdmin').DataTable({
        ajax: {
            url: '/RoomType/ListRoomType',
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
                data: 'description', 
            },
            {
                data: 'people', 
            },
            {
                data: 'imagePath',
                render: (data) => `<td><img src="${data}" width="100" height="80" style="object-fit:fill;"></td>`,
            },
            {
                data: 'id', // Chức năng (nút)
                render: (data) => `
                    <a href="/RoomType/Details/${data}" class="btn btn-info btn-sm">Xem</a>
                    <a href="/RoomType/Edit/${data}" class="btn btn-warning btn-sm">Sửa</a>
                    <button onClick="DeleteRoomType(${data})" class="btn btn-danger btn-sm">Xóa</button>
                `,
            }
        ],
        columnDefs: [
            { width: "5%", targets: 0 },
            { width: "15%", targets: 1 },
            { width: "30%", targets: 2 },
            { width: "10%", targets: 3 },
            { width: "15%", targets: 4 },
            { width: "20%", targets: 5 }
        ]
    });
});
// Hàm gọi AJAX để xóa
function DeleteRoomType(id) {
    if (confirm('Bạn có chắc chắn muốn xóa khu vực này?')) {
        $.ajax({
            url: '/RoomType/Delete/' + id, // URL của Controller
            type: 'POST', // Phương thức POST
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Lấy token anti-forgery
            },
            success: function (response) {
                if (response.success) {
                    toastr.success('Xóa khu vực thành công!');
                    // Reload lại DataTable sau khi xóa thành công
                    $('#RoomTypeTableAdmin').DataTable().ajax.reload();
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