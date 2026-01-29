using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StateSequenceController : MonoBehaviour
{
    [Header("UI组件")]
    public Text displayText;           // 显示文本的UI
    public Button nextButton;          // 切换按钮
    
    [Header("状态配置")]
    public string[] stateNames = { "伪装", "刺探", "震慑", "共情" };
    public Color[] stateColors = { 
        new Color(0.3f, 0.5f, 0.7f),   // 灰蓝 - 伪装
        new Color(0.6f, 0.2f, 0.8f),   // 紫 - 刺探
        new Color(0.9f, 0.2f, 0.2f),   // 红 - 震慑
        new Color(0.2f, 0.8f, 0.4f)    // 绿 - 共情
    };
    
    [Header("序列控制")]
    [Tooltip("数字序列：1=伪装, 2=刺探, 3=震慑, 4=共情")]
    public List<int> numberSequence = new List<int> { 1, 2, 3, 4 }; // 默认顺序
    public string fileName = "sequence.txt"; // 可选：从文件读取序列
    
    [Header("运行状态")]
    private int currentIndex = 0;      // 当前在序列中的位置
    private bool isSequenceFinished = false;
    
    public enum State { 伪装 = 1, 刺探 = 2, 震慑 = 3, 共情 = 4 }
    public State currentState;

    void Start()
    {
        // 绑定按钮事件
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClick);
        
        // 尝试从文件加载序列（如果失败就用Inspector里设置的）
        LoadSequenceFromFile();
        
        // 初始化显示第一个
        ApplyStateByIndex(0);
    }

    // 按钮点击回调
    public void OnNextButtonClick()
    {
        if (isSequenceFinished)
        {
            Debug.Log("序列已结束，重新开始");
            currentIndex = 0;
            isSequenceFinished = false;
        }
        else
        {
            currentIndex++;
            if (currentIndex >= numberSequence.Count)
            {
                isSequenceFinished = true;
                currentIndex = numberSequence.Count - 1; // 保持在最后一个
                
                if (displayText != null)
                {
                    displayText.text = "序列结束";
                    displayText.color = Color.gray;
                }
                return;
            }
        }
        
        ApplyStateByIndex(currentIndex);
    }

    // 应用序列中指定索引的状态
    void ApplyStateByIndex(int index)
    {
        if (numberSequence.Count == 0) return;
        
        int stateNum = numberSequence[index];
        stateNum = Mathf.Clamp(stateNum, 1, 4); // 确保在1-4范围内
        
        currentState = (State)stateNum;
        
        // 更新UI
        UpdateDisplay(stateNum);
        
        Debug.Log($"[{index + 1}/{numberSequence.Count}] 当前状态: {stateNames[stateNum - 1]}");
    }

    void UpdateDisplay(int stateNum)
    {
        if (displayText == null) return;
        
        int arrayIndex = stateNum - 1; // 数组从0开始
        
        displayText.text = $"当前策略：{stateNames[arrayIndex]}";
        
        if (arrayIndex < stateColors.Length)
            displayText.color = stateColors[arrayIndex];
    }

    // 从文件加载数字序列（每行一个数字，或用逗号分隔）
    void LoadSequenceFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        
        if (!File.Exists(path))
        {
            Debug.Log($"未找到序列文件，使用Inspector预设序列: {string.Join(",", numberSequence)}");
            return;
        }

        try
        {
            string content = File.ReadAllText(path);
            ParseSequence(content);
            Debug.Log($"从文件加载序列: {string.Join(",", numberSequence)}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取序列文件失败: {e.Message}");
        }
    }

    // 解析字符串为数字序列
    void ParseSequence(string content)
    {
        numberSequence.Clear();
        
        // 尝试按行解析
        string[] lines = content.Split(new[] { '\n', '\r', ',', ' ' }, 
            System.StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (int.TryParse(trimmed, out int num))
            {
                if (num >= 1 && num <= 4)
                    numberSequence.Add(num);
            }
        }
        
        // 如果没有解析到任何数字，设置默认值
        if (numberSequence.Count == 0)
        {
            numberSequence.AddRange(new[] { 1, 2, 3, 4 });
            Debug.LogWarning("文件中没有找到有效数字(1-4)，使用默认序列");
        }
    }

    // 公共方法：动态设置序列（供其他脚本调用）
    public void SetSequence(List<int> newSequence)
    {
        numberSequence = newSequence;
        currentIndex = 0;
        isSequenceFinished = false;
        ApplyStateByIndex(0);
    }

    // 公共方法：重置到开始
    public void ResetSequence()
    {
        currentIndex = 0;
        isSequenceFinished = false;
        ApplyStateByIndex(0);
    }

    void OnDestroy()
    {
        // 取消按钮绑定
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextButtonClick);
    }
}