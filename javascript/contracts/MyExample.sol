import "Example.sol";

contract MyExample is Example {

  address owner = msg.sender;

  public increment() {
    if (owner == msg.sender) {
      value++;
    }
  }

}
