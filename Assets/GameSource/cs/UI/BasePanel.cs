using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    void Start()
    {
        InitializingPanel();
    }

    protected virtual void InitializingPanel()
    {
        PanelSystem.RegistPanel(GetType(), this);
    }

    void Update()
    {
        UpdatingPanel();
    }

    protected virtual void UpdatingPanel()
    {

    }

    public virtual void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
