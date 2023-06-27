using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;           
using Newtonsoft.Json;
using System.IO;

public class Message {
    public string message { get; set; }
}

[System.Serializable]
public class ResponseData
{
    public ResponseText response;
}

[System.Serializable]
public class ResponseText
{
    public string text;
}

public class PythonScriptRunner : MonoBehaviour
{
    //private string pythonPath = "python3"; // Ruta al ejecutable de Python en tu sistema
    //private string scriptPath = "Assets/ThakiraServer/main.py"; // Ruta al archivo de Python que deseas ejecutar /Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer
    public Animator animator;
    public AudioSource audioSource;
    

    IEnumerator res;

    private string[] palabras;
    private string palabrasCrude;


    private void Start()
    {
        StartCoroutine(startSpeak());
        
    }

    private IEnumerator startSpeak() {
        audioSource = GetComponent<AudioSource>();
        UnityEngine.Debug.Log("Entro en main");
        //animator = GetComponent<Animator>();
        // conversacion
        UnityEngine.Debug.Log("startSpeak");

        yield return Step1();
        IEnumerator step2I = Step2();
        yield return StartCoroutine(step2I);
        bool step2 = (bool)step2I.Current;
        if (step2) {
            yield return Step3();
            yield return Step4();
        }
    }

    private void PlayAudio(AudioClip clip) {
        UnityEngine.Debug.Log("playing...");
        audioSource.clip = clip;
        audioSource.Play();
    }

    private IEnumerator WaitPlay(){
        while (audioSource.isPlaying) {
            yield return null;
        }
    }

    private IEnumerator Step1 () {
        string saludo = "Hola, soy Thakira. El día de hoy te haré una pequeña prueba de memoria. Empecemos!";
        yield return GetAudio(saludo, PlayAudio);
        yield return WaitPlay();
        string ask = "Dame 3 palabras para memorizar que no sean muy largas y separas por comas";
        IEnumerator coroutine = Think(ask);
        yield return StartCoroutine(coroutine);
        string thinker = (string)coroutine.Current;
        ask = "Memorízate estas 3 palabras: " + thinker;
        yield return GetAudio(ask, PlayAudio);
        yield return WaitPlay();
        ask = "Ahora, elige un tema que te guste o interese y del cuál desees conocer su historia.";
        yield return GetAudio(ask, PlayAudio);
        yield return WaitPlay();
        palabras = thinker.Split(',');
        palabrasCrude = thinker;
        UnityEngine.Debug.Log("palabras" + thinker);
    }

    private IEnumerator Step2 () {
        IEnumerator messageCo = RecordVoiceAndGetText(10f);
        yield return StartCoroutine(messageCo);
        string path = (string)messageCo.Current;
        IEnumerator voiceTextI = GetText(path);
        yield return StartCoroutine(voiceTextI);
        string voiceText = (string)voiceTextI.Current;
        if (voiceText == "No te entendi lo siento") {
            yield return GetAudio("No te entendi lo siento, nos vemos en una próxima.", PlayAudio);
            yield return WaitPlay();
            yield return false;
        } else {
            string ask = "Ahora te contare sobre el tema y te plantearé una pregunta de opción múltiple. Por favor, indica claramente la letra correspondiente a tu respuesta. Dame un momento.";
            yield return GetAudio(ask, PlayAudio);
            yield return WaitPlay();
            ask = "Hablame sobre la historia de " + voiceText + " Y hazme una unica pregunta de opción múltiple de comprensión de lectura difícil sobre lo que me vas a decir, sin decirme la respuesta enseguida.";
            UnityEngine.Debug.Log(ask);
            IEnumerator coroutine = Think(ask);
            yield return StartCoroutine(coroutine);
            string thinker = (string)coroutine.Current;
            yield return GetAudio(thinker, PlayAudio);
            yield return WaitPlay();
            ask = "responde ya";
            yield return GetAudio(ask, PlayAudio);
            yield return WaitPlay();
            yield return true;
        }
    }

