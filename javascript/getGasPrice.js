var Web3 = require('web3');
var web3 = new Web3();

web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));

var gas = web3.eth.gasPrice;

console.log('GasPrice: ' + gas);
console.log('GasPrice (hex): ' + web3.toHex(''+gas));
