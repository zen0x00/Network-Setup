using System.Diagnostics;
using System.Text;

public static class LinuxProcessBridge
{
    public static string Run(string filePath, string arguments = "")
    {
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = filePath;
        info.Arguments = arguments;
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
        info.UseShellExecute = false;
        info.CreateNoWindow = true;

        Process process = new Process();
        process.StartInfo = info;
        process.Start();

        StringBuilder output = new StringBuilder();
        output.Append(process.StandardOutput.ReadToEnd());
        output.Append(process.StandardError.ReadToEnd());

        process.WaitForExit();
        return output.ToString();
    }
}