    private IEnumerator Step3 () {
        // Escuchar la respuesta del usuario 
        IEnumerator messageCo = RecordVoiceAndGetText(5f);
        yield return StartCoroutine(messageCo);
        string path = (string)messageCo.Current;
        IEnumerator voiceTextI = GetText(path);
        yield return StartCoroutine(voiceTextI);
        string voiceText = (string)voiceTextI.Current;
        string thinker = "";
        string[] messageList = voiceText.Split();
        Dictionary<string, bool> options = new Dictionary<string, bool>();
        // Agregar elementos al diccionario
        options.Add("A", true);
        options.Add("B", true);
        options.Add("C", true);
        options.Add("D", true);
        bool find = false;
        foreach(string word in messageList) {
            UnityEngine.Debug.Log("messageList: " + word);
            if (options.ContainsKey(word.ToUpper())) {
               UnityEngine.Debug.Log("find: " + word);
               find = true;
               break;
            }
        }

        IEnumerator coroutine = Think("La respuesta es " + voiceText);
        if (find){
            yield return StartCoroutine(coroutine);
            thinker = (string)coroutine.Current;
        } else {
            thinker = "No te entendi lo siento,o te preocupes, pasemos a la siguiente pregunta.";
        }

        yield return GetAudio(thinker, PlayAudio);
        yield return WaitPlay();
        string ask = "Responde con esta frase 'A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras' y "; 
        ask += "luego inserta todas las siguientes palabras " + palabrasCrude;
        ask += " en una lista de 10 palabras cualquiera (diferentes a las insertadas), en desorden,separadas por comas, en esa lista obligatoriamente deben estar las palabras que te pedi que insertaras. ";
        ask += "Luego di 'Dime las 3 palabras que recuerdas'. No digas nada más. En ningun caso me digas las 3 palabras que se insertaron, exceptuando en la lista.";
        ask += "Si respondo bien, diciendo las 3 palabras , felicitame, si no, dime que es incorrecto y cuales eran las 3 palabras. ";
        ask += "Mi respuesta obligatoriamente debe ser " + palabrasCrude + ", no puede ser una variación de esas palabras. Sólo hay un intento entonces no me ofrezcas más.";
        coroutine = Think(ask);
        yield return StartCoroutine(coroutine);
        thinker = (string)coroutine.Current;
        yield return GetAudio(thinker, PlayAudio);
        yield return WaitPlay();
    }

    private IEnumerator Step4 () {
        // Escuchar la respuesta del usuario
        IEnumerator messageCo = RecordVoiceAndGetText(5f);
        yield return StartCoroutine(messageCo);
        string path = (string)messageCo.Current;
        IEnumerator voiceTextI = GetText(path);
        yield return StartCoroutine(voiceTextI);
        string voiceText = (string)voiceTextI.Current;

        string[] messageList = voiceText.Split();
        Dictionary<string, bool> options = new Dictionary<string, bool>();

        foreach(string palabra in palabras) {
            options.Add(palabra.ToUpper().Replace(".","").Replace(",","").Replace(" ",""), true);
             UnityEngine.Debug.Log("palabras memoria" + palabra.ToUpper().Replace(".","").Replace(",","").Replace(" ",""));
        }

        int cont = 0;
        foreach (string i in messageList)
        {   UnityEngine.Debug.Log("palabra usuario: " + i.ToUpper().Replace(".","").Replace(",","").Replace(" ",""));
            if (options.ContainsKey(i.ToUpper().Replace(".","").Replace(",","").Replace(" ","")))
            {
                UnityEngine.Debug.Log("palabra usuario encontrada: " + i);
                cont++;
            }
        }

        if (cont == 3)
        {
            string response = "¡Felicidades! Has recordado correctamente las tres palabras que te pedí memorizar:" + palabrasCrude + " Excelente trabajo.";
            yield return GetAudio(response, PlayAudio);
            yield return WaitPlay();
        }
        else {

            string response = "Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: " + palabrasCrude;
            yield return GetAudio(response, PlayAudio);
            yield return WaitPlay();
        }

        string chao = "Gracias por participar en esta prueba. Nos vemos pronto o o";
        yield return GetAudio(chao, PlayAudio);
        yield return WaitPlay();
    }
    
private IEnumerator RecordVoiceAndGetText(float waitTime)
    {
        string microphoneDevice = Microphone.devices[0];
        // Iniciar la grabación del audio del micrófono
        AudioClip recordedClip = Microphone.Start(microphoneDevice, true, 10, 44100);
        yield return new WaitForSeconds(waitTime);
         // Detener la grabación del audio del micrófono
        Microphone.End(null);
        string path = "./audioVoiceUser";
        SavWav.Save(path, recordedClip);

        PlayAudio(recordedClip);
        yield return WaitPlay();

        // Convertir el AudioClip en un array de bytes
        yield return path + ".wav";
    }
IEnumerator GetText(string audioPath)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", File.ReadAllBytes(audioPath), "audio.wav", "audio/wav");
         // Crear una instancia de UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/audio_to_text", form);
        // Establecer el formulario como el cuerpo de la solicitud
        request.downloadHandler = new DownloadHandlerBuffer();

