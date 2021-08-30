function Out_As_UTF8 {
    param (
        $content,$path
    )
    $UTF8woBOM = New-Object "System.Text.UTF8Encoding" -ArgumentList @($false)
    [System.IO.File]::WriteAllLines((Join-Path $PWD $path), @($content), $UTF8woBOM)
}
dotnet format

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\StackArithmetic\SimpleAdd\SimpleAdd.vm)
Out_As_UTF8 $asm ..\StackArithmetic\SimpleAdd\SimpleAdd.asm
..\..\..\tools\CPUEmulator.bat ..\StackArithmetic\SimpleAdd\SimpleAdd.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\StackArithmetic\StackTest\StackTest.vm)
Out_As_UTF8 $asm ..\StackArithmetic\StackTest\StackTest.asm
..\..\..\tools\CPUEmulator.bat ..\StackArithmetic\StackTest\StackTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\MemoryAccess\BasicTest\BasicTest.vm)
Out_As_UTF8 $asm ..\MemoryAccess\BasicTest\BasicTest.asm
..\..\..\tools\CPUEmulator.bat ..\MemoryAccess\BasicTest\BasicTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\MemoryAccess\PointerTest\PointerTest.vm)
Out_As_UTF8 $asm ..\MemoryAccess\PointerTest\PointerTest.asm
..\..\..\tools\CPUEmulator.bat ..\MemoryAccess\PointerTest\PointerTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend.csproj ..\MemoryAccess\StaticTest\StaticTest.vm)
Out_As_UTF8 $asm ..\MemoryAccess\StaticTest\StaticTest.asm
..\..\..\tools\CPUEmulator.bat ..\MemoryAccess\StaticTest\StaticTest.tst
