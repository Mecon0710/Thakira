from flask import Flask, request 
from brain import engine
from conversor import convert
app = Flask(__name__)


@app.route('/status')
def hello_world():
    return 'ok'


@app.route('/listen', methods=['POST'])
def listen():
    data = request.get_json()
    message = data['message']
    response = engine.think(message)
    return response


@app.route('/text_to_audio', methods=['POST'])
def text_to_audio():
    data = request.get_json()
    message = data['message']
    response = convert.text_to_audio(message)
    return response


@app.route('/audio_to_text', methods=['POST']) 
def audio_to_text():
    file_audio = request.files['audio']
    path = 'audios/input/voice.mp3'
    file_audio.save(path)
    message = convert.audio_to_text(path)
    return {"response": message}

#Funcion principal
@app.route('/speak', methods=['POST']) #Ejecutar en terminal: curl -X POST -F 'audio=@<path>' http://localhost:8080/speak
def speak():
    file_audio = request.files['audio']
    path = 'audios/input/voice.mp3'
    file_audio.save(path)
    message = engine.speak(path)
    return {"response": message}

@app.route('/discurso', methods=['POST']) 
def discurso():
    file = 'audios/output/teen.mp3'
    n=0
    # Abrir el archivo en modo lectura
    with open('inicial/intro.txt', 'r') as file:
        # Leer y mostrar cada línea del archivo
        for line in file:
            if n==0:
                tts=convert.text_to_audio(line)#OUTPUT
                n=n+1
            elif n==1:
                response = engine.think(line)
                response = "Memorizate estas 3 palabras: " + response +"        Ahora, dime un tema del que quieras hablar, que te guste o interese."
                convert.text_to_audio(response)#OUTPUT
                n=n+1

    input = 'audios/input/voice.mp3'
    output = convert.audio_to_text(input)#INPUT
    message = output['text']
    message = "Hablame sobre " + message + " version extendida Y hazme una pregunta de opcion multiple de comprension de lectura dificil sin decirme la respuesta enseguida."
    res = engine.think(message)
    convert.text_to_audio(res)#OUTPUT
    #----------------------------------------------
    output2 = convert.audio_to_text(input)#INPUT
    message2 = output2['text']
    res2 = engine.think(message2)
    convert.text_to_audio(res2)#OUTPUT
    #----------------------------------------------
    message3 = "Di 'A continuación te presentaré varias palabras y entre esas las que te pedi que memorizaras' y luego nombra las 3 palabras con 7 más y luego di 'Dime las 3 palabras que recuerdas'"
    res3 = engine.think(message3)
    convert.text_to_audio(res3)#OUTPUT
    #----------------------------------------------
    output3 = convert.audio_to_text(input)#INPUT
    message3 = output3['text']
    res4 = engine.think(message3)
    convert.text_to_audio(res4)#OUTPUT
    #----------------------------------------------
    message4  = "Gracias por participar en esta prueba. Nos vemos prontooo"
    convert.text_to_audio(res3)#OUTPUT

    return "Prueba finalizada"

if __name__ == '__main__':
    app.run("0.0.0.0", 8080, False, True, threaded=True)
