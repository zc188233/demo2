using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playertext1 : MonoBehaviour
{

    public TMP_Text playertext;
    public string[] lines = {
        "你的靴子很新。",
        "我……我也没了。",
        "滚开，别挡路！",
        "我懂，饥饿很难熬。",
        "我没见过药。",
        "药在哪？说！",
        "你想忘记什么。",
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //主角对话判断函数
    public void speak()
    {
        if (npctextrex.index == 0)
        {
            if(player.playerstatus == 1)
            {
                playertext.text = lines[0];
            }
            else if(player.playerstatus == 2)
            {
                playertext.text = lines[1];
            }
            else if(player.playerstatus == 3)
            {
                playertext.text = lines[2];
            }
            else if(player.playerstatus == 4)
            {
                playertext.text = lines[3];
            }
        }
        else if (npctextrex.index == 1)
        {
            if(player.playerstatus == 2)
            {
                playertext.text = lines[4];
            }
            else if(player.playerstatus == 3)
            {
                playertext.text = lines[5];
            }
            else if(player.playerstatus == 4)
            {
                playertext.text = lines[6];
            }
        }
    }
}
