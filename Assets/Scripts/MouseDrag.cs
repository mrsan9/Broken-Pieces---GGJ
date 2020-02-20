using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseDrag : MonoBehaviour {

    float distance = 10;

    float mZPos;

    ObjectControl control;

    private void Awake()
    {
        control = transform.GetComponentInParent<ObjectControl>();
        
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < control.brokenObjs.Count; ++i)
        {
            if (this.gameObject == control.brokenObjs[i])
            {
                mZPos = Camera.main.WorldToScreenPoint(control.oldPositions[i].transform.position).z;
            }
        }
    }
    private void OnMouseDrag()
    {
        for (int i = 0; i < control.brokenObjs.Count; ++i)
        {
            if (this.gameObject == control.brokenObjs[i])
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mZPos);
                Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                objectPos.z = control.oldPositions[i].transform.position.z;
                transform.position = objectPos;
            }
        }
    }
}
