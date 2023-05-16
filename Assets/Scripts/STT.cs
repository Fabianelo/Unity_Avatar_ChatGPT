//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// <code>
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ChatGPTWrapper;
using System;
using UnityEngine.UIElements;

public class STT : MonoBehaviour
{
    private object threadLocker = new object();
    private string message;

    bool isRecognizedSpeech_OK = false;
    bool isRecognizedSpeech_Error = false;
    public async void ButtonClick()
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // Replace with your own subscription key and service region (e.g., "westus").
        var config = SpeechConfig.FromSubscription(Credentials.Azure_SubscriptionKey, Credentials.Azure_ServiceRegion);
        var autoDetectSourceLanguageConfig =
            AutoDetectSourceLanguageConfig.FromLanguages(
                new string[] { "en-US", "de-DE", "es-ES" });
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioConfig))
        {
            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.

            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            string newMessage = string.Empty;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                newMessage = result.Text;
                isRecognizedSpeech_OK = true;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
                isRecognizedSpeech_Error = true;
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
                isRecognizedSpeech_Error = true;
            }

            lock (threadLocker)
            {
                message = newMessage;
            }
        }
    }

    void Update()
    {
        if (isRecognizedSpeech_OK)
        {
            isRecognizedSpeech_OK = false;
            Manager._OnSSTResponse_OK(message);
        }
        if (isRecognizedSpeech_Error)
        {
            isRecognizedSpeech_Error = false;
            Manager._OnSSTResponse_ERROR(message);
        }
    }
}