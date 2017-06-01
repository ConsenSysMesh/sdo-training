pragma solidity ^0.4.0;

contract Example {

  uint value;

  function setValue(uint pValue) {
    value = pValue;
  }

  function getValue() returns(uint) {
    return value;
  }

}
