// withdraw.js
$(document).ready(function () {
    const $form = $("#withdraw-form");
    const $submitButton = $form.find('button[type="submit"]');

    $form.submit(async function (e) {
        e.preventDefault();

        const amount = $("#amount").val();
        if (!amount || isNaN(amount) || parseFloat(amount) <= 0) {
            ErrorHandler.warning('Please enter a valid amount.');
            return;
        }

        $submitButton.prop('disabled', true);

        try {
            const response = await fetch('/Transactions/Withdraw', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({ amount: parseFloat(amount) })
            });

            const result = await response.json();


            if (result.statusCode !== 200) {
                const errorMessage = result.message || result.Message || result.error || 'An error occurred while processing your withdrawal.';
                ErrorHandler.error(errorMessage);
                return;
            }

            ErrorHandler.success(result.message);
            $("#amount").val('');
            if (typeof fetchWalletBalance === 'function') {
                fetchWalletBalance();
            } 
            
        } catch (error) {
            ErrorHandler.error('Failed to connect to the server. Please try again.');
        } finally {
            $submitButton.prop('disabled', false);
        }
    });
});