# Introductionn

This folder has some NodeJS scripts that can be run to execute some simple tasks.

# Setting up

In order to speed up the development process, you should first developing your apps
pointing to a [TestRPC](https://github.com/ethereumjs/testrpc) node. TestRPC creates
10 accounts for you and upon startup, print them on the screen. You can use any of 
these accounts to send transactions.

If you use any of the TestRPC pre-created accounts, you would not need to sign your
transactions as they will be signed automatically by the TestRPC itself.

Most scripts are configured to be run against a TestRPC node, as can be seen by the
code below:

```javascript
web3.setProvider(new web3.providers.HttpProvider('http://localhost:8545'));
```

If you want to use another Ethereum node to run the scripts against to, you will need
to change the `http://localhost:8545` code. You will also need to sign your transaction
as is demonstrated by the script `transferValue2.js`.

# NodeJS

Before installing and running the scripts, make sure to install NodeJS on your machine.
Instruction to do so can be found in [this link](https://nodejs.org/en/download/package-manager/).

# Install

In order to run the scripts, you first need to install the required libraries. For doing
that, just run:

```
npm install
```

# Running

To run a script, just type `node <script>` and any other parameter required by it.


