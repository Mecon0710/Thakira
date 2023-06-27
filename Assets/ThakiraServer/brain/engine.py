import openai 

from conversor import convert

openai.api_key = "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"
conversation = [{"role": "system", "content": "Eres un asistente de ayuda mental"}]


def think(userMessage):
    try:
        conversation.append({"role": "user", "content": f"{userMessage}"})
        response = openai.ChatCompletion.create(
            model="gpt-3.5-turbo",
            messages=conversation
        )
        print("user message total_tokens:" + str(response['usage']['total_tokens']))
        conversation.append({"role": "assistant", "content": response.choices[0].message.content})
        return response.choices[0].message['content'], conversation
    except Exception as ex:
        print(ex)
        return f"No pude responder porque {str(ex)}"


def speak(userVoiceAudioPath):
    # convertir audio a text
    user_message = convert.audio_to_text(userVoiceAudioPath)
    # pensar
    response, metadata = think(user_message['text'])
    # convertir texto a audio
    convert.text_to_audio(response)
    return {"user_audio_to_text": user_message, "teen_response": response, "metadata": metadata}
