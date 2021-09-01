// File BasicLoop.vm
// Push constant 0
@0
D=A
@SP
A=M
M=D
@SP
M=M+1
// Pop local 0
@SP
M=M-1
A=M
D=M
@LCL
A=M
M=D
// Label argument LOOP_START
(LOOP_START)
// Push argument 0
@ARG
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// Push local 0
@LCL
A=M
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
// Pop local 0
@SP
M=M-1
A=M
D=M
@LCL
A=M
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
// Push argument 0
@ARG
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
// If-Goto argument LOOP_START
@SP
M=M-1
A=M
D=M
@LOOP_START
D;JNE
// Push local 0
@LCL
A=M
D=M
@SP
A=M
M=D
@SP
M=M+1
