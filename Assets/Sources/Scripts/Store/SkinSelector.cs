using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{

    private INIParser m_parser;

    private int m_currentIndex = 0;

    public List<SkinData> skins;
    private SkinData currentSkin;
    public SkinData CurrentSkin { get => currentSkin; set => currentSkin = value; }

    public Image skinPreviewImage;
    public Text skinPreviewName;
    public Image skinPreviewSelectButtonCheck;

    private void Awake()
    {
        m_parser = new INIParser();
        m_parser.Open(Application.persistentDataPath + "/settings.ini");
        m_currentIndex = m_parser.ReadValue("Player", "SkinSelected", 0);
        m_parser.Close();

        if (m_currentIndex < 0 || m_currentIndex >= skins.Count)
        {
            m_currentIndex = 0;
        }
        CurrentSkin = skins[m_currentIndex];

        UpdateSkinPreview(m_currentIndex);
    }

    public void PreviousSkin()
    {
        int newIndex = m_currentIndex - 1;
        UpdateSkinPreview(newIndex);
    }

    public void NextSkin()
    {
        int newIndex = m_currentIndex + 1;
        UpdateSkinPreview(newIndex);
    }

    public void SelectCurrentSkin()
    {
        CurrentSkin = skins[m_currentIndex];
        skinPreviewSelectButtonCheck.enabled = true;

        m_parser.Open(Application.persistentDataPath + "/settings.ini");
        m_parser.WriteValue("Player", "SkinSelected", m_currentIndex);
        m_parser.Close();
    }

    private void UpdateSkinPreview(int index)
    {
        if (index < 0) {
            index = skins.Count - 1;
        } else if(index >= skins.Count)
        {
            index = 0;
        }

        m_currentIndex = index;
        skinPreviewImage.sprite = skins[m_currentIndex].Preview;
        skinPreviewName.text = skins[m_currentIndex].SkinName;

        if(CurrentSkin == skins[index])
        {
            skinPreviewSelectButtonCheck.enabled = true;
        } else
        {
            skinPreviewSelectButtonCheck.enabled = false;
        }
    }

}
