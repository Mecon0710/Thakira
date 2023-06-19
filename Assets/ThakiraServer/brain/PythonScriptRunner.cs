using System.Diagnostics;
using UnityEngine;

public class PythonScriptRunner : MonoBehaviour
{
    private string pythonPath = "python3"; // Ruta al ejecutable de Python en tu sistema
    private string scriptPath = "/Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer/main.py"; // Ruta al archivo de Python que deseas ejecutar /Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer
    public Animator animator;

    private void Awake()
    {
        UnityEngine.Debug.Log("Entro en RunPythonScript");
        animator = GetComponent<Animator>();
        RunPythonScript();

    }

    private void RunPythonScript()
    {
        UnityEngine.Debug.Log("Entro en RunPythonScript2");

        // Crear un motor de ejecución de Python
        var engine = Python.CreateEngine();

        // Cargar el script en Python
        var scope = engine.CreateScope();
        var source = engine.CreateScriptSourceFromFile(scriptPath);
        source.Execute(scope);

        // Obtener la referencia a la función en Python
        var function = scope.GetVariable<Func<PythonDictionary, object>>("nombre_de_la_funcion");

        // Crear un diccionario con los argumentos para la función en Python
        var arguments = new PythonDictionary();
        arguments["argumento1"] = "valor1";
        arguments["argumento2"] = 42;

        // Llamar a la función en Python y obtener el resultado
        var result = function(arguments);

        // Mostrar el resultado
        Console.WriteLine("Resultado de la función en Python:");
        Console.WriteLine(result);
    }
}

