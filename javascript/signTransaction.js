var bip39        = require("bip39");
var hdkey        = require('ethereumjs-wallet/hdkey');
var Transaction  = require('ethereumjs-tx');
var colors       = require('colors');
var Web3         = new require('web3');
var web3         = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 5) {
  console.log('\nUsage: signTransaction.js <from privkey> <from address> <to address> <value in ethers>\n');
  process.exit(0);
}

var tx = {
  from: process.argv[3],
  to: process.argv[4],
  value: web3.toHex(web3.toWei(process.argv[5], 'ether')),
  gas: '0xff000'
}

web3.eth.getTransactionCount(tx.from, function(err, count) {
  if (err) {
    console.log('\nError: ' + err + '\n');
    return;
  }

  tx.nonce = count;

  var transaction = new Transaction(tx);
  transaction.sign(Buffer.from(process.argv[2], 'hex'));
  var serialized = transaction.serialize();

  web3.eth.sendRawTransaction(serialized, function(err, txid) {
    if (err) {
      console.log('\nError: ' + err + '\n');
    } else {
      console.log('\nTxid: ' + txid + '\n');
    }
  })

})

