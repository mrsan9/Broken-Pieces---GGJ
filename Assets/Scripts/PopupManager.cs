using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public List<GameObject> Popups = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(this);
            }
        }
    }

    public void Show(string name)
    {
        for(int i=0;i<Popups.Count;++i)
        {
            if(Popups[i].name==name)
            {
                Popups[i].SetActive(true);
                break;
            }
        }
    }


    public void Hide(string name)
    {
        for (int i = 0; i < Popups.Count; ++i)
        {
            if (Popups[i].name == name)
            {
                Popups[i].SetActive(false);
                break;
            }
        }
    }
}
