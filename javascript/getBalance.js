var Web3 = new require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 3) {
  console.log('\nUsage: getBalance.js <account>\n');
  process.exit(0);
}

web3.eth.getBalance(process.argv[2], function(err, balance) {
  if (err) {
    console.log('Error: ' + err);
  } else {
    console.log('\nBalance: ' + web3.fromWei(balance, 'ether') + '\n');
  }
})
