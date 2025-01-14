async function fetchWalletBalance() {
    try {
        const response = await fetch('/Wallet'); 
        if (!response.ok) throw new Error('Failed to fetch wallet balance');

        const data = await response.json();
        const { balance, currency } = data; 

        document.getElementById('wallet-balance').innerText = `Balance: ${balance.toFixed(2)} ${currency}`;
    } catch (error) {
        console.error('Error fetching wallet balance:', error);
        document.getElementById('wallet-balance').innerText = 'Balance: Error';
    }
}

if (document.getElementById('wallet-balance')) {
    fetchWalletBalance();
    setInterval(fetchWalletBalance, 30000);
}
