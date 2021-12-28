@echo off
path "c:\Program Files\WinRAR\"
d:
cd\bak
for %%i in (*.bak) do rar a -df -tl %%i.rar %%i