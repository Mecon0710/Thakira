using UnityEngine;
using System.IO;
using System.Diagnostics;

public class AudioUser : MonoBehaviour
{
    // Variables de configuración
    public int recordingDuration = 45; // Duración de la grabación en segundos

    // Variables internas
    private string micDeviceName; // Nombre del dispositivo de audio
    private string pcmPath = "./audios/input/voice.pcm"; // Ruta del archivo PCM temporal
    private string mp3Path = "./audios/input/voice.mp3"; // Ruta de la carpeta para guardar el archivo MP3

    // Método para iniciar la grabación
    public void StartRecording()
    {
        // Obtener el nombre del dispositivo de audio predeterminado
        micDeviceName = Microphone.devices[0];

        // Crear un AudioClip para grabar audio
        AudioClip audioClip = Microphone.Start(micDeviceName, false, recordingDuration, AudioSettings.outputSampleRate);

        // Esperar hasta que la grabación haya comenzado
        while (Microphone.GetPosition(micDeviceName) <= 0) { }

        // Esperar hasta que se complete la grabación
        while (Microphone.IsRecording(micDeviceName)) { }

        // Obtener los datos de audio en formato PCM
        float[] pcmData = new float[audioClip.samples];
        audioClip.GetData(pcmData, 0);

        // Guardar los datos PCM en un archivo temporal
        SavePCMData(pcmData, pcmPath);

        // Convertir el archivo PCM a MP3
        ConvertPCMToMP3();

        // Eliminar el archivo PCM temporal
        File.Delete(pcmPath);

         UnityEngine.Debug.Log("Grabación completada. Archivo MP3 guardado en: " + mp3Path);
    }

    // Método para guardar los datos PCM en un archivo
    private void SavePCMData(float[] pcmData, string filePath)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            foreach (float sample in pcmData)
            {
                // Convertir el valor de la muestra a bytes y escribirlo en el archivo
                short sampleValue = (short)(sample * short.MaxValue);
                writer.Write(sampleValue);
            }
        }
    }


    private void ConvertPCMToMP3()
    {
        string lameExePath = "/usr/local/bin/lame"; // Ruta al ejecutable de LAME

        // Comando para ejecutar LAME y realizar la conversión
        string command = $"--silent --preset standard \"{pcmPath}\" \"{mp3Path}\"";

        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = lameExePath;
        processInfo.Arguments = command;
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;

        Process process = new Process();
        process.StartInfo = processInfo;
        process.Start();
        process.WaitForExit();
    }


    
}
