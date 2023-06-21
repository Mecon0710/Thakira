using System;
using System.Diagnostics;
using UnityEngine;
using IronPython.Hosting;
using IronPython.Runtime;           


public class PythonScriptRunner : AudioUser
{
    //private string pythonPath = "python3"; // Ruta al ejecutable de Python en tu sistema
    private string scriptPath = "Assets/ThakiraServer/main.py"; // Ruta al archivo de Python que deseas ejecutar /Users/Meli/Documents/Demo_2/Thakira/Thakira/Assets/ThakiraServer
    public Animator animator;

    private void Awake()
    {
        UnityEngine.Debug.Log("Entro en tunel");
        animator = GetComponent<Animator>();
        RunPythonScript();

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
}

