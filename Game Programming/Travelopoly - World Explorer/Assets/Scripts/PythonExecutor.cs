using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonExecutor : MonoBehaviour
{
    public string pythonExecutableName = "your_script.exe"; // Name of the Python executable (compiled .exe file)

    void Start()
    {
        RunPythonApp();
    }

    public void RunPythonApp()
    {
        try
        {
            // Get the directory where the Unity executable is running
            string unityBuildFolder = Path.GetDirectoryName(Application.dataPath);  // Get Unity build folder

            // Combine the Unity build folder path with the relative path to the Python executable
            string pythonExecutablePath = Path.Combine(unityBuildFolder, pythonExecutableName);

            // Check if the Python executable exists in the build folder
            if (File.Exists(pythonExecutablePath))
            {
                // Start the Python application (Flask server)
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = pythonExecutablePath;  // Path to Python executable
                startInfo.UseShellExecute = true;  // Use shell to execute the Python script

                Process process = Process.Start(startInfo);
                UnityEngine.Debug.Log("Python server started at http://127.0.0.1:5000/submit_answer");

                // Optional: Wait for the Python process to finish (if needed)
                // process.WaitForExit();
            }
            else
            {
                UnityEngine.Debug.LogError("Python executable not found in the Unity build folder!");
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"Error starting Python app: {ex.Message}");
        }
    }
}
