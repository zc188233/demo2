using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCStatus : MonoBehaviour 
{
    public Text statusText;  // 状态显示文本
    
    private int stateIndex = 0;  // 当前状态索引：0=正常, 1=警戒, 2=攻击, 3=死亡
    
    // 绑定到按钮的OnClick，每次点击切换到下一状态
    public void NextState()
    {
        stateIndex = (stateIndex + 1) % 4;  // 循环递增，到3后回到0
        
        // 按顺序显示4种状态
        string[] states = { "正常", "警戒", "攻击", "死亡" };
        statusText.text = "状态：" + states[stateIndex];
    }
}