using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;           


public class PythonScriptRunner : MonoBehaviour
{
    //private string pythonPath = "python3"; // Ruta al ejecutable de Python en tu sistema
    //private string scriptPath = "Assets/ThakiraServer/main.py"; // Ruta al archivo de Python que deseas ejecutar /Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer
    public Animator animator;
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
        StartCoroutine(startSpeak());
    }

    private string startSpeak() {
        UnityEngine.Debug.Log("Entro en main");
        //animator = GetComponent<Animator>();
        // conversacion
        UnityEngine.Debug.Log("GetAudio");
        res = GetAudio("hola melissa");
        //UnityEngine.Debug.Log(res);

        /*
        Step1();
        Step2();
        Step3();
        Step4();
        }
        */
        return "";
    }
/*
    private void Step1 () {
    saludo = "Hola, soy Thakira. El día de hoy te haré una pequeña prueba de memoria. Empecemos!";
    audio = GetAudio(saludo);
    // play audio
    ask = "Dame 3 palabras para memorizar que no sean muy largas y separas por comas";
    palabras = Think(ask);
    response1 = "Memorízate estas 3 palabras: " + str(palabras[0]);
    audio = GetAudio(response1);
      // play audio
    WaitForSeconds(5f);
    response2 = "Ahora, elige un tema que te guste o interese y del cuál desees conocer su historia. Al final te plantearé una pregunta de opción múltiple. Por favor, indica claramente la letra correspondiente a tu respuesta.";
    audio = GetAudio(response1);
      // play audio
    return "Step 1 finalizado";
    }

    private void Step2 () {
        // Escuchar la respuesta del usuario
        message = GetText();
        ask = "Hablame sobre la historia de " + message + " Y hazme una unica pregunta de opción múltiple de comprensión de lectura difícil sobre lo que me vas a decir, sin decirme la respuesta enseguida.";
        response = Think(ask);
        audio = GetAudio(response);
        return "Step 2 finalizado";
    }

    private void Step3 () {
        // Escuchar la respuesta del usuario 
        message = GetText();
        string[] messageList = message.Split();
        if (messageList.Contains("a") || messageList.Contains("A") || messageList.Contains("b") || messageList.Contains("B") ||
            messageList.Contains("c") || messageList.Contains("C") || messageList.Contains("d") || messageList.Contains("D")){
                response = Think(message);
        }
        else {
            message2 = "Por favor, indica claramente la letra correspondiente a tu respuesta.";
            audio = GetAudio(message2);
            // play audio
            // Escuchar la respuesta del usuario 
            message3 = GetText();
            response = Think(message3);
        }
        audio = GetAudio(response);
        // play audio
        ask = "Responde con esta frase 'A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras' y luego inserta todas las siguientes palabras " +str(palabras[0])+ " en una lista de 10 palabras cualquiera (diferentes a las insertadas), en desorden,separadas por comas, en esa lista obligatoriamente deben estar las palabras que te pedi que insertaras. Luego di 'Dime las 3 palabras que recuerdas'. No digas nada más. En ningun caso me digas las 3 palabras que se insertaron, exceptuando en la lista. Si respondo bien, diciendo las 3 palabras , felicitame, si no, dime que es incorrecto y cuales eran las 3 palabras. Mi respuesta obligatoriamente debe ser " +str(palabras[0])+", no puede ser una variación de esas palabras. Sólo hay un intento entonces no me ofrezcas más.";
        response2 = Think(ask);
        audio2 = GetAudio(response2);
        // play audio
        return "Step 3 finalizado";
    }

    private void Step4 () {
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
            audio = GetAudio(response);
            // play audio
        }
        else {

            response = "Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: " + str(palabras[0]);
            audio = GetAudio(response);
            // play audio
        }

        chao = "Gracias por participar en esta prueba. Nos vemos pronto o o";
        audio = GetAudio(chao);
        // play audio

        return "Step 4 finalizado";
    }

    private void RunPythonScript()
    {
        UnityEngine.Debug.Log("Entro en RunPythonScript");

        // Crear un motor de ejecución de Python
        var engine = Python.CreateEngine();

        // Cargar el script en Python
        UnityEngine.Debug.Log("Cargo el script en Python");
        var scope = engine.CreateScope();
        var source = engine.CreateScriptSourceFromFile(scriptPath);
        source.Execute(scope);
        
        // Obtener la referencia a las funciones en Python
        UnityEngine.Debug.Log("Obtuvo la referencia a las funciones en Python");
        var step1 = scope.GetVariable<Func<object>>("step1");
        var step2 = scope.GetVariable<Func<object>>("step2");
        var step3 = scope.GetVariable<Func<object>>("step3");
        var step4 = scope.GetVariable<Func<object>>("step4");

        // Llamar a la funciones en Python en orden de la prueba
        UnityEngine.Debug.Log("Inicio de la prueba");
        step1();
        StartRecording();
        step2();
        StartRecording();
        step3();
        StartRecording();
        step4();

        UnityEngine.Debug.Log("Prueba completada");
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


    IEnumerator Think(string message)
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

IEnumerator GetAudio(string message)
    {
        // Crear un diccionario con los datos que deseas enviar en el cuerpo JSON
        UnityEngine.Debug.Log("Entro en GetAudio");
        Dictionary<string, string> requestData = new Dictionary<string, string>();
        requestData.Add("message", message);

        // Serializar el diccionario a formato JSON
        string jsonBody = JsonUtility.ToJson(requestData);
        UnityEngine.Debug.Log(jsonBody );
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
        }

        // return null;
    }

}

