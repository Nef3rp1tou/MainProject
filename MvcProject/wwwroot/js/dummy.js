$(document).ready(function () {
    const $confirmButton = $('#confirm-payment');

    $confirmButton.click(function (e) {
        e.preventDefault();

        // Extract values from the button's data attributes
        const transactionId = $confirmButton.data('transaction-id');
        const amount = $confirmButton.data('amount');

        // Disable button to prevent multiple clicks
        $confirmButton.prop('disabled', true);

        // Send AJAX request to Payment Controller
        $.ajax({
            url: '/payment/senddepositfinish',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({ transactionId, amount }),
            success: function (response) {
                if (response && response.status === 2) {
                    alert('Payment completed successfully!');
                    window.location.href = '/home/index'; // Redirect to history page
                } else {
                    alert('Payment failed. Please try again.');
                }
            },
            error: function (xhr) {
                const errorMessage = xhr.responseJSON?.message || 'An error occurred during the payment process.';
                console.error('Error:', errorMessage);
                alert(errorMessage);
            },
            complete: function () {
                // Re-enable button
                $confirmButton.prop('disabled', false);
            }
        });
    });
});
