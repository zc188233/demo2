using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class carddisplay : MonoBehaviour
{

    public Text nameText;
    public Image backgroundImage;

    public Card card;

    //public Card card;
    // Start is called before the first frame update
    void Start()
    {
        Showcard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Showcard()
    {
        nameText.text = card.name;
        
    }
}
