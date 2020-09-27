using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //public static readonly int FRAME_RATE = 100;

    // GRID
    //public static readonly int NUM_ROWS = 9;
    //public static readonly int NUM_COLUMNS = 9;

    // MOVEMENT
    public static readonly int ROTATION_SPEED = 2;
    public static readonly int STEP_DISTANCE = 1;
    public static readonly int JUMP_DISTANCE = 4;
    public static readonly int DEGREES_IN_STEP = 2;

    // BUFFER
    public static readonly int BUFFER_MAX_FRAMES = 8;

    // HEALTH
    public static readonly int MAX_HEALTH = 100;
    public static readonly float DAMAGE = 0.05f;
    public static readonly int SEDENTARY_THRESHOLD = 25;

    public static readonly int gameLength = 5;

    public static Dictionary<UIManager.Player, int> scores = new Dictionary<UIManager.Player, int>
    {
        {UIManager.Player.Player1, gameLength},
        {UIManager.Player.Player2, gameLength}
    };
}
