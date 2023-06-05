import openai
import os
from gtts import gTTS
from pydub import AudioSegment

def audio_to_text(path):
    #audio = AudioSegment.from_mp3(path)
    # Convertir el audio a formato WAV (formato requerido por Whisper)
    #audio_wav = audio.export(format="wav")
    audio_file_to_translate = open(path, "rb")
    transcript = openai.Audio.transcribe("whisper-1", audio_file_to_translate, language="es")
    return transcript


def text_to_audio(message):
    file = 'audios/output/teen.mp3'
    tts = gTTS(text=message, lang='es', tld="es", slow=False)

    # Guardar el audio generado por gTTS en un archivo temporal
    tts.save('audios/output/temp.mp3')

    # Cargar el audio con pydub
    audio = AudioSegment.from_file('audios/output/temp.mp3')

    # Aumentar la velocidad en un factor determinado (acelerar)
    factor = 0.8
    audio_acelerado = audio.speedup(playback_speed=factor)

    # Guardar el audio acelerado en un nuevo archivo
    audio_acelerado.export(file, format='mp3')

    # Eliminar el archivo temporal
    os.remove('audios/output/temp.mp3')

    return "ok"
