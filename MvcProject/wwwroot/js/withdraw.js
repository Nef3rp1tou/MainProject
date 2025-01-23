document.getElementById('withdraw-form').addEventListener('submit', function (event) {
    event.preventDefault();

    const fullName = document.getElementById('fullName').value.trim();
    const cardNumber = document.getElementById('cardNumber').value.trim();
    const amount = parseFloat(document.getElementById('amount').value);

    if (!fullName || !cardNumber || isNaN(amount) || amount <= 0) {
        document.getElementById('response-message').innerText = 'Please provide valid input in all fields.';
        return;
    }

    const data = { fullName, cardNumber, amount };

    fetch('/Transactions/Withdraw', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(data => {
            if (data.success) {
                document.getElementById('response-message').innerText = 'Withdrawal successful!';
            } else {
                document.getElementById('response-message').innerText = 'Withdrawal failed: ' + data.message;
            }
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('response-message').innerText = 'An error occurred. Please try again.';
        });
});
