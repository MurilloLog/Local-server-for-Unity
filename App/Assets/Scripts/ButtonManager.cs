using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* Home activity */
    public void home2CRoom()
    {
        SceneManager.LoadScene("CRoom");
    }

    /* CRoom activity */
    public void CRoom2Host()
    {
        SceneManager.LoadScene("Host");
    }

    public void CRoom2Guest()
    {
        SceneManager.LoadScene("Guest");
    }

    /* Game */
    public void SetConfig2Game()
    {
        SceneManager.LoadScene("App");
    }
}
