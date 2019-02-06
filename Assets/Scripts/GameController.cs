using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public string inputMethod = "keyboard";

    public static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        var inputArg = GetArg("-inputMethod");
        switch(inputArg)
        {
            case "keyboard":
            case "arcade":
                inputMethod = inputArg;
                break;
            default:
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("ArcadeExit") > 0)
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
