using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{

    public GameObject Platform;
    public GameObject Title;
    public GameObject Menu;
    public GameObject MenuOptions;

    private float m_delayAnimation = 0f;
    private bool m_animate = true;
    private bool m_isMenuDisplayed = false;
    private Vector3 m_platformStartPosition;
    private float m_platformDisplayedPositionY = 3f;

    private void Awake()
    {
        m_platformStartPosition = Platform.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        Animation anim = Title.GetComponent<Animation>();
        anim.Play("MenuScaleIn");
        m_delayAnimation = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_delayAnimation > 0f)
        {
            m_delayAnimation -= Time.deltaTime;
        } else if(m_animate == true)
        {
            ScaleInMenu();
            m_animate = false;
        }
    }

    public void ScaleInMenu()
    {
        Animation[] anims = Menu.GetComponentsInChildren<Animation>();
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("MenuScaleIn");
        }
    }

    public void ScaleOutMenu()
    {
        Animation[] anims = Menu.GetComponentsInChildren<Animation>();
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("MenuScaleOut");
        }
    }

    public void ToggleMenuOptions()
    {
        if(m_isMenuDisplayed == false)
        {
            ScaleOutMenu();
            Platform.GetComponent<Animation>().Play("PlatformUp");
            MenuOptions.GetComponent<Animation>().Play("MenuOptionsScaleIn");
            m_isMenuDisplayed = true;
        } else
        {
            Platform.GetComponent<Animation>().Play("PlatformDown");
            MenuOptions.GetComponent<Animation>().Play("MenuOptionsScaleOut");
            m_isMenuDisplayed = false;
            m_animate = true;
            m_delayAnimation = 1f;
        }
    }
}
