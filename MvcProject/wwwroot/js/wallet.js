// wallet.js
async function fetchWalletBalance()
{
    try {
        const response = await fetch('/Wallet', {
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        });

        const json = await response.json();

       
        if (json.statusCode !== 200 ) {
            const errorMessage = json.message || json.Message || 'Failed to fetch wallet balance';
            ErrorHandler.error(errorMessage);
            document.getElementById('wallet-balance').innerText = 'Balance: Error';
            return;
        }

     
        const { currency, balance } = json.data

        document.getElementById('wallet-balance').innerText = `Balance: ${balance.toFixed(2)} ${currency}`;

    } catch (error) {
        ErrorHandler.error('Failed to connect to wallet service');
        document.getElementById('wallet-balance').innerText = 'Balance: Error';
    }
}

if (document.getElementById('wallet-balance')) {
    fetchWalletBalance();

    const refreshInterval = setInterval(fetchWalletBalance, 30000);

    $(window).on('unload', function () {
        clearInterval(refreshInterval);
    });
}

window.fetchWalletBalance = fetchWalletBalance;