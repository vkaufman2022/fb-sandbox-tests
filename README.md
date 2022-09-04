# fb-sandbox-tests

Prerequisites
In order to successfully run the project please fill in project appsetting.json:
"bearer": "your email address".

- Visual Studio Installed
- .NETCore 6.0 Installed
- Test project uses the followinng packages:
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="6.0.1" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="6.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.3.1" />
    <PackageReference Include="SpecFlow" Version="3.9.74" />
    <PackageReference Include="SpecFlow.Assist.Dynamic" Version="1.4.2" />
    <PackageReference Include="SpecFlow.NUnit" Version="3.9.74" />
    <PackageReference Include="SpecFlow.NUnit.Runners" Version="3.9.74" />
    <PackageReference Include="SpecFlow.Plus.LivingDocPlugin" Version="3.9.57" />
    <PackageReference Include="SpecFlow.Tools.MsBuild.Generation" Version="3.9.74" />

1. run all tests from command line
go to path \GitHub\fb-sandbox-tests\fb-sandbox-tests
dotnet clean fb-sandbox-tests.sln
dotnet build fb-sandbox-tests.sln
go to path \fb-sandbox-tests\fb-sandbox-tests\fb-sandbox-tests\bin\Debug\net6.0
dotnet test -v d fbsandboxtests.dll
 
2. run tests via Visual Studio - https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2022

3. generate test report
install specflow living doc CLI - https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Installing-the-command-line-tool.html
go to path \fb-sandbox-tests\fb-sandbox-tests\fb-sandbox-tests\bin\Debug\net6.0
livingdoc test-assembly fbsandboxtests.dll -t TestExecution.json
LivingDoc.html report will be created

4. get list of tests
go to path \fb-sandbox-tests\fb-sandbox-tests\fb-sandbox-tests\bin\Debug\net6.0
dotnet test --no-build --list-tests fbsandboxtests.dll

