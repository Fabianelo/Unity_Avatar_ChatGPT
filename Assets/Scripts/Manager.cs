using ChatGPTWrapper;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.VersionControl;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Manager : MonoBehaviour
{
    public UnityEngine.UI.Image imageOutputPanel;
    public TMPro.TextMeshProUGUI outputText;
    [Header("Componentes")]
    public STT stt;
    public bool stt_Enabled;
    public ChatGPTConversation chatGPT;
    public bool chatGPT_Enabled;
    public TTS tts;
    public bool tts_enabled;
    public AvatarController avatarController;
    public bool avatar_enabled;
    public static Action<string> _OnSSTResponse_OK;
    public static Action<string> _OnSSTResponse_ERROR;
    public static Action<string> _OnChatGPTResponse;
    public static Action<string> _OnChatGPTError;
    private void OnEnable()
    {
        _OnSSTResponse_OK += SSTResponse_OK;
        _OnSSTResponse_ERROR += SSTResponse_ERROR;
        _OnChatGPTResponse += ChatGPTResponse;
        _OnChatGPTError += ChatGPTError;
    }

    private void SSTResponse_OK(string value)
    {
        outputText.text += "<color=\"grey\">User: " + value + "</color>\n";
        if (!chatGPT_Enabled)
        {
            stt_Enabled = true;
            imageOutputPanel.color = UnityEngine.Color.black;
        }
        else
        {
            imageOutputPanel.color = UnityEngine.Color.blue;
            chatGPT.SendToChatGPT(value);
        }
    }
    private void SSTResponse_ERROR(string value)
    {
        outputText.text += value + "\n";
        stt_Enabled = true;
        imageOutputPanel.color = UnityEngine.Color.black;
    }
    public void ChatGPTResponse(string value)
    {
        if (tts_enabled)
        {
            StartCoroutine(WaitUntilSoundPlay());
            tts.PlayText(value);        
            outputText.text += "ChatGPT: " + value + "\n";
        }
        else
        {
            stt_Enabled = true;
            imageOutputPanel.color = UnityEngine.Color.black;
            outputText.text += "ChatGPT: " + value + "\n";
        }
    }
    private void ChatGPTError(string value)
    {
        stt_Enabled = true;
        imageOutputPanel.color = UnityEngine.Color.black;
        outputText.text += "<color=\"red\"> ChatGPT: " + value + "</color>\n";
    }
    private IEnumerator WaitUntilSoundPlay()
    {
        yield return new WaitUntil(() => tts.audioSource.isPlaying == true);
        avatarController.SetTalkPose(true);
        yield return new WaitUntil(() => tts.audioSource.isPlaying == false);
        avatarController.SetTalkPose(false);
        imageOutputPanel.color = UnityEngine.Color.black;
        stt_Enabled = true;
    }
    private void Start()
    {
        outputText.text = String.Empty;
        if (avatar_enabled)
        {
            RectTransform rt = imageOutputPanel.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(1100, rt.offsetMin.y);
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (stt_Enabled)
            {
                stt_Enabled = false;
                stt.ButtonClick();
                imageOutputPanel.color = UnityEngine.Color.red;
            }
        }
    }
}
