$(document).ready(function () {
    $("#deposit-form").submit(async function (e) {
        e.preventDefault();

        const amount = $("#amount").val();
        if (!amount || isNaN(amount) || parseFloat(amount) <= 0) {
            $("#response-message").html('<div class="alert alert-danger">Invalid amount entered.</div>');
            return;
        }

        try {
            const response = await fetch('/Transactions/Deposit', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ amount: parseFloat(amount) }) // Send as JSON object with "amount"
            });

            const result = await response.json();
            if (result.success) {
                // Redirect to the PaymentUrl
                window.location.href = result.redirectUrl;
            } else {
                $("#response-message").html(`<div class="alert alert-danger">${result.message}</div>`);
            }
        } catch (error) {
            $("#response-message").html(`<div class="alert alert-danger">Error: ${error.message}</div>`);
        }
    });
});
