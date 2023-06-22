using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;           
using Newtonsoft.Json;

public class Message {
    public string message { get; set; }
}

public class PythonScriptRunner : MonoBehaviour
{
    //private string pythonPath = "python3"; // Ruta al ejecutable de Python en tu sistema
    //private string scriptPath = "Assets/ThakiraServer/main.py"; // Ruta al archivo de Python que deseas ejecutar /Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private string saludo;
    private string ask;
    private string response;
    private string response1;
    private string response2;
    private string message;
    

    IEnumerator res;

    private string[] palabras;


    private void Start()
    {
        
        string[] devices = Microphone.devices;
        if (devices.Length == 0)
        {
            UnityEngine.Debug.Log("No se encontraron dispositivos de entrada de audio.");
        }
        else
        {
            UnityEngine.Debug.Log("Dispositivos de entrada de audio disponibles:");

            for (int i = 0; i < devices.Length; i++)
            {
                UnityEngine.Debug.Log("Dispositivo " + i + ": " + devices[i]);
            }
        }
        StartCoroutine(startSpeak());
        
    }

    private IEnumerator startSpeak() {
        UnityEngine.Debug.Log("Entro en main");
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(audioClip);
        UnityEngine.Debug.Log(assetPath);
        audioClip = Resources.Load<AudioClip>(assetPath);
        if ( audioClip == null ) {
            UnityEngine.Debug.Log("No se pudo cargar el archivo de audio.");
        } else {
            UnityEngine.Debug.Log("Archivo de audio cargado.");
        }
        //animator = GetComponent<Animator>();
        // conversacion
        UnityEngine.Debug.Log("startSpeak");

        yield return GetAudio("hola melissa", (data) => {
            UnityEngine.Debug.Log(data + " respuesta audio 1");
        } );
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();

        while (audioSource.isPlaying) {
            yield return null;
        }


        yield return GetAudio("hola tizon", (response) => {
            UnityEngine.Debug.Log(response + " respuesta audio 2");
        } );

        audioSource.clip = audioClip;
        audioSource.Play();

        while (audioSource.isPlaying) {
            yield return null;
        }

        UnityEngine.Debug.Log("5 seconds");
        yield return new WaitForSeconds(5f);

        yield return Think("dame 3 palabras aleatorias", (response) => {
            UnityEngine.Debug.Log(response + " -> respuesta think");
        } );
/*
        yield return Step1();
        yield return Step2();
        yield return Step3();
        yield return Step4();
*/
    }
/*
    private IEnumerator Step1 () {
    saludo = "Hola, soy Thakira. El día de hoy te haré una pequeña prueba de memoria. Empecemos!";
    yield return GetAudio(saludo, (data) => {
    UnityEngine.Debug.Log(data + " respuesta saludo");
    } );
    // play audio
    ask = "Dame 3 palabras para memorizar que no sean muy largas y separas por comas";
    yield return Think(ask, (response) => {
            UnityEngine.Debug.Log(response + " -> respuesta think");
    } );
    response1 = "Memorízate estas 3 palabras: " + str(palabras[0]);
    yield return GetAudio(response1, (data) => {
    UnityEngine.Debug.Log(data + " respuesta 3RP");
    } );
      // play audio
    yield return new WaitForSeconds(5f);
    response2 = "Ahora, elige un tema que te guste o interese y del cuál desees conocer su historia. Al final te plantearé una pregunta de opción múltiple. Por favor, indica claramente la letra correspondiente a tu respuesta.";
    yield return GetAudio(response1, (data) => {
    UnityEngine.Debug.Log(data + " respuesta eleccion tema");
    } );
      // play audio
    }

    private IEnumerator Step2 () {
        // Escuchar la respuesta del usuario
        message = GetText();
        ask = "Hablame sobre la historia de " + message + " Y hazme una unica pregunta de opción múltiple de comprensión de lectura difícil sobre lo que me vas a decir, sin decirme la respuesta enseguida.";
        yield return Think(ask, (response) => {
            UnityEngine.Debug.Log(response + " -> respuesta think");
        } );
        yield return GetAudio(response, (data) => {
        UnityEngine.Debug.Log(data + " respuesta pregunta");
        } );
        
    }

    private IEnumerator Step3 () {
        // Escuchar la respuesta del usuario 
        message = GetText();
        string[] messageList = message.Split();
        if (messageList.Contains("a") || messageList.Contains("A") || messageList.Contains("b") || messageList.Contains("B") ||
            messageList.Contains("c") || messageList.Contains("C") || messageList.Contains("d") || messageList.Contains("D")){
            yield return Think(message, (response) => {
            UnityEngine.Debug.Log(response + " -> respuesta think");
            } );
        }
        else {
            message2 = "Por favor, indica claramente la letra correspondiente a tu respuesta.";
            yield return GetAudio(message2, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
            // play audio
            // Escuchar la respuesta del usuario 
            message3 = GetText();
            yield return Think(message3, (response) => {
            UnityEngine.Debug.Log(response + " -> respuesta think");
            } );
        }
        yield return GetAudio(response, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
        // play audio
        ask = "Responde con esta frase 'A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras' y luego inserta todas las siguientes palabras " +str(palabras[0])+ " en una lista de 10 palabras cualquiera (diferentes a las insertadas), en desorden,separadas por comas, en esa lista obligatoriamente deben estar las palabras que te pedi que insertaras. Luego di 'Dime las 3 palabras que recuerdas'. No digas nada más. En ningun caso me digas las 3 palabras que se insertaron, exceptuando en la lista. Si respondo bien, diciendo las 3 palabras , felicitame, si no, dime que es incorrecto y cuales eran las 3 palabras. Mi respuesta obligatoriamente debe ser " +str(palabras[0])+", no puede ser una variación de esas palabras. Sólo hay un intento entonces no me ofrezcas más.";
        yield return Think(ask, (response2) => {
            UnityEngine.Debug.Log(response2 + " -> respuesta think");
            } );
        yield return GetAudio(response2, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
        // play audio
        
    }

    private IEnumerator Step4 () {
        // Escuchar la respuesta del usuario
        message = GetText();
        string[] messageList = message.Split();
        string[] palabrasList = str(palabras[0]).Split();

        int cont = 0;
        foreach (string i in messageList)
        {
            if (palabrasList.Contains(i))
            {
            cont++;
            }
        }

        if (cont == 3)
        {
            response = "¡Felicidades! Has recordado correctamente las tres palabras que te pedí memorizar:" + str(palabras[0]) + " Excelente trabajo.";
            yield return GetAudio(response, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
            // play audio
        }
        else {

            response = "Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: " + str(palabras[0]);
            yield return GetAudio(response, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
            // play audio
        }

        chao = "Gracias por participar en esta prueba. Nos vemos pronto o o";
        yield return GetAudio(chao, (data) => {
                UnityEngine.Debug.Log(data + " respuesta pregunta");
            } );
        // play audio

        
    }
*/
    

