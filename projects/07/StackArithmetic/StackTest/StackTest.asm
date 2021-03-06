// Push 17
@17
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 17
@17
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
D=D-M
@SP
M=M-1
// Push a == b
@EQ0
D;JEQ
@SP
A=M
M=0
@EQ_END0
0;JMP
(EQ0)
@SP
A=M
M=-1
(EQ_END0)
@SP
M=M+1
// Push 17
@17
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 16
@16
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
D=D-M
@SP
M=M-1
// Push a == b
@EQ1
D;JEQ
@SP
A=M
M=0
@EQ_END1
0;JMP
(EQ1)
@SP
A=M
M=-1
(EQ_END1)
@SP
M=M+1
// Push 16
@16
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 17
@17
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
D=D-M
@SP
M=M-1
// Push a == b
@EQ2
D;JEQ
@SP
A=M
M=0
@EQ_END2
0;JMP
(EQ2)
@SP
A=M
M=-1
(EQ_END2)
@SP
M=M+1
// Push 892
@892
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 891
@891
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
D=M-D
@SP
M=M-1
// Push a < b
@LT3
D;JLT
@SP
A=M
M=0
@LT_END3
0;JMP
(LT3)
@SP
A=M
M=-1
(LT_END3)
@SP
M=M+1
// Push 891
@891
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 892
@892
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
D=M-D
@SP
M=M-1
// Push a < b
@LT4
D;JLT
@SP
A=M
M=0
@LT_END4
0;JMP
(LT4)
@SP
A=M
M=-1
(LT_END4)
@SP
M=M+1
// Push 891
@891
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 891
@891
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
D=M-D
@SP
M=M-1
// Push a < b
@LT5
D;JLT
@SP
A=M
M=0
@LT_END5
0;JMP
(LT5)
@SP
A=M
M=-1
(LT_END5)
@SP
M=M+1
// Push 32767
@32767
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 32766
@32766
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
D=M-D
@SP
M=M-1
// Push b > a
@GT6
D;JGT
@SP
A=M
M=0
@GT_END6
0;JMP
(GT6)
@SP
A=M
M=-1
(GT_END6)
@SP
M=M+1
// Push 32766
@32766
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 32767
@32767
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
D=M-D
@SP
M=M-1
// Push b > a
@GT7
D;JGT
@SP
A=M
M=0
@GT_END7
0;JMP
(GT7)
@SP
A=M
M=-1
(GT_END7)
@SP
M=M+1
// Push 32766
@32766
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 32766
@32766
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
D=M-D
@SP
M=M-1
// Push b > a
@GT8
D;JGT
@SP
A=M
M=0
@GT_END8
0;JMP
(GT8)
@SP
A=M
M=-1
(GT_END8)
@SP
M=M+1
// Push 57
@57
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 31
@31
D=A
@SP
A=M
M=D
@SP
M=M+1
// Push 53
@53
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
// Push 112
@112
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
D=M-D
@SP
M=M-1
// Push a+b
A=M
M=D
@SP
M=M+1
// POP left
@SP
A=M-1
D=-M
@SP
M=M-1
// Push -a
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
D=D&M
@SP
M=M-1
// Push a & b
A=M
M=D
@SP
M=M+1
// Push 82
@82
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
D=D|M
@SP
M=M-1
// Push a | b
A=M
M=D
@SP
M=M+1
// POP left
@SP
A=M-1
D=!M
@SP
M=M-1
// Push !a
@SP
A=M
M=D
@SP
M=M+1
