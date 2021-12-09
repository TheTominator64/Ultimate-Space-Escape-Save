using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void OpenItchPage()
    {
        Application.OpenURL("https://thetominator.itch.io/");
    }

    public void OpenYouTubeChannel()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCdUyrmA2gLkyYvaTgROny2Q");
    }

    public void OpenDiscordInvite()
    {
        Application.OpenURL("https://discord.gg/Vcwu8HGbn9");
    }
}
