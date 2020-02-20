using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemComponent))]
public class ItemComponentInspector : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemComponent targ = target as ItemComponent;
        EItemState newValue = (EItemState)EditorGUILayout.EnumPopup(targ.state);
        if (newValue != targ.state)
        {
            targ.state = newValue;
            targ.SetObjectProperties();
            // do stuff, call functions, etc.
        }

    }
}
