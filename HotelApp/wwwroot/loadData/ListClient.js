$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#ClientTableAdmin').DataTable({
        "ajax": { url: '/Admin/Client/GetAll' },
        "columns": [
            { data: null, render: (data, type, row, meta) => meta.row + 1 },
            { data: 'fullName', "width": "25%" },
            { data: 'email', "width": "25%" },
            { data: 'phoneNumber', "width": "25%" },
            {
                data: 'avatarUrl',
                render: (data) => {
                    const imageUrl = data.replace('~', '');
                    return `<img src="${imageUrl}" width="100" height="80" style="object-fit:fill;">`;
                }
            },
        ]
    });
}
