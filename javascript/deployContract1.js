var Web3 = require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 3) {
  console.log('\nUsage: deployContract1.js <from>\n');
  process.exit(0);
}

var contractBinary = '0x6060604052341561000c57fe5b5b60c68061001b6000396000f30060606040526000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff1680632096525514604457806355241077146067575bfe5b3415604b57fe5b60516084565b6040518082815260200191505060405180910390f35b3415606e57fe5b60826004808035906020019091905050608f565b005b600060005490505b90565b806000819055505b505600a165627a7a72305820f82ee94badc08a2f68d9f3895fcda079f891442891bbc2c65396a556a97ce72b0029';

var transaction = {
  from: process.argv[2],
  data: contractBinary,
  gas: 200000,
  // Where is the "to" field?!
  // Where is the "value" field?!
}

web3.eth.sendTransaction(transaction, function(err, txid) {

  if (err) {
    console.log('Error: ' + err);
    return;
  } 

  console.log('\nTxid: ' + txid + '\n');

  var timer = setInterval(function() {

    var receipt = web3.eth.getTransactionReceipt(txid);
    if (receipt) {
      console.log('\nContract address: ' + receipt.contractAddress + '\n');
      clearInterval(timer);
    } else {
      console.log('Waiting for transaction to be mined');
    }

  }, 1000);
})
