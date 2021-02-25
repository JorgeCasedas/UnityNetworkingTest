using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance;

    public Image myHpBar;
    public Image notMyHpBar;
    public float notMyHp;
    float lastHp;

    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastHp = notMyHp = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastHp != notMyHp)
        {
            notMyHpBar.fillAmount = notMyHp;
            lastHp = notMyHp;
        }
    }

    public void ChangeMyHp (float hp)
    {
        myHpBar.fillAmount = hp / 100;
    }

    public void ChangeNotMyHp (float hp)
    {
        notMyHp = hp / 100;

    }
}
