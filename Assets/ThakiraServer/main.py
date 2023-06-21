# -*- coding: utf-8 -*-

from flask import Flask, request 
from brain import engine
from conversor import convert
import time

app = Flask(__name__)
palabras = []

"""
------------------
Funciones de prueba
------------------
"""

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
    path = './audios/input/voice.mp3'
    file_audio.save(path)
    message = convert.audio_to_text(path)
    return {"response": message}


@app.route('/speak', methods=['POST']) #Ejecutar en terminal: curl -X POST -F 'audio=@<path>' http://localhost:8080/speak
def speak():
    file_audio = request.files['audio']
    path = './audios/input/voice.mp3'
    file_audio.save(path)
    message = engine.speak(path)
    return {"response": message}


"""
------------------
Funcion principal
------------------
"""

def step1 ():
    convert.text_to_audio("Hola, soy Thakira. El día de hoy te haré una pequeña prueba de memoria. Empecemos!")#OUTPUT
    time.sleep(8)
    palabras = engine.think("Dame 3 palabras para memorizar que no sean muy largas y separas por comas")
    #Cascada, resplandor, aurora.
    response = "Memorízate estas 3 palabras: " + str(palabras[0])
    convert.text_to_audio(str(response))#OUTPUT
    time.sleep(10) #Usuario procesa la informacion
    response = "Ahora, elige un tema que te guste o interese y del cuál desees conocer su historia. Al final te plantearé una pregunta de opción múltiple. Por favor, indica claramente la letra correspondiente a tu respuesta."
    convert.text_to_audio(response)#OUTPUT
    time.sleep(40) #Usuario piensa en el tema
    return "Step 1 finalizado"

def step2 ():
    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    input = './audios/input/voice.mp3'
    output = convert.audio_to_text(input)#INPUT
    message = output['text']
    message = "Hablame sobre la historia de " + message + " Y hazme una unica pregunta de opción múltiple de comprensión de lectura difícil sobre lo que me vas a decir, sin decirme la respuesta enseguida."
    res = engine.think(message)
    convert.text_to_audio(res)#OUTPUT
    return "Step 2 finalizado"


def step3 ():
    input = './audios/input/voice.mp3'
    time.sleep(90) #Usuario piensa en la respuesta
    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    output2 = convert.audio_to_text(input)#INPUT
    message2 = output2['text']
    messageList = message2.split()
    print(message2)
    if  "a" in messageList  or "A" in messageList or "b" in messageList or "B" in messageList or "c" in messageList or "C" in messageList or "d" in messageList or "D" in messageList:
        res2 = engine.think(message2)
    else:
        res2 = convert.text_to_audio("No entendí la respuesta, por favor, repítela")
        time.sleep(60)
        output21 = convert.audio_to_text(input)#INPUT
        message21 = output21['text']
        res2 = engine.think(message21)

    convert.text_to_audio(res2)#OUTPUT
    #time.sleep(60)
    message3 = "Responde con esta frase 'A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras' y luego inserta todas las siguientes palabras " +str(palabras[0])+ " en una lista de 10 palabras cualquiera (diferentes a las insertadas), en desorden,separadas por comas, en esa lista obligatoriamente deben estar las palabras que te pedi que insertaras. Luego di 'Dime las 3 palabras que recuerdas'. No digas nada más. En ningun caso me digas las 3 palabras que se insertaron, exceptuando en la lista. Si respondo bien, diciendo las 3 palabras , felicitame, si no, dime que es incorrecto y cuales eran las 3 palabras. Mi respuesta obligatoriamente debe ser " +str(palabras[0])+", no puede ser una variación de esas palabras. Sólo hay un intento entonces no me ofrezcas más."
    res3 = engine.think(message3)
    convert.text_to_audio(res3)#OUTPUT
    return "Step 3 finalizado"


