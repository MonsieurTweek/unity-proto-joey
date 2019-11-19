using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    public GameManager gameManager;

    public Text display;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

        if(gameManager.State == GameManager.GameState.Ready)
        {
            #if !UNITY_EDITOR
                bool isTouching = (Input.touchCount > 0);
            #else
                bool isTouching = Input.GetMouseButton(0);
            #endif

            if(isTouching == true)
            {
                gameManager.StartGame();
                display.enabled = false;
            }

        }

    }
}
