// File SimpleAdd.vm
// Push constant 7
@7
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push constant 8
@8
D=A
@SP
A=M
M=D
@SP
M=M+1
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M+D
@SP
A=M
M=D
@SP
M=M+1