def step4 ():
    time.sleep(90) #Usuario intenta recordar las palabras
    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    output3 = convert.audio_to_text(input)#INPUT
    message3 = output3['text']
    print("Palabras dichas: " +message3)
    print("Palabras correctas: " +str(palabras[0]))
    messageList = message3.split()
    palabrasList = str(palabras[0]).split()
    print("Palabras dichas: " +str(messageList))
    print("Palabras correctas: " +str(palabrasList))
    cont = 0
    for i in messageList:
        if  i in palabrasList:
            cont +=1

    if cont == 3:
        res3 = "¡Felicidades! Has recordado correctamente las tres palabras que te pedí memorizar:" + str(palabras[0]) + " Excelente trabajo."
        convert.text_to_audio(res3)
        time.sleep(15)
    else:
        res3 = "Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: " + str(palabras[0])
        convert.text_to_audio(res3)
        time.sleep(15)
    
    message4  = "Gracias por participar en esta prueba. Nos vemos pronto o o"
    convert.text_to_audio(message4)#OUTPUT
    #time.sleep(8)
    return "Step 4 finalizado"


@app.route('/discurso', methods=['POST']) 
def main():
    print("Entro")
    convert.text_to_audio("Hola, soy Thakira. El día de hoy te haré una pequeña prueba de memoria. Empecemos!")#OUTPUT
    time.sleep(8)
    palabras = engine.think("Dame 3 palabras para memorizar que no sean muy largas y separas por comas")
    #Cascada, resplandor, aurora.
    response = "Memorízate estas 3 palabras: " + str(palabras[0])
    convert.text_to_audio(str(response))#OUTPUT
    time.sleep(10) #Usuario procesa la informacion

    response = "Ahora, elige un tema que te guste o interese y del cuál desees conocer su historia. Al final te plantearé una pregunta de opción múltiple. Por favor, indica claramente la letra correspondiente a tu respuesta."
    convert.text_to_audio(response)#OUTPUT
    time.sleep(40) #Usuario piensa en el tema

    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    input = './audios/input/voice.mp3'
    output = convert.audio_to_text(input)#INPUT
    message = output['text']
    message = "Hablame sobre la historia de " + message + " Y hazme una unica pregunta de opción múltiple de comprensión de lectura difícil sobre lo que me vas a decir, sin decirme la respuesta enseguida."
    res = engine.think(message)
    convert.text_to_audio(res)#OUTPUT

    """"
    ----------------
        Ejemplo 
    ----------------
    La historia de los robots se remonta a varios siglos atrás. Desde los antiguos mitos y leyendas hasta los avances tecnológicos de la actualidad, la idea de crear máquinas que pudieran realizar tareas humanas ha sido una constante en la imaginación humana.

    Sin embargo, los primeros intentos prácticos de construir robots se dieron en el siglo XX. En 1920, el escritor checo Karel Čapek acuñó el término "robot" en su obra de teatro "R.U.R." (Robots Universales Rossum), donde los robots eran seres artificiales creados para servir a los humanos, pero que finalmente se rebelaban contra sus creadores.

    A medida que avanzaba la tecnología, se desarrollaron los primeros robots industriales en la década de 1950. Estos robots eran utilizados principalmente en la línea de producción de fábricas y se programaban para realizar tareas repetitivas y peligrosas. Con el tiempo, los avances en la inteligencia artificial permitieron el desarrollo de robots más sofisticados capaces de realizar tareas más complejas.

    En la actualidad, los robots se utilizan en una amplia gama de campos, desde la industria automotriz hasta la medicina y la exploración espacial. Se han diseñado robots humanoides que pueden interactuar con los seres humanos de manera más natural, y los avances en la robótica siguen expandiendo las posibilidades de lo que los robots pueden lograr.

    Ahora, aquí tienes la pregunta de opción múltiple de comprensión de lectura:

    ¿Quién acuñó el término "robot" en su obra de teatro "R.U.R." publicada en 1920?

    A) Isaac Asimov
    B) Arthur C. Clarke
    C) Karel Čapek
    D) Aldous Huxley
    """

    #---------------------------------------------- Respuesta de la pregunta
    """
    ----------------
        Ejemplo 
    ----------------
    C
    """

    time.sleep(90) #Usuario piensa en la respuesta
    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    output2 = convert.audio_to_text(input)#INPUT
    message2 = output2['text']
    messageList = message2.split()
    print(message2)
    if  "a" in messageList  or "A" in messageList or "b" in messageList or "B" in messageList or "c" in messageList or "C" in messageList or "d" in messageList or "D" in messageList:
        res2 = engine.think(message2)
    else:
        res2 = convert.text_to_audio("No entendí la respuesta, por favor, repítela")
        time.sleep(60)
        output21 = convert.audio_to_text(input)#INPUT
        message21 = output21['text']
        res2 = engine.think(message21)

    convert.text_to_audio(res2)#OUTPUT
    time.sleep(60)
    """
    ----------------
        Ejemplo 
    ----------------
    Correcto, la respuesta es C) Karel Čapek. Fue el escritor checo quien acuñó el término "robot" en su obra de teatro "R.U.R." publicada en 1920. ¡Bien hecho!
    """
    #----------------------------------------------
    message3 = "Responde con esta frase 'A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras' y luego inserta todas las siguientes palabras " +str(palabras[0])+ " en una lista de 10 palabras cualquiera (diferentes a las insertadas), en desorden,separadas por comas, en esa lista obligatoriamente deben estar las palabras que te pedi que insertaras. Luego di 'Dime las 3 palabras que recuerdas'. No digas nada más. En ningun caso me digas las 3 palabras que se insertaron, exceptuando en la lista. Si respondo bien, diciendo las 3 palabras , felicitame, si no, dime que es incorrecto y cuales eran las 3 palabras. Mi respuesta obligatoriamente debe ser " +str(palabras[0])+", no puede ser una variación de esas palabras. Sólo hay un intento entonces no me ofrezcas más."
    res3 = engine.think(message3)
    convert.text_to_audio(res3)#OUTPUT
    """
    ----------------
        Ejemplo 
    ----------------
    A continuación te presentaré varias palabras y entre esas las que te pedí que memorizaras.

    Lista de palabras en desorden:

    Mesa
    Silla
    Cascada
    Perro
    Resplandor
    Lámpara
    Aurora
    Gato
    Libro
    Árbol
    Dime las 3 palabras que recuerdas.
    """
    #---------------------------------------------- Usuario dice las 3 palabras que recuerda 
    """
    ----------------
        Ejemplo 
    ----------------
    Cascada, resplandor, mariposa
    """

    time.sleep(90) #Usuario intenta recordar las palabras
    #USUARIO HABLA PERSONAJE SE QUEDA QUIETO
    output3 = convert.audio_to_text(input)#INPUT
    message3 = output3['text']
    print("Palabras dichas: " +message3)
    print("Palabras correctas: " +str(palabras[0]))
    messageList = message3.split()
    palabrasList = str(palabras[0]).split()
    print("Palabras dichas: " +str(messageList))
    print("Palabras correctas: " +str(palabrasList))
    cont = 0
    for i in messageList:
        if  i in palabrasList:
            cont +=1

    if cont == 3:
        res3 = "¡Felicidades! Has recordado correctamente las tres palabras que te pedí memorizar:" + str(palabras[0]) + " Excelente trabajo."
        convert.text_to_audio(res3)
        time.sleep(15)
    else:
        res3 = "Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: " + str(palabras[0])
        convert.text_to_audio(res3)
        time.sleep(15)

        

        

    """
    ----------------
        Ejemplo 
    ----------------
    Lamento informarte que la respuesta no es correcta. Las tres palabras que te pedí memorizar fueron: "Cascada", "Resplandor" y "Aurora". Gracias por participar y por tu intento.
    """
    #----------------------------------------------
    message4  = "Gracias por participar en esta prueba. Nos vemos pronto o o"
    convert.text_to_audio(message4)#OUTPUT
    time.sleep(8)

    return "Prueba finalizada"

if __name__ == '__main__':
    #main()
    app.run("0.0.0.0", 8080, False, True, threaded=True)
