var Transaction  = require('ethereumjs-tx');
var Wallet       = require('ethereumjs-wallet');
var Web3         = new require('web3');
var web3         = new Web3();

web3.setProvider(new web3.providers.HttpProvider('YOUR HTTP PROVIDER HERE'));

if (process.argv.length < 5) {
  console.log('\nUsage: signTransaction.js <from privkey> <to address> <value in ethers>\n');
  process.exit(0);
}

var rawPrivateKey = Buffer.from(process.argv[2], 'hex');

var from = '0x' + Wallet.fromPrivateKey(rawPrivateKey).getAddress().toString('hex');

var tx = {
  from: from,
  to: process.argv[3],
  value: web3.toHex(web3.toWei(process.argv[4], 'ether')),
  gasPrice: web3.toHex(web3.eth.gasPrice),
  gas: web3.toHex(24000)
}

web3.eth.getTransactionCount(tx.from, function(err, count) {
  if (err) {
    console.log('\nError: ' + err + '\n');
    return;
  }

  tx.nonce = count;

  var transaction = new Transaction(tx);
  transaction.sign(rawPrivateKey);
  var serialized = transaction.serialize().toString('hex');

  web3.eth.sendRawTransaction('0x' + serialized, function(err, txid) {
    if (err) {
      console.log('\nError: ' + err + '\n');
    } else {
      var timer = setInterval(function() {
        var receipt = web3.eth.getTransactionReceipt(txid);
        if (receipt == null) {
          console.log('Waiting for transaction to be confirmed...');
        } else {
          clearInterval(timer);
          console.log('Transaction confirmed!');
        }
      }, 300);
    }
  })

})

