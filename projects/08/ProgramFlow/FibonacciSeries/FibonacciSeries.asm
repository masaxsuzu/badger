// File FibonacciSeries.vm
// Push argument 1
@ARG
A=M
A=A+1
D=M
@SP
A=M
M=D
@SP
M=M+1
// Pop pointer 1
@SP
M=M-1
A=M
D=M
@3
A=A+1
M=D
// Push constant 0
@0
D=A
@SP
A=M
M=D
@SP
M=M+1
// Pop that 0
@SP
M=M-1
A=M
D=M
@THAT
A=M
M=D
// Push constant 1
@1
D=A
@SP
A=M
M=D
@SP
M=M+1
// Pop that 1
@SP
M=M-1
A=M
D=M
@THAT
A=M
A=A+1
M=D
// Push argument 0
@ARG
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push constant 2
@2
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
D=M-D
@SP
A=M
M=D
@SP
M=M+1
// Pop argument 0
@SP
M=M-1
A=M
D=M
@ARG
A=M
M=D
// Label argument MAIN_LOOP_START
(MAIN_LOOP_START)
// Push argument 0
@ARG
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// If-Goto argument COMPUTE_ELEMENT
@SP
M=M-1
A=M
D=M
@COMPUTE_ELEMENT
D;JNE
// Goto argument END_PROGRAM
@END_PROGRAM
0;JMP
// Label argument COMPUTE_ELEMENT
(COMPUTE_ELEMENT)
// Push argument 0
@THAT
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push argument 1
@THAT
A=M
A=A+1
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
// Pop that 2
@SP
M=M-1
A=M
D=M
@THAT
A=M
A=A+1
A=A+1
M=D
@3
A=A+1
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push constant 1
@1
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
// Pop pointer 1
@SP
M=M-1
A=M
D=M
@3
A=A+1
M=D
// Push argument 0
@ARG
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push constant 1
@1
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
D=M-D
@SP
A=M
M=D
@SP
M=M+1
// Pop argument 0
@SP
M=M-1
A=M
D=M
@ARG
A=M
M=D
// Goto argument MAIN_LOOP_START
@MAIN_LOOP_START
0;JMP
// Label argument END_PROGRAM
(END_PROGRAM)
