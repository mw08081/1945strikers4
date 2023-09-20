using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PanelSystem : MonoBehaviour
{
    static Dictionary<Type, BasePanel> panels = new Dictionary<Type, BasePanel>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static bool RegistPanel(Type panelType, BasePanel basePanel)
    {
        if(panels.ContainsKey(panelType))
        {
            Debug.LogError("Already exist Panel Type");

            return false;
        }

        panels.Add(panelType, basePanel);
        return true;
    }

    public static bool DestroyPanel(Type panelType)
    {
        if(!panels.ContainsKey(panelType))
        {
            Debug.LogError("No Exist Paenl");

            return false;
        }

        panels.Remove(panelType);
        return true;
    }

    public static BasePanel GetPanel(Type panelType)
    {
        if(!panels.ContainsKey(panelType))
        {
            Debug.LogError("No Exist Panel");
            return null;
        }

        return panels[panelType];
    }
}
