// admin-dashboard.js
$(document).ready(function () {
    let table = $('#pending-requests').DataTable();

    if ($.fn.DataTable.isDataTable('#pending-requests')) {
        table.destroy();
    }

    table = $('#pending-requests').DataTable({
        responsive: true,
        pageLength: 10,
        order: [[4, 'desc']],
        language: {
            search: "",
            searchPlaceholder: "Search requests..."
        },
        destroy: true // Allow table to be reinitialized
    });

    function handleRequest(url, requestId, actionType) {
        const $button = $(`.${actionType}-request[data-id="${requestId}"]`);
        const originalText = $button.text();

        $button.closest('tr').find('button').prop('disabled', true);
        $button.text('Processing...');

        return $.ajax({
            url: url,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestId),
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        })
            .done(function (response) {
                if (response.statusCode === 200) {
                    ErrorHandler.success(response.message || `Request ${actionType}d successfully`);
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                } else {
                    ErrorHandler.error(response.message || `Failed to ${actionType} request`);
                    $button.closest('tr').find('button').prop('disabled', false);
                }
            })
            .fail(function (jqXHR) {
                let errorMsg;
                if (jqXHR.statusCode === 200) {
                    errorMsg = jqXHR.message;
                } else {
                    errorMsg = `Failed to ${actionType} request. Please try again.`;
                }
                ErrorHandler.error(errorMsg);
                $button.closest('tr').find('button').prop('disabled', false);
            })
            .always(function () {
                $button.text(originalText);
            });
    }

    $(document).on('click', '.approve-request', function () {
        const requestId = $(this).data('id');
        handleRequest('/Admin/ApproveRequest', requestId, 'approve');
    });

    $(document).on('click', '.reject-request', function () {
        const requestId = $(this).data('id');
        handleRequest('/Admin/RejectRequest', requestId, 'reject');
    });

    function refreshTable() {
        $.get(window.location.href)
            .done(function (response) {
                const newDoc = new DOMParser().parseFromString(response, 'text/html');
                const newTableBody = $(newDoc).find('#pending-requests tbody').html();

                // Clear and redraw the table with new data
                table.clear();
                $('#pending-requests tbody').html(newTableBody);
                table.rows.add($('#pending-requests tbody tr'));
                table.draw();
            })
            .fail(function (jqXHR) {
                let errorMsg = 'Failed to refresh data';
                if (jqXHR.responseJSON && jqXHR.responseJSON.message) {
                    errorMsg = jqXHR.responseJSON.message;
                }
                ErrorHandler.warning(errorMsg);
            });
    }

    const refreshInterval = setInterval(refreshTable, 30000);

    // Clean up interval when leaving the page
    $(window).on('unload', function () {
        clearInterval(refreshInterval);
    });
});