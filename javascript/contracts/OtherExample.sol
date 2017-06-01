import "MyExample.sol"

contract OtherExample is MyExample {

  event ValueIncremented(address indexed who, uint value);

  function increment() {
    if (msg.sender == owner) {
      value++;
    }
    ValueIncremented(msg.sender, value);
  }

}
