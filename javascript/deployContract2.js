var solc = require('solc');
var Web3 = require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 3) {
  console.log('\nUsage: deployContract2.js <from>\n');
  process.exit(0);
}

var contractCode = 'pragma solidity ^0.4.0; contract test { uint value; function setValue(uint pValue) { value = pValue; } function getValue() returns (uint v) { v = value;} }';

var input = {'MyContract': contractCode}

var output = solc.compile({sources: input});

var contractBinary = output.contracts['MyContract:test'].bytecode;

var transaction = {
  from: process.argv[2],
  value: '0x0',
  data: contractBinary,
  gas: 200000,
  // Where is the "to" field?!
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
