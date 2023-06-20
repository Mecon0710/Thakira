using UnityEngine;
using System.IO;
using System.Diagnostics;

public class AudioRecorder : MonoBehaviour
{
    // Variables de configuración
    public int recordingDuration = 45; // Duración de la grabación en segundos

    // Variables internas
    private string micDeviceName; // Nombre del dispositivo de audio
    private string pcmFilePath; // Ruta del archivo PCM temporal
    private string mp3Path = "audios/input"; // Ruta de la carpeta para guardar el archivo MP3

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
        pcmFilePath = Path.Combine(Application.persistentDataPath, "voice.pcm");
        SavePCMData(pcmData, pcmFilePath);

        // Convertir el archivo PCM a MP3
        string mp3FilePath = Path.Combine(mp3Path, "voice.mp3");
        ConvertPCMToMP3(pcmFilePath, mp3FilePath);

        // Eliminar el archivo PCM temporal
        File.Delete(pcmFilePath);

        Debug.Log("Grabación completada. Archivo MP3 guardado en: " + mp3FilePath);
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

    // Método para convertir un archivo PCM a MP3 utilizando LAME
    private void ConvertPCMToMP3(string pcmFilePath, string mp3FilePath)
    {
        from pydub import AudioSegment

        input_file = "input.pcm"
        output_file = "output.mp3"

        # Cargar archivo PCM
        pcm_audio = AudioSegment.from_file(input_file, format="raw", channels=2, sample_width=2, frame_rate=44100)

        # Exportar audio en formato MP3
        pcm_audio.export(output_file, format="mp3")
    }
}
