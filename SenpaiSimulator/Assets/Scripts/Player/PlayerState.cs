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

    public void UpdateState(State _state)
    {
        switch (state)
        {
            case State.FREE:
                state = _state;
                break;

            case State.TALKING:
                state = _state;
                break;

            default:
                print("Something tried to change the Player State to a state that does not exist.");
                break;
        }
    }
}
