var Web3 = new require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 5) {
  console.log('\nUsage: transferValue.js <from> <to> <value in ethers>\n');
  process.exit(0);
}

var transaction = {
  from: process.argv[2],
  to: process.argv[3],
  value: web3.toWei(process.argv[4], 'ether'),
  gas: 24000
}

// There is a hidden MAGIC here!

web3.eth.sendTransaction(transaction, function(err, txid) {
  if (err) {
    console.log('\nError: ' + err + '\n');
  } else {
    console.log('\nTxid: ' + txid + '\n');
  }
})
