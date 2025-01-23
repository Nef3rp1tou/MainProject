$(document).ready(function () {
    var $confirmButton = $('#confirm-payment');

    $confirmButton.click(function (e) {
        e.preventDefault();

        // Extract values from the button's data attributes
        var transactionId = $confirmButton.data('transaction-id');
        var amount = $confirmButton.data('amount');

        $confirmButton.prop('disabled', true); // Disable button to prevent multiple clicks

        // Send AJAX request to Payment Controller
        $.ajax({
            url: '/payment/senddepositfinish',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                transactionId: transactionId,
                amount: amount
            }),
            success: function (response) {
                if (response && response.status === 'Success') {
                    alert('Payment completed successfully!');
                    window.location.href = '/transaction/history'; // Redirect to history page
                } else {
                    alert('Payment failed. Please try again.');
                }
            },
            error: function (xhr) {
                var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'An error occurred during the payment process';
                console.error('Error:', errorMessage);
                alert(errorMessage);
            },
            complete: function () {
                $confirmButton.prop('disabled', false); // Re-enable button
            }
        });
    });
});
