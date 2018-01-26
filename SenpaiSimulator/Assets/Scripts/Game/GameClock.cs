using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameClock : MonoBehaviour {

    public float sec;
    private float _sec;
    public int min;
    public int hr;
    public string suffix;
    string clockTime;

    public static GameClock Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        _sec = sec;
	}
	
	public void UpdateThis () {
        _sec += Time.deltaTime;

        sec = (float)Math.Floor(_sec);

        print(hr.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00") + suffix);

        if (sec >= 60)
        {
            _sec = 0;
            min++;
            if (min >= 60)
            {
                min = 0;
                hr++;
                if (hr >= 13)
                {
                    hr = 1;
                    switch (suffix)
                    {
                        case "am":
                            suffix = "pm";
                            break;

                        case "pm":
                            suffix = "pm";
                            break;

                        default:
                            print("Time suffix not valid, defaulting to 'am.'");
                            suffix = "am";
                            break;
                            
                    }
                }
            }
        }
	}

    void ResetClock()
    {
        _sec = 0;
        min = 0;
        hr = 0;
    }
}
