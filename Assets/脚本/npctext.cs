using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npctext : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] private TMP_Text targetText;
    
    [Header("文本内容")]
    [SerializeField] private string newText = "文本已改变！";
    
    // 调用按钮
    public void ChangeText()
    {
        if (targetText != null)
        {
            targetText.text = newText;
        }
        else
        {
            Debug.LogWarning("未指定目标文本组件！", this);
        }
    }
}