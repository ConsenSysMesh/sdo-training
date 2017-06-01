using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace LoyaltyPoints.Sample
{
    /// <summary>
    /// This is the generated service from the byte code to simplify integration with Ethereum
    /// </summary>
   public class LoyaltyPointsService
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'receiver','type':'address'},{'name':'amount','type':'uint256'}],'name':'sendLoyaltyPoints','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'familyMember','type':'address'}],'name':'registerFamilyMember','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'addr','type':'address'}],'name':'getBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'type':'function'},{'inputs':[],'payable':false,'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Transfer','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052341561000c57fe5b5b60028054600160a060020a03191632600160a060020a0316908117909155600090815260208190526040902061271090555b5b6102078061004f6000396000f300606060405263ffffffff60e060020a600035041663502af4398114610037578063d0a2fe4a14610058578063f8b2cb4f14610076575bfe5b341561003f57fe5b610056600160a060020a03600435166024356100a4565b005b341561006057fe5b610056600160a060020a036004351661017c565b005b341561007e57fe5b610092600160a060020a03600435166101bc565b60408051918252519081900360200190f35b600160a060020a033381166000908152600160205260409020548382169116148015906100e0575060025433600160a060020a03908116911614155b156100eb5760006000fd5b600160a060020a033316600090815260208190526040902054819010156101125760006000fd5b600160a060020a0333811660008181526020818152604080832080548790039055938616808352918490208054860190558351858152935191937fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef929081900390910190a35b5050565b33600160a060020a039081166000908152600160205260409020805473ffffffffffffffffffffffffffffffffffffffff19169183169190911790555b50565b600160a060020a0381166000908152602081905260409020545b9190505600a165627a7a72305820dfc8b30c1237061c6d048e312e2446f5f66b144a01a43606e821133b9648b8680029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount );
        }   

        private Contract contract;

        public LoyaltyPointsService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        public Function GetFunctionSendLoyaltyPoints() {
            return contract.GetFunction("sendLoyaltyPoints");
        }
        public Function GetFunctionRegisterFamilyMember() {
            return contract.GetFunction("registerFamilyMember");
        }
        public Function GetFunctionGetBalance() {
            return contract.GetFunction("getBalance");
        }

        public Event GetEventTransfer() {
            return contract.GetEvent("Transfer");
        }

        public Task<BigInteger> GetBalanceAsyncCall(string addr) {
            var function = GetFunctionGetBalance();
            return function.CallAsync<BigInteger>(addr);
        }

        public Task<string> SendLoyaltyPointsAsync(string addressFrom, string receiver, BigInteger amount, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSendLoyaltyPoints();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, receiver, amount);
        }
        public Task<string> RegisterFamilyMemberAsync(string addressFrom, string familyMember, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionRegisterFamilyMember();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, familyMember);
        }
        public Task<string> GetBalanceAsync(string addressFrom, string addr, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionGetBalance();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, addr);
        }
    }

    public class TransferEventDTO 
    {
        [Parameter("address", "from", 1, true)]
        public string From {get; set;}

        [Parameter("address", "to", 2, true)]
        public string To {get; set;}

        [Parameter("uint256", "value", 3, false)]
        public BigInteger Value {get; set;}
    }
}

