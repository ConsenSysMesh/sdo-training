using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace LoyaltyPoints.Sample
{
    public class LoyaltyPointsSample
    {
        private string password;
        private string filePath;

        public LoyaltyPointsSample(string password, string filePath)
        {
            this.password = password;
            this.filePath = filePath;
        }

        public async Task DeployCallAndSendTransaction()
        {
            //The abi contract definition
            var abi = @"[{'constant':false,'inputs':[{'name':'receiver','type':'address'},{'name':'amount','type':'uint256'}],'name':'sendLoyaltyPoints','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'familyMember','type':'address'}],'name':'registerFamilyMember','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'addr','type':'address'}],'name':'getBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'type':'function'},{'inputs':[],'payable':false,'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Transfer','type':'event'}]";

            //The compiled contract
            var byteCode = "0x6060604052341561000c57fe5b5b60028054600160a060020a03191632600160a060020a0316908117909155600090815260208190526040902061271090555b5b6102078061004f6000396000f300606060405263ffffffff60e060020a600035041663502af4398114610037578063d0a2fe4a14610058578063f8b2cb4f14610076575bfe5b341561003f57fe5b610056600160a060020a03600435166024356100a4565b005b341561006057fe5b610056600160a060020a036004351661017c565b005b341561007e57fe5b610092600160a060020a03600435166101bc565b60408051918252519081900360200190f35b600160a060020a033381166000908152600160205260409020548382169116148015906100e0575060025433600160a060020a03908116911614155b156100eb5760006000fd5b600160a060020a033316600090815260208190526040902054819010156101125760006000fd5b600160a060020a0333811660008181526020818152604080832080548790039055938616808352918490208054860190558351858152935191937fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef929081900390910190a35b5050565b33600160a060020a039081166000908152600160205260409020805473ffffffffffffffffffffffffffffffffffffffff19169183169190911790555b50565b600160a060020a0381166000908152602081905260409020545b9190505600a165627a7a723058201fd6774990bb9a38cf249ed95cc2fdb035eeed3131cd9cf756fbf1c1dba18dd10029";

            //new instance of Web3

            //loading from an encrypted file using a password
            var account = Account.LoadFromKeyStoreFile(filePath, password);

            //connecting to a local instance using test-rpc, geth, parity on localhost
            var web3 = new Web3(account, "http://localhost:8545");

            var addressFrom = account.Address;

            //Deploy the contract
            var txnHash = await web3.Eth.DeployContract.SendRequestAsync(byteCode, addressFrom, new HexBigInteger(250000));

            //retrieve the transaction receipt of the deployment
            var txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);

            while (txnReceipt == null)
            {
                Thread.Sleep(500);
                txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);
            }

            //the transaction receipt contains the address of the deployed contract
            var contract = web3.Eth.GetContract(abi, txnReceipt.ContractAddress);

            //New instance of the "getBalance" smart contract function
            var functionGetBalance = contract.GetFunction("getBalance");

            //CALLING A CONTRACT FUNCTION
            //Retrieve the balance of the owner of the contract,  which is the address that deployed the smart contract
            var balanceOwner = await functionGetBalance.CallAsync<uint>(addressFrom);

            //A customer address to send some loyalty points
            var addressTo = "0x6fc17cfd738ebec0159d952553c71272ec57e50a";

            //Creating an instance wrapper of the "sendLoyaltyPoints" smartcontract function
            var functionSendLoyaltyPoints = contract.GetFunction("sendLoyaltyPoints");

            //The  owner of the contract can now send some loyalty points to a customer
            txnHash = await functionSendLoyaltyPoints.SendTransactionAsync(addressFrom, new HexBigInteger(250000), null, addressTo, 10);

            //wait for the transaction to be "mined" and persisted on the blockchain
            txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);

            while (txnReceipt == null)
            {
                Thread.Sleep(500);
                txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);
            }

            // as an owner should hold now only 9990 loyalty points
            balanceOwner = await functionGetBalance.CallAsync<uint>(addressFrom);
            
            //The customer should have now 10 loyalty points
            var balanceTo = await functionGetBalance.CallAsync<uint>(addressTo);

            var transferEvent = contract.GetEvent("Transfer");
            //the logs for the event "Transfer" should contain one log entry for  as the sender for the value of 10
            var logs = await web3.Eth.Filters.GetLogs.SendRequestAsync(transferEvent.CreateFilterInput(new object[] { addressFrom }));
            var events = Event.DecodeAllEvents<TransferEventDTO>(logs);

        }

        public class TransferEventDTO
        {
            [Parameter("address", "from", 1, true)]
            public string From { get; set; }

            [Parameter("address", "to", 2, true)]
            public string To { get; set; }

            [Parameter("uint256", "value", 3, false)]
            public BigInteger Value { get; set; }
        }
    }
}
