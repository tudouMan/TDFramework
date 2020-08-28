@SET EXCEL_FOLDER=..\..\≈‰÷√±Ì
@SET JSON_FOLDER=..\..\Clinet\Assets\Art\Data
@SET EXE=..\..\Tools\ExcelExport\excel2json.exe

@ECHO Converting excel files in folder %EXCEL_FOLDER% ...
for /f "delims=" %%i in ('dir /b /a-d /s %EXCEL_FOLDER%\*.xlsx') do (
    @echo   processing %%~nxi 
    @CALL %EXE% --excel %EXCEL_FOLDER%\%%~nxi --json %JSON_FOLDER%\%%~ni.json --header 3 --array false
)