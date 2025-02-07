$(document).ready(function () {
    const $confirmButton = $('#confirm-payment');

    $confirmButton.click(async function (e) {
        e.preventDefault();

        const transactionId = $confirmButton.data('transaction-id');
        const amount = $confirmButton.data('amount');

        if (!transactionId || !amount || isNaN(amount) || parseFloat(amount) <= 0) {
            ErrorHandler.warning('Invalid transaction details.');
            return;
        }

        $confirmButton.prop('disabled', true);

        try {
            const response = await fetch('/payment/senddepositfinish', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({ transactionId, amount })
            });

            const result = await response.json();

            if (result.statusCode !== 200) {
                const errorMessage = result.message || result.Message || 'An error occurred during payment processing.';
                ErrorHandler.error(errorMessage);
                return;
            }

            if (result.data.status === 2) {
                ErrorHandler.success('Payment completed successfully! Redirecting...');
                setTimeout(() => { window.location.href = '/home/index'; }, 2000);
            } else {
                ErrorHandler.error(result.message || 'Payment failed. Please try again.');
            }
        } catch (error) {
            ErrorHandler.error('Failed to connect to the server. Please try again.');
        } finally {
            $confirmButton.prop('disabled', true);
        }
    });
});
