# Custom Multicycle DataPath Design 

Custom Multicycle DataPath Design on Quartus II 13.0sp1.

The multicycle microarchitecture is based on Dr.Asadi's *Computer Architecture* slides.

## Contributors

1. [Ahmad Salimi](https://github.com/ahmadsalimi)
2. [Hamila Mailee](https://github.com/hamilamailee)
3. [Saber Zafarpoor](https://github.com/SaberDoTcodeR)

## Hierarchical Design

### High-Level Abstraction
![DataPath](images/Datapath.jpg)
This is the Highest Level Abstraction of our design. We have several components that will be explained.

#### Datapath modules:
- **PC**: A 32-bit register that stores current Program Counter.
- **Memory**: A ROM with 20-bit words for storing instructions.
- **IR**: A 20-bit register that stores current executing instruction and decodes each part of instruction (`opcode, cin, in1, in2, out`).
- **Control**: A Finite State Machine that adjusts control signals, such as `IRWrite`, `PCWrite`, `RegWrite` `ALUSrcA`, `ALUSrcB`, `ALUOp`, and `Li` according to `Opcode` and previous state. The FSM diagram is as shown below.

![Finite State Machine](images/Control_FSM.png)

The control unit was designed by one-hot method to convert above FSM to the below circuit.

![Control](images/Control.jpg)

- **RF**: A Register File that has 32 Registers with width of 32 bits.
- **Zero Extend**: extends 5-bit input to a 32-bit bus.
- **ALU**: *Hamila*

#### Instruction Execution Stages:
|  Cycle |   1   |   2   |  3  |  4 |
|:------:|:-----:|:-----:|:---:|:--:|
| R-type | IF-PC | ID-RF | ALU | WB |
|   Li   | IF-PC |   ID  |  WB |    |
|  No-Op | IF-PC |   ID  |     |    |

- Instruction Fetch, PC assignment:
    - `IR <- Mem[PC]`
    - `PC <- PC + 1`

![IF-PC](images/IF-PC.jpg)

- Instruction Decode, Register File:
    - `A <- GPR[in1]`
    - `B <- GPR[in2]`

![ID-RF](images/ID-RF.jpg)

- ALU execution:

ALU executes the instruction. [Control unit](#control-unit) specifies type of operation.

![EXE](images/EXE.jpg)

- Write Back:

`Li` signal from [Control unit](#control-unit) chooses `AluOut` of zero-extended `shiftamt` value in RF.

![WB](images/WB.jpg)
