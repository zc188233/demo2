using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turstpoint : MonoBehaviour
{
    public static int turst = 0;
    public static int san = 100;
    public Text trustpoint;
    public Text sanpoint;
    void Start()
    {
        
    }
    public void trustchange()
    {
        if(player.playerstatus == 1&&rex.rexstatus == 2)
        {
            turst += 15;
            san  -= 0;
        }
        else if(player.playerstatus == 2&&rex.rexstatus == 2)
        {
            turst += 0;
            san  -= 5;
        }
        else if(player.playerstatus == 3&&rex.rexstatus == 2)
        {
            turst += -10;
            san -= 10;
        }
        else if(player.playerstatus == 4&&rex.rexstatus == 2)
        {
            turst += 5;
            san -= 5;
        } 
        else if(player.playerstatus == 1&&rex.rexstatus == 1)
        {
            turst += -10;
            san  -= 10;
        }
        else if(player.playerstatus == 2&&rex.rexstatus == 1)
        {
            turst += 15;
            san  -= 0;
        }
        else if(player.playerstatus == 2&&rex.rexstatus == 1)
        {
            turst += 0;
            san  -= 5;
        }
        else if(player.playerstatus == 2&&rex.rexstatus == 1)
        {
            turst += -30;
            san  -= 20;
        }

        trustpoint.text = turst.ToString();
        sanpoint.text = san.ToString();
    }
}