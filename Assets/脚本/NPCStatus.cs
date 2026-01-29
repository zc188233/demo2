using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NPCStatusReader : MonoBehaviour
{
    public Text displayText;           // UI显示文本
    public string fileName = "npc.txt";    // 文件名（如带后缀请写"npc.txt"）
    
    public enum State { 伪装 = 1, 刺探, 震慑, 共情 }
    public State currentState;

    void Start()
    {
        // 游戏开始时自动读取一次
        ReadAndSetState();
    }

    // 按钮绑定此方法
    public void ReadAndSetState()
    {
        int number = ReadNumberFromFile();
        number = Mathf.Clamp(number, 1, 4);
        
        currentState = (State)number;
        UpdateDisplay(number);
    }

    int ReadNumberFromFile()
    {
        // 文件路径：Assets/StreamingAssets/npc
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        
        if (!File.Exists(path))
        {
            Debug.LogError($"未找到文件：{path}\n请确保文件已放入Assets/StreamingAssets文件夹");
            displayText.text = "错误：文件不存在";
            return 1;
        }

        string content = File.ReadAllText(path);
        
        // 提取第一个1-4的数字
        foreach (char c in content)
        {
            if (c >= '1' && c <= '4')
                return c - '0';
        }
        
        Debug.LogWarning("文件内容不包含有效数字1-4，默认使用1");
        return 1;
    }

    void UpdateDisplay(int stateNum)
    {
        string[] names = { "", "伪装", "刺探", "震慑", "共情" };
        
        if (displayText != null)
        {
            displayText.text = "当前策略：" + names[stateNum];
            
            // 颜色区分：灰蓝/紫/红/绿
            Color[] colors = { 
                Color.clear, 
                new Color(0.3f, 0.5f, 0.7f),
                new Color(0.6f, 0.2f, 0.8f),
                new Color(0.9f, 0.2f, 0.2f),
                new Color(0.2f, 0.8f, 0.4f)
            };
            displayText.color = colors[stateNum];
        }
    }
}