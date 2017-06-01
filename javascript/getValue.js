var solc = require('solc');
var Web3 = require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

if (process.argv.length < 3) {
  console.log('\nUsage: getValue.js <contract address>\n');
  process.exit(0);
}

var contractCode = 'pragma solidity ^0.4.0; contract test { uint value; function setValue(uint pValue) { value = pValue; } function getValue() returns (uint value) { return value;} }';

var input = {'MyContract': contractCode}

var output = solc.compile({sources: input});

var abi = output.contracts['MyContract:test'].interface;

var ABI = JSON.parse(abi);

var contract = web3.eth.contract(ABI).at(process.argv[2]);

contract.getValue.call(function(err, value) {
  if (err) {
    console.log('\nError: ' + err + '\n');
  } else {
    console.log('\nValue: ' + value + '\n');
  }
})
