// deposit.js
$(document).ready(function () {
    const $form = $("#deposit-form");
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
            const response = await fetch('/Transactions/Deposit', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({ amount: parseFloat(amount) })
            });

            const result = await response.json();

            if (result.statusCode !== 200) {
                ErrorHandler.error(result.message || result.Message || 'An error occurred while processing your deposit.');
                return;
            }

           
             ErrorHandler.success('Processing your deposit...');
             window.location.href = result.data;
            
        } catch (error) {
            console.error('Deposit error:', error);
            ErrorHandler.error('Failed to connect to the server. Please try again.');
        } finally {
            $submitButton.prop('disabled', false);
        }
    });
});