pragma solidity ^0.4.4;

contract LoyaltyPoints {

	//The address of the asigned owner of the contract
	address owner;

	//key value pair of a customer address and their correspondent balances
	mapping (address => uint) balances;
	//key value pair of a customer address and his assigned family member, which he is allowed to transfer funds
	mapping (address=>address) familyMembers;
	
	//Transfer event, the information is logged after loyalty points has been sent
	event Transfer(address indexed from, address indexed to, uint256 value);

	//Constructor of the contract
	function LoyaltyPoints() {
		//assign the creator of the contract (msg.sender) as the owner of the contract
		owner = msg.sender;
		//the contract and owner has a limited amount of loyalty points
		balances[msg.sender] = 10000;
	}

	//function to assign a family member to a customer account
	function registerFamilyMember(address familyMember) {
		familyMembers[msg.sender] = familyMember;
	}

	function sendLoyaltyPoints(address receiver, uint amount) {
		//Validate that the account we are transfering the loyalty points is a registered family member
		//or the sender is the owner of the contract, hence is allowed to send to any customer
		if(familyMembers[msg.sender] != receiver && msg.sender != owner) throw;
		
		if (balances[msg.sender] < amount) throw;
		balances[msg.sender] -= amount;
		balances[receiver] += amount;
		
		//when the transfer has been completed, log the event for information
		Transfer(msg.sender, receiver, amount);
	}

	//function to return the balance of an account
	function getBalance(address addr) returns(uint) {
		return balances[addr];
	}
}
