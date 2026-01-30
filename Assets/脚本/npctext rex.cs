using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class npctextrex : MonoBehaviour
{
    public TMP_Text npctext;
    
    public string[] lines = {
        "行行好…给点吃的吧。我已经三天…没看见罐头了。（请出牌）",
        "好吧，我不是要饭的。我是在找‘那种药’…能让人忘记一些事的药。这诊所里…也许还有库存。！（请出牌）",
        "“…呃，别动手。”（请出牌）",
        "“你真好心…（请出牌）",
        "我以前是‘真言社’的‘基层认知协调员’…就是给不听话的人‘贴标签’的。瘟疫失控后，我最想删除的，是自己记忆里的那些哭喊声。(点击回合结束继续)",
        "雷克斯（撩起衣领，露出脖颈后的激光码）：“看，这是我的‘病历’。”“小心穿制服的人……他们还在‘回收’我们。”"
    };
    
    private int index = 0;
    //private int player = 1;
    void Start()
    {
    }
    
    void Update()
    {
        
    }
    public void NextLine()
    {
        if (index == 0)
        {
            if(player.playerstatus == 1)
            {
                npctext.text = lines[1];
                index++;
                rex.rexstatus = 1;
            }
            else if(player.playerstatus == 4)
            {
                npctext.text = lines[3];
            }
            else if(player.playerstatus == 3)
            {
                npctext.text = lines[2];
            }
        }
        else if (index == 1)
        {
            npctext.text = lines[4];
            index++;
            rex.rexstatus = 0;
        }
        else if (index == 2)
        {
            npctext.text = lines[5];
        }
    }
}