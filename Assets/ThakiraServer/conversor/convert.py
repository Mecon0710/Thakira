import openai
import os
from gtts import gTTS
from pydub import AudioSegment
from pydub.exceptions import InvalidDuration

def audio_to_text(path):
    #audio = AudioSegment.from_mp3(path)
    # Convertir el audio a formato WAV (formato requerido por Whisper)
    #audio_wav = audio.export(format="wav")
    audio_file_to_translate = open(path, "rb")
    transcript = openai.Audio.transcribe("whisper-1", audio_file_to_translate, language="es")
    return transcript




def text_to_audio(message, crossfade=100):
    file = './audios/output/text_to_audio.mp3' 
    temporal = './audios/output/temp.mp3'
    if type(message) == tuple:
        message = message[0]
    print("message: ")
    print(message)
    tts = gTTS(text=message, lang='es', tld="es", slow=False)
    factor = 1.25

    # Guardar el audio generado por gTTS en un archivo temporal
    tts.save(file)
    """
    try:
        # Cargar el audio con pydub
        audio = AudioSegment.from_file(temporal)

        # Aumentar la velocidad en un factor determinado (acelerar) si la duración es mayor a cero
        if len(audio) > 0:
            if len(audio) > crossfade * 2:
                # Aplicar el fundido solo si la duración del audio es mayor al doble del fundido
                crossfade = min(crossfade, len(audio) // 2)
                audio_acelerado = audio.speedup(playback_speed=factor).fade_in(crossfade).fade_out(crossfade)
            else:
                audio_acelerado = audio
        else:
            audio_acelerado = audio

        print("audio_acelerado!!!")
        # Guardar el audio acelerado en un nuevo archivo
        audio_acelerado.export(file, format='mp3')

    except InvalidDuration:
        # En caso de duración cero o inválida, guardar el audio original sin modificar
        tts.save(file)

    # Eliminar el archivo temporal
    os.remove(temporal)
    """

    return file



