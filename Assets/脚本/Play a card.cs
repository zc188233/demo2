using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playacard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Playcard(int A)
    {
       player.playerstatus=A;
       // 记录玩家选择
       player.RecordChoice(A);
    }
}
