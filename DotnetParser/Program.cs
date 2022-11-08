using APIView.DotnetParser;
using System;
using System.IO;

try
{
    LanguageService buildService = null;
    string InFilePath = args[0];
    string OutFilePath = args[1];

    switch (Path.GetExtension(InFilePath))
    {
        case ".zip":
            buildService = new CLanguageService();
            break;
        case ".cppast":
            buildService = new CppLanguageService();
            break;
        case ".dll":
        case ".nupkg":
            buildService = new CSharpLanguageService();
            break;
        default:
            break;
    }

    var memS = new MemoryStream();
    CodeFile codeFile = null;
    using (var In = File.OpenRead(InFilePath))
    {
        In.CopyTo(memS);
        memS.Seek(0, SeekOrigin.Begin);
    }
    codeFile = await buildService.GetCodeFileAsync(Path.GetFileName(InFilePath), memS, false);

    using (var OutFileStream = new FileStream(OutFilePath, FileMode.CreateNew, FileAccess.Write))
    {
        await codeFile.SerializeAsync(OutFileStream);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
