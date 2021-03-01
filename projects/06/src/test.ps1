dotnet run --  ..\add\Add.asm > ..\add\Add.got.hack
dotnet run --  ..\max\MaxL.asm > ..\max\MaxL.got.hack
dotnet run --  ..\max\Max.asm > ..\max\Max.got.hack
dotnet run --  ..\pong\PongL.asm > ..\pong\PongL.got.hack
dotnet run --  ..\pong\Pong.asm > ..\pong\Pong.got.hack
dotnet run --  ..\rect\RectL.asm > ..\rect\RectL.got.hack
dotnet run --  ..\rect\Rect.asm > ..\rect\Rect.got.hack

..\..\..\tools\Assembler.bat ..\max\Max.asm
..\..\..\tools\Assembler.bat ..\max\MaxL.asm
..\..\..\tools\Assembler.bat ..\pong\Pong.asm
..\..\..\tools\Assembler.bat ..\pong\PongL.asm
..\..\..\tools\Assembler.bat ..\rect\Rect.asm 
..\..\..\tools\Assembler.bat ..\rect\RectL.asm

echo "Add";   Compare-Object $(cat ..\add\Add.hack) $(cat ..\add\Add.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "MaxL";  Compare-Object $(cat ..\max\MaxL.hack) $(cat ..\max\MaxL.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "Max";   Compare-Object $(cat ..\max\Max.hack) $(cat ..\max\Max.got.hack)  | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "PongL"; Compare-Object $(cat ..\pong\PongL.hack) $(cat ..\pong\PongL.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "Pong";  Compare-Object $(cat ..\pong\Pong.hack) $(cat ..\pong\Pong.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "RectL"; Compare-Object $(cat ..\rect\RectL.hack) $(cat ..\rect\RectL.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
echo "Rect";  Compare-Object $(cat ..\rect\Rect.hack) $(cat ..\rect\Rect.got.hack) | Select-Object -Property @{Name = 'ReadCount'; Expression = {$_.InputObject.ReadCount}}, * | Sort-Object -Property ReadCount
