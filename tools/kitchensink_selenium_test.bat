IF "%Configuration%"=="" set Configuration=Debug

star -d=kitchensink --resourcedir="%~dp0src\KitchenSink\wwwroot" "%~dp0bin/%Configuration%/KitchenSink.exe"
IF ERRORLEVEL 1 EXIT /b 1

packages\NUnit.ConsoleRunner.3.2.0\tools\nunit3-console.exe test\KitchenSink.Tests\KitchenSink.Tests.csproj /config:%Configuration%
IF ERRORLEVEL 1 EXIT /b 1