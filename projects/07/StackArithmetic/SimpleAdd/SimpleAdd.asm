// Push 7
@7
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 8
@8
D=A
@SP
A=M
M=D
@SP
M=M+1
// POP left
@SP
A=M-1
D=M
@SP
M=M-1
// POP right
@SP
A=M-1
D=D+M
@SP
M=M-1
// Push a+b
A=M
M=D
@SP
M=M+1
