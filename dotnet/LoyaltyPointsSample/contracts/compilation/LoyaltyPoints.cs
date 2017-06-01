using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;

namespace DefaultNamespace
{
   public class LoyaltyPointsService
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'receiver','type':'address'},{'name':'amount','type':'uint256'}],'name':'sendLoyaltyPoints','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'familyMember','type':'address'}],'name':'registerFamilyMember','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[],'name':'EmiratesLoyaltyPoints','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'addr','type':'address'}],'name':'getBalance','outputs':[{'name':'','type':'uint256'}],'payable':false,'type':'function'},{'anonymous':false,'inputs':[{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Transfer','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052341561000c57fe5b5b6102638061001c6000396000f300606060405263ffffffff60e060020a600035041663502af4398114610042578063d0a2fe4a14610063578063e704025214610081578063f8b2cb4f14610093575bfe5b341561004a57fe5b610061600160a060020a03600435166024356100c1565b005b341561006b57fe5b610061600160a060020a036004351661019b565b005b341561008957fe5b6100616101db565b005b341561009b57fe5b6100af600160a060020a0360043516610218565b60408051918252519081900360200190f35b600160a060020a033381166000908152600260205260409020548382169116148015906100fd575060005433600160a060020a03908116911614155b156101085760006000fd5b600160a060020a0333166000908152600160205260409020548190101561012f5760006000fd5b600160a060020a03338116600081815260016020908152604080832080548790039055938616808352918490208054860190558351858152935191937fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef929081900390910190a35b5050565b33600160a060020a039081166000908152600260205260409020805473ffffffffffffffffffffffffffffffffffffffff19169183169190911790555b50565b6000805473ffffffffffffffffffffffffffffffffffffffff191633600160a060020a03169081178255815260016020526040902061271090555b565b600160a060020a0381166000908152600160205260409020545b9190505600a165627a7a723058206e9f1396cddc64f03908a9dc385c2fe1e8970d3e53f5ea7f47d4067746bcffa50029";

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
        public Function GetFunctionEmiratesLoyaltyPoints() {
            return contract.GetFunction("EmiratesLoyaltyPoints");
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
        public Task<string> EmiratesLoyaltyPointsAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionEmiratesLoyaltyPoints();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
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

