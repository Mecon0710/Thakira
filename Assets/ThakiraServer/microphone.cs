using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    AudioClip recordedClip;
    string microphoneDevice;
    bool isRecording = false;

    void Start()
    {
        // Obtener el nombre del dispositivo de micrófono disponible
        microphoneDevice = Microphone.devices[0];
    }

    public void StartRecording()
    {
        // Iniciar la grabación desde el micrófono
        recordedClip = Microphone.Start(microphoneDevice, true, 10, 44100);
        isRecording = true;
    }

    public void StopRecording()
    {
        // Detener la grabación
        Microphone.End(microphoneDevice);
        isRecording = false;

        // Guardar el audio grabado en un archivo
        string filePath = Application.persistentDataPath + "/audios/input/voice.mp3";
        SavWav.Save(filePath, recordedClip);
    }

    void Update()
    {
        if (isRecording)
        {
            // Realizar acciones mientras se está grabando, si es necesario
        }
    }
}
