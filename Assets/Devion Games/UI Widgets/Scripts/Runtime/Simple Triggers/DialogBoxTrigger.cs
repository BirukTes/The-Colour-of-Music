using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.UIWidgets;
using UnityEngine.Events;
using System;

public class DialogBoxTrigger : MonoBehaviour
{
    public string title;
    [TextArea]
    public string text;
    public Sprite icon;
    public string[] options;

    private DialogBox m_DialogBox;

    private void Start()
    {
        this.m_DialogBox = FindObjectOfType<DialogBox>();   
    }

    public void Show() {
        if (m_DialogBox == null)
            Start();

        m_DialogBox.Show(title, text, icon, null, options);
    }

    public void ShowWithCallback(UnityAction<int> result)
    {
        if (m_DialogBox == null)
            Start();
        m_DialogBox.Show(title, text, icon, result, options);
    }

    private void OnDialogResult(int index)
    {
        m_DialogBox.Show("Result", "Callback Result: "+options[index], icon, null, "OK");
    }
}
