using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public ItemComponent currentHandling = null;

    public Animator animator;

    public Material silhouetteMaterial;
    public ObjectControl control;
    public ItemSilhouetteGenerator silhouetteGenerator;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance!= null)
        {
            Destroy(this.gameObject);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PickUpObject();
        }        
    }

    public void PickUpObject()
    {

    }

    
}
