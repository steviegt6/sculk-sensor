using System;
using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JetBrains.Annotations;
using Sculk.Sensor;
using Sculk.Sensor.Config;

namespace Sculk.Catalyst;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
[Command(Description = "Decompiles the Sculk directory or file")]
public sealed class DecompileCommand : ICommand {
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

        var projectType = ResolveProjectType();
        Output = ResolveOutput(projectType);

        switch (projectType) {
            case ProjectType.File:
                DecompileFile(c);
                break;

            case ProjectType.Directory:
                DecompileDirectory(c);
                break;

            case ProjectType.Unknown:
                c.Output.WriteLine($"Input '{Input}' does not exist.");
                break;

            default:
                throw new InvalidOperationException();
        }

        return default;
    }

    private ProjectType ResolveProjectType() {
        if (File.Exists(Input))
            return ProjectType.File;

        if (Directory.Exists(Input))
            return ProjectType.Directory;

        return ProjectType.Unknown;
    }

    private string? ResolveOutput(ProjectType projectType) {
        if (Output is not null)
            return Output;

        Output = Path.Combine(
            Path.GetDirectoryName(Input)!,
            Path.GetFileNameWithoutExtension(Input)
        );

        return projectType switch {
            ProjectType.File => Output + ".sculk",
            ProjectType.Directory => Output,
            ProjectType.Unknown => null,
            _ => throw new InvalidOperationException(),
        };
    }

    private void DecompileFile(IConsole c) {
        if (!File.Exists(Input))
            throw new FileNotFoundException(Input);

        c.Output.WriteLine($"Treating output '{Output}' as a file.");
        c.Output.WriteLine($"Decompiling file '{Input}'...");

        var settings = GetSettings(ProjectType.File);
        Decompile(c, settings);
    }

    private void DecompileDirectory(IConsole c) {
        if (!Directory.Exists(Input))
            throw new DirectoryNotFoundException(Input);

        c.Output.WriteLine($"Treating output '{Output}' as a directory.");
        c.Output.WriteLine($"Decompiling directory '{Input}'...");

        var settings = GetSettings(ProjectType.Directory);
        Decompile(c, settings);
    }

    private static void Decompile(IConsole c, DecompilerSettings settings) {
        void logProgress(DecompilationProgress progress) {
            var msg = $"({progress.CurrentProgress}/{progress.TotalProgress})";

            if (progress.Name is not null)
                msg += $" {progress.Name}";

            if (progress.Description is not null) {
                if (progress.Name is not null)
                    msg += " -";

                msg += $" {progress.Description}";
            }

            c.Output.WriteLine(msg);
        }

        settings.OnProgress += logProgress;
        var decompiler = new SculkDecompiler(settings);
    }

    private DecompilerSettings GetSettings(ProjectType projectType) {
        return new DecompilerSettings {
            // TODO: Configurable formatting options.
            Formatting = new FormattingSettings(),
            ProjectType = projectType,
            Input = Input,
        };
    }
}
