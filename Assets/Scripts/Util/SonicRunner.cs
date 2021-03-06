using System.Diagnostics;
using UnityEngine;
using System.Text;
using System;

public class SonicRunner
{
    public (int, string) sonicGetTempi(string audioPath)
    {
        string argsToSonic = "-d vamp:qm-vamp-plugins:qm-tempotracker:tempo -q -w jams --jams-stdout \"" + audioPath + "\"";

        UnityEngine.Debug.Log(argsToSonic);

        return runSonic(argsToSonic);
    }

    public (int, string) sonicGetModes(string audioPath)
    {
        string argsToSonic = "-d vamp:qm-vamp-plugins:qm-keydetector:mode -q -w jams --jams-stdout \"" + audioPath + "\"";

        UnityEngine.Debug.Log(argsToSonic);

        return runSonic(argsToSonic);
    }


    private (int, string) runSonic(string argsToSonic)
    {
        int exitCode = 0;

        // Set vamp plugins path
        if (Environment.GetEnvironmentVariable("VAMP_PATH") == null)
        {
            Environment.SetEnvironmentVariable("VAMP_PATH", Application.streamingAssetsPath + "/TCM/Vamp Plugins/");
        }

        string sonicExecutablePath = Application.streamingAssetsPath + "/TCM/sonic-annotator-1.6-win64/sonic-annotator.exe";

        var outputStrBuilder = new StringBuilder();
        var errorStrBuilder = new StringBuilder();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = sonicExecutablePath,
            Arguments = argsToSonic,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        Process sonicProcess = Process.Start(processStartInfo);

        // hookup the eventhandlers to capture the data that is received
        sonicProcess.OutputDataReceived += (sender, args) => outputStrBuilder.AppendLine(args.Data);
        sonicProcess.ErrorDataReceived += (sender, args) => errorStrBuilder.AppendLine(args.Data);

        // start our event pumps
        sonicProcess.BeginOutputReadLine();
        sonicProcess.BeginErrorReadLine();

        sonicProcess.WaitForExit();

        exitCode = sonicProcess.ExitCode;

        UnityEngine.Debug.Log(outputStrBuilder.ToString());
        UnityEngine.Debug.Log(errorStrBuilder.ToString());

        return (exitCode, outputStrBuilder.ToString());
    }
}

public class CopyOfSonicRunner
{
    public (int, string) sonicGetTempi(string audioPath)
    {
        string argsToSonic = "-d vamp:qm-vamp-plugins:qm-tempotracker:tempo -q -w jams --jams-stdout \"" + audioPath + "\"";

        UnityEngine.Debug.Log(argsToSonic);

        return runSonic(argsToSonic);
    }

    public (int, string) sonicGetModes(string audioPath)
    {
        string argsToSonic = "-d vamp:qm-vamp-plugins:qm-keydetector:mode -q -w jams --jams-stdout \"" + audioPath + "\"";

        UnityEngine.Debug.Log(argsToSonic);

        return runSonic(argsToSonic);
    }


    private (int, string) runSonic(string argsToSonic)
    {
        int exitCode = 0;

        // Set vamp plugins path
        if (Environment.GetEnvironmentVariable("VAMP_PATH") == null)
        {
            Environment.SetEnvironmentVariable("VAMP_PATH", Application.streamingAssetsPath + "/TCM/Vamp Plugins/");
        }

        string sonicExecutablePath = Application.streamingAssetsPath + "/TCM/sonic-annotator-1.6-win64/sonic-annotator.exe";

        var outputStrBuilder = new StringBuilder();
        var errorStrBuilder = new StringBuilder();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = sonicExecutablePath,
            Arguments = argsToSonic,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        Process sonicProcess = Process.Start(processStartInfo);

        // hookup the eventhandlers to capture the data that is received
        sonicProcess.OutputDataReceived += (sender, args) => outputStrBuilder.AppendLine(args.Data);
        sonicProcess.ErrorDataReceived += (sender, args) => errorStrBuilder.AppendLine(args.Data);

        // start our event pumps
        sonicProcess.BeginOutputReadLine();
        sonicProcess.BeginErrorReadLine();

        sonicProcess.WaitForExit();

        exitCode = sonicProcess.ExitCode;

        UnityEngine.Debug.Log(outputStrBuilder.ToString());
        UnityEngine.Debug.Log(errorStrBuilder.ToString());

        return (exitCode, outputStrBuilder.ToString());
    }
}
