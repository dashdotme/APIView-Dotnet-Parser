# Dotnet APIView token file generator

## What is this?

This is a repackaging of work in [APIView](https://github.com/Azure/azure-sdk-tools/tree/main/src/dotnet/APIView/APIViewWeb), separated into its own executable module.

APIView is a tool for viewing and reviewing the stubs of source code, with the goal of improving the resulting API. It relies on different parsers for each language to generate a JSON stub file. 

This package was created to solve two small issues:
1) The C# parser, called 'APIView' in the original repository, wasn't wired to work as an executable
2) The C++ and C parsers were built in to the web service, APIViewWeb, direct

Most of the code here can be found in the original 'APIView' folder, except for the C++ and C parsers. Actual changes are minimal. Packages have been renamed for clarity, however.

## Getting started

### Prerequisites

- .Net SDK 3.1 or above

### Build the tool

To build the tool, execute the following command from the root directory of this repository:
```
dotnet build --configuration Release
```

The default build output location for the executable should look something like this:

```
.\DotnetParser\bin\Release\netcoreapp3.1\DotnetParser.exe
```

To access the parser anywhere you can add this path to your $PATH variable.

### Run the tool

The Dotnet parser creates APIView-readable files for a few different languages - C#, C++ and C. The only thing you need to adjust for each is the input.

To run the parser from the root after building, execute the following command:

```
.\path\to\DotnetParser.exe <path to input> <path to output>
```

Any output name is accepted, but the output location must be a folder which already exists.

## Creating the inputs

For C#, currently only nupkg files are supported. To turn your package into a nupkg, run `dotnet pack` from the project folder.

For C++ and C, Clang must be used to generate an abstract syntax tree for the project which the parser can then process. 

### Example command for C++

```
clang++ [input like ...\azure-sdk-for-cpp\sdk\storage\azure-storage-files-datalake\inc\azure\storage\files\datalake.hpp] -Xclang -ast-dump -I .\sdk\storage\azure-storage-files-datalake\inc -I .\sdk\core\azure-core\inc -I .\sdk\storage\azure-storage-common\inc -I .\sdk\storage\azure-storage-blobs\inc > [out name like Azure_Storage_Files_Datalake.cppast]
```

Please note the 'cppast' file extension is essential for later processing by this module.

### Example command for C, from Azure SDK for C:

```
clang [input like az_*.h] -Xclang -ast-dump=json -I [directory like...\azure-sdk-for-c\sdk\inc] -I [any dependencies like "...\Visual Studio 2022\VC\Tools\MSVC\14.33.31629\include" > [out name like az_core.ast]
```

### C parsing requires a zipped file
Example powershell command:
```
Compress-Archive az_core.ast -DestinationPath az_core.zip
```

Example Debian-based Linux commands:
``` 
sudo apt install zip
zip az_core.zip az_core.ast
```
