// File StaticTest.vm
// Push constant 111
@111
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push constant 333
@333
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push constant 888
@888
D=A
@SP
A=M
M=D
@SP
M=M+1
// Pop static 8
@SP
M=M-1
A=M
D=M
@StaticTest.8
M=D
// Pop static 3
@SP
M=M-1
A=M
D=M
@StaticTest.3
M=D
// Pop static 1
@SP
M=M-1
A=M
D=M
@StaticTest.1
M=D
// Push static 3
@StaticTest.3
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push static 1
@StaticTest.1
D=M
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
D=M-D
@SP
A=M
M=D
@SP
M=M+1
// Push static 8
@StaticTest.8
D=M
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
