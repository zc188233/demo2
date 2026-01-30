using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turstpoint : MonoBehaviour
{
    public Text trustpoint;           // 信任值数值显示
    public Text hintText;             // 提示文本（显示当前选中哪个）
    
    // 4个预备按钮的Image组件（用于高亮显示选中状态，可选）
    public Image buttonA;
    public Image buttonB;
    public Image buttonC;
    public Image buttonD;
    public Color selectedColor = Color.yellow;  // 选中色
    public Color normalColor = Color.white;     // 正常色
    
    private int pendingPenalty = 0;   // 当前待执行的惩罚值，0表示未选择

    void Start()
    {
        trustpoint.text = "100";
        pendingPenalty = 0;
        UpdateVisuals();
    }

    // 预备按钮A（-5）：Inspector中绑定 SelectPenalty(5)
    // 预备按钮B（-10）：Inspector中绑定 SelectPenalty(10)
    // 预备按钮C（-15）：Inspector中绑定 SelectPenalty(15)  
    // 预备按钮D（-20）：Inspector中绑定 SelectPenalty(20)
    public void SelectPenalty(int amount)
    {
        pendingPenalty = amount;
        hintText.text = "已选择 -" + amount + "，请确认执行";
        UpdateVisuals();
    }

    // 执行按钮绑定此方法
    public void ConfirmExecute()
    {
        int current = int.Parse(trustpoint.text);
        current -= pendingPenalty;
        trustpoint.text = current.ToString();
        
        // 执行后重置
        pendingPenalty = 0;
        hintText.text = "请选择卡牌";
        UpdateVisuals();
    }

    // 更新按钮高亮状态
    void UpdateVisuals()
    {
        // 根据当前选中值高亮对应按钮（假设5,10,15,20对应ABCD）
        buttonA.color = (pendingPenalty == 5) ? selectedColor : normalColor;
        buttonB.color = (pendingPenalty == 10) ? selectedColor : normalColor;
        buttonC.color = (pendingPenalty == 15) ? selectedColor : normalColor;
        buttonD.color = (pendingPenalty == 20) ? selectedColor : normalColor;
    }
}