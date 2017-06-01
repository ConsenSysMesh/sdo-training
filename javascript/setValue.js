var solc = require('solc');
var Web3 = require('web3');
var web3 = new Web3();

if (process.argv.length < 4) {
  console.log('\nUsage: getValue.js <from> <contract address>\n');
  process.exit(0);
}

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

var contractCode = 'pragma solidity ^0.4.0; contract test { uint value; function setValue(uint pValue) { value = pValue; } function getValue() returns (uint value) { return value;} }';

var input = {'MyContract': contractCode}

var output = solc.compile({sources: input});

var abi = output.contracts['MyContract:test'].interface;

var ABI = JSON.parse(abi);

var contract = web3.eth.contract(ABI).at(process.argv[3]);

contract.setValue(1234, {from: process.argv[2]}, function(err, txid) {
  if (err) {
    console.log('\nError: ' + err + '\n');
  } else {
    console.log('\nTxid: ' + txid + '\n');
  }
})
