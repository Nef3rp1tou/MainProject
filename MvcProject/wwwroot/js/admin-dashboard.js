$(document).ready(function () {
    // Approve request
    $(document).on('click', '.approve-request', function () {
        const requestId = $(this).data('id');
        $.ajax({
            url: '/Admin/ApproveRequest',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestId),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while approving the request.');
            }
        });
    });

    $(document).on('click', '.reject-request', function () {
        const requestId = $(this).data('id');
        $.ajax({
            url: '/Admin/RejectRequest',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestId),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while rejecting the request.');
            }
        });
    });
});