    IEnumerator GetText(string message)
        {
        
        // Create a dictionary with the data you want to send in the JSON body
        Dictionary<string, string> requestData = new Dictionary<string, string>();
        requestData.Add("key1", "value1");
        requestData.Add("key2", "value2");

        // Serialize the dictionary to JSON format
        string jsonBody = JsonUtility.ToJson(requestData);

        // Create the POST request with the JSON body
        using (UnityWebRequest request = new UnityWebRequest("", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            // Check if there was an error in the response
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error in request: " + request.error);
            }
            else
            {
                // Get the server response
                string response = request.downloadHandler.text;
                Debug.Log("Server response: " + response);
            }
        }
    }


    IEnumerator Think(string message, System.Action<string> callback)
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
            callback(response);
        }

        // return null;
    }

IEnumerator GetAudio(string message, System.Action<string> callback)
    {
        Message msg = new Message();
        msg.message = message;
        string jsonBody= JsonConvert.SerializeObject(msg);
         UnityEngine.Debug.Log(jsonBody);
        // Crear un diccionario con los datos que deseas enviar en el cuerpo JSON
        UnityEngine.Debug.Log("Entro en GetAudio");
        // Crear la solicitud POST con el cuerpo JSON
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/text_to_audio", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        UnityEngine.Debug.Log("Enviar");
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
            callback(response);
        }

        // return null;
    }

}

