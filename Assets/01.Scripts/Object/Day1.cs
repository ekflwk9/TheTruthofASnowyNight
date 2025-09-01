using GameMathods;
using UnityEngine;

public class Day1 : MonoBehaviour,
IDay
{
    public bool EventTime()
    {
        switch (GameService.progress.gameHour)
        {
            case 0:
                if (GameService.progress.gameMin == 20) return true;
                break;

            case 1:
                if (GameService.progress.gameMin == 5) return true;
                if (GameService.progress.gameMin == 50) return true;
                break;

            case 2:
                if (GameService.progress.gameMin == 35) return true;
                break;

            case 3:
                if (GameService.progress.gameMin == 15) return true;
                break;

            case 4:
                if (GameService.progress.gameMin == 1) return true;
                if (GameService.progress.gameMin == 50) return true;
                break;

            case 5:
                if (GameService.progress.gameMin == 25) return true;
                break;
        }

        return false;
    }
}
