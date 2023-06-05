using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

var engine = Python.CreateEngine();
var scope = engine.CreateScope();

string pythonCode = @"
def my_function(param):
    return 'Hello, ' + param
";

engine.Execute(pythonCode, scope);

dynamic pythonFunction = scope.GetVariable("my_function");
string result = pythonFunction("World");

Console.WriteLine(result);  // Imprime "Hello, World"
