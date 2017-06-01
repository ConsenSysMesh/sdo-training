pragma solidity ^0.4.4;

contract EmirateLoyaltyPoints {

    mapping (address => uint) balances;
    mapping (address=>address) familyMembers;
    address owner;

    event Transfer(address indexed from, address indexed to, uint256 value);

    function LoyaltyPoints() {
        owner = tx.origin;
        balances[tx.origin] = 10000;
    }

    function registerFamilyMember(address familyMember) {
        familyMembers[msg.sender] = familyMember;
    }

    function sendLoyaltyPoints(address receiver, uint amount) {
        if(familyMembers[msg.sender] != receiver && msg.sender != owner) throw;
        if (balances[msg.sender] < amount) throw;
        balances[msg.sender] -= amount;
        balances[receiver] += amount;
        Transfer(msg.sender, receiver, amount);
    }

    function getBalance(address addr) returns(uint) {
        return balances[addr];
    }
}