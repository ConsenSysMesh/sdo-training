using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LoyaltyPoints.Sample
{
    public class Program
    {
        public const string accountFile = "UTC--2017-05-28T11-21-49.1242038Z--FbC7FBBd38c831B15D217C651639eB715551A989";
        public const string password = "password";

        /*The file stores a private key: 0x33513f89bdf4c6681fc42b0b6149d825339baed391a55b5d88b1958f9917b907
        we can use the account private key directly
        and start test rpc as follows 
        testrpc --account="0x33513f89bdf4c6681fc42b0b6149d825339baed391a55b5d88b1958f9917b907,55000000000000000000000"
        */
        static void Main(string[] args)
        {
            //to create an account
            //var accountFile =  new AcccountCreator().CreateAccount("password", Environment.CurrentDirectory);

            var loyaltyPoints = new LoyaltyPointsSample(password, accountFile);
            loyaltyPoints.DeployCallAndSendTransaction().Wait();
        }
    }
}
