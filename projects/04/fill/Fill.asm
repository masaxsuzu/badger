// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

//  state = 0;
//  while (true) {
//      if(key) {
//          color = 1;
//      }   else {
//          color = 0;
//      }
//      if state == color continue;
//      state = color;
//      for pixel in screen {
//          pixel = color;
//      }
//  }

    @8192
    D=A
    @number_of_pixel
    M=D
    @R0
    M=0
(CHECK_KEY)
    @KBD
    D=M
    @KEY_NOT_PRESSED
    D;JEQ
(KEY_PRESSED)
    D=M
    @R1
    M=1
    @CHECK_STATE
    D;JMP
(KEY_NOT_PRESSED)
    D=M
    @R1
    M=0
    @CHECK_STATE
    D;JMP
(CHECK_STATE)
    @R0
    D=M
    @R1
    D=D-M
    @CHECK_KEY
    D;JEQ
    @i
    M=0;
    @R1
    D=M
    @R0
    M=D
    @FILL_WHITE
    D;JEQ
    @FILL_BLACK
    D;JMP
(FILL_WHITE)
    @i
    D=M
    @number_of_pixel
    D=M-D
    @CHECK_KEY
    D;JLT
    @SCREEN
    A=A+D
    M=0
    @i
    MD=M+1
    @FILL_WHITE
    0;JMP
(FILL_BLACK)
    @i
    D=M
    @number_of_pixel
    D=M-D
    @CHECK_KEY
    D;JLT
    @SCREEN
    A=A+D
    M=-1
    @i
    MD=M+1
    @FILL_BLACK
    0;JMP
