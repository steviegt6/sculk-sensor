using System;
using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JetBrains.Annotations;

namespace Sculk.Catalyst;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
[Command(Description = "Decompiles the Sculk directory or file")]
public sealed class DecompileCommand : ICommand {
    public enum OutputType {
        File,
        Directory,
        Unknown,
    }

    public const int INPUT_ORDER = 0;
    public const string INPUT_NAME = "input";
    public const string INPUT_DESCRIPTION = "The input file or directory.";
    public const bool INPUT_REQUIRED = true;

    public const int OUTPUT_ORDER = 1;
    public const string OUTPUT_NAME = "output";
    public const string OUTPUT_DESCRIPTION = "The output file or directory";
    public const bool OUTPUT_REQUIRED = false;

    [CommandParameter(
        INPUT_ORDER,
        Name = INPUT_NAME,
        Description = INPUT_DESCRIPTION,
        IsRequired = INPUT_REQUIRED
    )]
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public string Input { get; set; } = null!;

    [CommandParameter(
        OUTPUT_ORDER,
        Name = OUTPUT_NAME,
        Description = OUTPUT_DESCRIPTION,
        IsRequired = OUTPUT_REQUIRED
    )]
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public string? Output { get; set; }

    ValueTask ICommand.ExecuteAsync(IConsole c) {
        //Input = Path.GetFullPath(Input);

        var outputType = ResolveOutputType();
        Output = ResolveOutput(outputType);

        switch (outputType) {
            case OutputType.File:
                DecompileFile(c);
                break;

            case OutputType.Directory:
                DecompileDirectory(c);
                break;

            case OutputType.Unknown:
                c.Output.WriteLine($"Input '{Input}' does not exist.");
                break;

            default:
                throw new InvalidOperationException();
        }

        return default;
    }

    private OutputType ResolveOutputType() {
        if (File.Exists(Input))
            return OutputType.File;

        if (Directory.Exists(Input))
            return OutputType.Directory;

        return OutputType.Unknown;
    }

    private string? ResolveOutput(OutputType outputType) {
        if (Output is not null)
            return Output;

        Output = Path.Combine(
            Path.GetDirectoryName(Input)!,
            Path.GetFileNameWithoutExtension(Input)
        );

        return outputType switch {
            OutputType.File => Output + ".sculk",
            OutputType.Directory => Output,
            OutputType.Unknown => null,
            _ => throw new InvalidOperationException(),
        };
    }

    private void DecompileFile(IConsole c) {
        if (!File.Exists(Input))
            throw new FileNotFoundException(Input);

        c.Output.WriteLine($"Treating output '{Output}' as a file.");
        c.Output.WriteLine($"Decompiling file '{Input}'...");
    }

    private void DecompileDirectory(IConsole c) {
        if (!Directory.Exists(Input))
            throw new DirectoryNotFoundException(Input);

        c.Output.WriteLine($"Treating output '{Output}' as a directory.");
        c.Output.WriteLine($"Decompiling directory '{Input}'...");
    }
}
