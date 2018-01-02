using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public static State state;

    public enum State
    {
        FREE,
        TALKING
    }
}
