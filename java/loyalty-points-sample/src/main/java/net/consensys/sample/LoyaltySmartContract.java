package net.consensys.sample;

import java.math.BigInteger;
import java.util.Arrays;
import java.util.Collections;
import java.util.concurrent.Future;

import org.web3j.abi.TypeReference;
import org.web3j.abi.datatypes.Address;
import org.web3j.abi.datatypes.Function;
import org.web3j.abi.datatypes.Type;
import org.web3j.abi.datatypes.generated.Uint256;
import org.web3j.crypto.Credentials;
import org.web3j.protocol.Web3j;
import org.web3j.protocol.core.methods.response.TransactionReceipt;
import org.web3j.tx.Contract;

public class LoyaltySmartContract extends Contract {
	@SuppressWarnings("deprecation")
	protected LoyaltySmartContract(String contractAddress, Web3j web3j, Credentials credentials, BigInteger gasPrice, BigInteger gasLimit) {
		super(contractAddress, web3j, credentials, gasPrice, gasLimit);
	}

	public Future<Uint256> getBalance(Address addr) {
		@SuppressWarnings("rawtypes")
		Function function = new Function("getBalance", Arrays.<Type> asList(addr), Arrays.<TypeReference<?>> asList(new TypeReference<Uint256>() {}));
		return executeCallSingleValueReturnAsync(function);
	}

	public Future<TransactionReceipt> sendLoyaltyPoints(Address receiver, Uint256 amount) {
		@SuppressWarnings("rawtypes")
		Function function = new Function("sendLoyaltyPoints", Arrays.<Type> asList(receiver, amount), Collections.<TypeReference<?>> emptyList());
		return executeTransactionAsync(function);
	}
}
