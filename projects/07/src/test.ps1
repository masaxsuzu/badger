function Out_As_UTF8 {
    param (
        $content,$path
    )
    $UTF8woBOM = New-Object "System.Text.UTF8Encoding" -ArgumentList @($false)
    [System.IO.File]::WriteAllLines((Join-Path $PWD $path), @($content), $UTF8woBOM)
}

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\StackArithmetic\SimpleAdd\SimpleAdd.vm)
Out_As_UTF8 $asm ..\StackArithmetic\SimpleAdd\SimpleAdd.asm
..\..\..\tools\CPUEmulator.bat ..\StackArithmetic\SimpleAdd\SimpleAdd.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\StackArithmetic\StackTest\StackTest.vm)
Out_As_UTF8 $asm ..\StackArithmetic\StackTest\StackTest.asm
..\..\..\tools\CPUEmulator.bat ..\StackArithmetic\StackTest\StackTest.tst
