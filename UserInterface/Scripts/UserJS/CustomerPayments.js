

$(document).ready(function () {

    $('#tblOutStandingDetails').DataTable({
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 1
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[2, 'asc']]
    });
});