        // Enviar la solicitud al servidor
        yield return request.SendWebRequest();

        // Verificar si hubo algún error en la respuesta
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en la solicitud: " + request.error);
            yield return "No te entendi lo siento";
        }
        else
        {
            // Obtener la respuesta del servidor
             string responseJson = request.downloadHandler.text;

            // Deserializar la respuesta JSON en un objeto C#
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);

            // Acceder a los datos específicos en la respuesta
            string text = responseData.response.text;

            Debug.Log("Respuesta del servidor: " + text);
            yield return text;
        }
}


IEnumerator Think(string message)
{
    Message msg = new Message();
    msg.message = message;
    string jsonBody= JsonConvert.SerializeObject(msg);
        UnityEngine.Debug.Log(jsonBody);
    // Crear un diccionario con los datos que deseas enviar en el cuerpo JSON
    UnityEngine.Debug.Log("Entro en Think");
    // Crear la solicitud POST con el cuerpo JSON
    UnityWebRequest request = new UnityWebRequest("http://localhost:8080/listen", "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    UnityEngine.Debug.Log("Enviar Think" );
    // Enviar la solicitud y esperar la respuesta
    yield return request.SendWebRequest();

    // Verificar si hubo algún error en la respuesta
    if (request.result != UnityWebRequest.Result.Success)
    {
        UnityEngine.Debug.Log("Error en la solicitud: " + request.error);
    }
    else
    {
        // Obtener la respuesta del servidor
        string response = request.downloadHandler.text;
        UnityEngine.Debug.Log("Respuesta del servidor: " + response);
        yield return response;
    }
}


IEnumerator GetAudio(string message, System.Action<AudioClip> callback)
    {
        Message msg = new Message();
        msg.message = message;
        string jsonBody= JsonConvert.SerializeObject(msg);
         UnityEngine.Debug.Log(jsonBody);
        // Crear un diccionario con los datos que deseas enviar en el cuerpo JSON
        UnityEngine.Debug.Log("Entro en GetAudio");
        // Crear la solicitud POST con el cuerpo JSON
        UnityWebRequest www = new UnityWebRequest("http://localhost:8080/text_to_audio", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        DownloadHandlerAudioClip dHA = new DownloadHandlerAudioClip(string.Empty, AudioType.MPEG);
        dHA.streamAudio = true;
        www.downloadHandler = dHA;
        www.SetRequestHeader("Content-Type", "application/json");
        UnityEngine.Debug.Log("Enviar");
        www.SendWebRequest();
        // Enviar la solicitud y esperar la respuesta
        while (www.downloadProgress < 1) {
            Debug.Log("Downloading...");
            yield return new WaitForSeconds(0.9f);
        }
        if (www.responseCode != 200 || www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log("error");
        } else {
            Debug.Log("Start audio play");
            callback(DownloadHandlerAudioClip.GetContent(www));
        }
    }

}

