package net.consensys.sample;

import java.io.File;
import java.math.BigInteger;
import java.util.Optional;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

import org.bouncycastle.util.encoders.Hex;
import org.web3j.abi.datatypes.Address;
import org.web3j.abi.datatypes.generated.Uint256;
import org.web3j.crypto.Credentials;
import org.web3j.crypto.WalletUtils;
import org.web3j.protocol.Web3j;
import org.web3j.protocol.core.DefaultBlockParameterName;
import org.web3j.protocol.core.methods.request.EthFilter;
import org.web3j.protocol.core.methods.response.EthSendTransaction;
import org.web3j.protocol.core.methods.response.TransactionReceipt;
import org.web3j.protocol.http.HttpService;
import org.web3j.tx.RawTransactionManager;
import org.web3j.tx.TransactionManager;

/**
 * Sample code showing how to create an account, deploy a smart contract, call it and listen to its transactions.
 * 
 * For didactic purposes it was writen using structured programming, instead of objected oriented.
 * 
 * So, don't reuse this code. When implementing a real app, I suggest using the web3j generators and their patterns.
 * 
 * @author italo.borssatto@consensys.net
 */
@SuppressWarnings("unused")
public class App {

	private static String contractAddress;

	public static void main(String[] args) throws Exception {
		// To create a new account just set the credentialsFileName to null
		String credentialsFileName = "UTC--2017-05-28T11-23-57.460000000Z--65a51e6d14556d91832fac457d1b648f0641e7a4.json";
		if (credentialsFileName == null) {
			// Local account creation
			credentialsFileName = WalletUtils.generateNewWalletFile("password", new File("."), false);
		}
		System.out.println("Account file: " + credentialsFileName);

		// Loading the account credentials
		Credentials credentials = WalletUtils.loadCredentials("password", new File(".", credentialsFileName));

		String addressFrom = credentials.getAddress();
		System.out.println("Private key: 0x" + Hex.toHexString(credentials.getEcKeyPair().getPrivateKey().toByteArray()));
		// Run: testrpc --account"<private key>,1000000000000000000000"

		// Connecting to a node
		Web3j web3j = Web3j.build(new HttpService("http://sdo-eth-sandbox.com:8545/"));
		// Web3j web3j = Web3j.build(new HttpService("http://localhost:8545/"));
		// Web3j web3j = Web3j.build(new HttpService("http://52.168.38.137:8545/"));

		// To deploy the contract again, set to null the contractAddress
		contractAddress = "0x612d329aea2a4d217d5517d4218e38c709d8efba"; // SDO sandbox
		if (contractAddress == null) {
			// Deploying a contract
			TransactionManager transactionManager = new RawTransactionManager(web3j, credentials);
			EthSendTransaction txSent = transactionManager.sendTransaction(BigInteger.valueOf(25000), BigInteger.valueOf(1000000), null, //
					"6060604052341561000c57fe5b5b60028054600160a060020a03191632600160a060020a0316908117909155600090815260208190526040902061271090555b5b6102078061004f6000396000f300606060405263ffffffff60e060020a600035041663502af4398114610037578063d0a2fe4a14610058578063f8b2cb4f14610076575bfe5b341561003f57fe5b610056600160a060020a03600435166024356100a4565b005b341561006057fe5b610056600160a060020a036004351661017c565b005b341561007e57fe5b610092600160a060020a03600435166101bc565b60408051918252519081900360200190f35b600160a060020a033381166000908152600160205260409020548382169116148015906100e0575060025433600160a060020a03908116911614155b156100eb5760006000fd5b600160a060020a033316600090815260208190526040902054819010156101125760006000fd5b600160a060020a0333811660008181526020818152604080832080548790039055938616808352918490208054860190558351858152935191937fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef929081900390910190a35b5050565b33600160a060020a039081166000908152600160205260409020805473ffffffffffffffffffffffffffffffffffffffff19169183169190911790555b50565b600160a060020a0381166000908152602081905260409020545b9190505600a165627a7a723058201fd6774990bb9a38cf249ed95cc2fdb035eeed3131cd9cf756fbf1c1dba18dd10029", //
					BigInteger.valueOf(0));

			if (txSent.getError() != null) { throw new Exception(txSent.getError().getMessage()); }
			String txHash = txSent.getTransactionHash();
			Optional<TransactionReceipt> transactionReceipt = web3j.ethGetTransactionReceipt(txHash).send().getTransactionReceipt();
			while (!transactionReceipt.isPresent()) {
				Thread.sleep(500);
				transactionReceipt = web3j.ethGetTransactionReceipt(txHash).send().getTransactionReceipt();
			}

			contractAddress = transactionReceipt.get().getContractAddress();
		}
		System.out.println("Smart contract address: " + contractAddress);

		// Calling the smart contract method to get the loyalty points balance from the sender account
		LoyaltySmartContract loyalty = new LoyaltySmartContract(contractAddress, web3j, credentials, BigInteger.valueOf(25000), BigInteger.valueOf(1000000));
		Uint256 balance = loyalty.getBalance(new Address(addressFrom)).get();
		System.out.println("Sender balance (loyalty points): " + balance.getValue());

		// Listening to the smart contract transactions using a filter, in another thread!
		ExecutorService executor = Executors.newSingleThreadExecutor();
		executor.execute(() -> {
			EthFilter ethFilter = new EthFilter(DefaultBlockParameterName.EARLIEST, DefaultBlockParameterName.LATEST, contractAddress);
			web3j.ethLogObservable(ethFilter).subscribe(log -> System.out.println("Transfer executed: " + log.getTransactionHash()));
		});

		// Calling the smart contract method to transfer 10 loyalty points to the receiver account
		String addressTo = "0x1733622a7c5ec85f7a7b26dd03842c87037c5e71";
		System.out.println("Sending 10  loyalty points to receiver...");
		loyalty.sendLoyaltyPoints(new Address(addressTo), new Uint256(10)).get();
		balance = loyalty.getBalance(new Address(addressTo)).get();
		System.out.println("Receiver balance (loyalty points): " + balance.getValue());

		// Calling the smart contract method to get the loyalty points balance from the sender account
		balance = loyalty.getBalance(new Address(addressFrom)).get();
		System.out.println("Sender balance (loyalty points): " + balance.getValue());

		// Stops the filter thread
		executor.awaitTermination(5, TimeUnit.SECONDS);
		System.exit(0);
	}
}
