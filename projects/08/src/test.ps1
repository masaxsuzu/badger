function Out_As_UTF8 {
    param (
        $content,$path
    )
    $UTF8woBOM = New-Object "System.Text.UTF8Encoding" -ArgumentList @($false)
    [System.IO.File]::WriteAllLines((Join-Path $PWD $path), @($content), $UTF8woBOM)
}

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\ProgramFlow\FibonacciSeries\FibonacciSeries.vm)
Out_As_UTF8 $asm ..\ProgramFlow\FibonacciSeries\FibonacciSeries.asm
..\..\..\tools\CPUEmulator.bat ..\ProgramFlow\FibonacciSeries\FibonacciSeries.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\ProgramFlow\BasicLoop\BasicLoop.vm)
Out_As_UTF8 $asm ..\ProgramFlow\BasicLoop\BasicLoop.asm
..\..\..\tools\CPUEmulator.bat ..\ProgramFlow\BasicLoop\BasicLoop.tst

# Regression tests
$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\..\07\StackArithmetic\SimpleAdd\SimpleAdd.vm)
Out_As_UTF8 $asm ..\..\07\StackArithmetic\SimpleAdd\SimpleAdd.asm
..\..\..\tools\CPUEmulator.bat ..\..\07\StackArithmetic\SimpleAdd\SimpleAdd.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\..\07\StackArithmetic\StackTest\StackTest.vm)
Out_As_UTF8 $asm ..\..\07\StackArithmetic\StackTest\StackTest.asm
..\..\..\tools\CPUEmulator.bat ..\..\07\StackArithmetic\StackTest\StackTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\..\07\MemoryAccess\BasicTest\BasicTest.vm)
Out_As_UTF8 $asm ..\..\07\MemoryAccess\BasicTest\BasicTest.asm
..\..\..\tools\CPUEmulator.bat ..\..\07\MemoryAccess\BasicTest\BasicTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\..\07\MemoryAccess\PointerTest\PointerTest.vm)
Out_As_UTF8 $asm ..\..\07\MemoryAccess\PointerTest\PointerTest.asm
..\..\..\tools\CPUEmulator.bat ..\..\07\MemoryAccess\PointerTest\PointerTest.tst

$asm = $(dotnet run --project .\Netsoft.Badger.Compiler.Backend2.csproj ..\..\07\MemoryAccess\StaticTest\StaticTest.vm)
Out_As_UTF8 $asm ..\..\07\MemoryAccess\StaticTest\StaticTest.asm
..\..\..\tools\CPUEmulator.bat ..\..\07\MemoryAccess\StaticTest\StaticTest.tst
