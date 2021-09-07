function Out_As_UTF8 {
    param (
        $content,$path
    )
    $UTF8woBOM = New-Object "System.Text.UTF8Encoding" -ArgumentList @($false)
    [System.IO.File]::WriteAllLines((Join-Path $PWD $path), @($content), $UTF8woBOM)
}

$tokens = $(dotnet run --project .\Netsoft.Badger.Compiler.Frontend.csproj ..\ArrayTest\Main.jack)
Out_As_UTF8 $tokens ..\ArrayTest\MainT.out.xml
..\..\..\tools\TextComparer.bat ..\ArrayTest\MainT.xml ..\ArrayTest\MainT.out.xml
