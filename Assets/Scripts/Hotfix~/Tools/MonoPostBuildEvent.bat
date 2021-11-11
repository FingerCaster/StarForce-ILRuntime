echo off
set currentDir=%~dp0
set targetDir=%1
set hotfixDllDir=%currentDir%..\..\..\..\Res\HotfixDll\
if not exist %hotfixDllDir% ( 
	md %hotfixDllDir% 
)
%currentDir%pdb2mdb.exe %targetDir%Hotfix.dll
copy %targetDir%Hotfix.dll %hotfixDllDir%Hotfix.dll
copy %targetDir%Hotfix.pdb %hotfixDllDir%Hotfix.pdb
copy %targetDir%Hotfix.dll.mdb %hotfixDllDir%Hotfix.dll.mdb
copy %targetDir%Hotfix.dll %hotfixDllDir%Hotfix.dll.bytes
copy %targetDir%Hotfix.pdb %hotfixDllDir%Hotfix.pdb.bytes