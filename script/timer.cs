using UnityEngine;
using System;
using UnityEngine.UI;

public enum colorCode
{
    GREEN,
    RED,
}

public class timer : MonoBehaviour {
	public Text Timer;
	DateTime time = DateTime.Now;
	private int TimeDetect,MainTimer;
	public GameObject Ship;
    public Button playAndReplayButton;
    public Text playText;
    public static bool flowContinue = false;

	// Use this for initialization
	void Start () {
	
		Timer.text="Time:00" ;
		TimeDetect = time.Second;
        changeColor(colorCode.GREEN);
        MainTimer = 60;
        playAndReplayButton.gameObject.SetActive(true);
        playText.text = "Play";

    }
	
	// Update is called once per frame
	void Update () {

        if (!flowContinue)
            return;

		DateTime time1 = DateTime.Now;

		if(Mathf.Abs(TimeDetect - time1.Second) >= 1){
			TimeDetect = time1.Second;
			MainTimer=MainTimer-1;
		}

		if(MainTimer < 0) {

            playAndReplayButton.gameObject.SetActive(true);
            flowContinue = false;
            Timer.text="Time Over";
            playText.text = "Replay";
            Ship.GetComponent<MeshRenderer>().enabled = false ;
        }

		if(MainTimer == 10)
		{
            changeColor(colorCode.RED);
            
        }

        if (MainTimer >=0) {
		    Timer.text = "Time:" + MainTimer.ToString();
		}
	}

    void changeColor (colorCode code)
    {
        switch (code)
        {
            case colorCode.GREEN:
                Timer.color = Color.green;
                break;
            case colorCode.RED:
                Timer.color = Color.red;
                break;
        }
    }

    public void PlayAndReplay ()
    {
        Ship.GetComponent<MeshRenderer>().enabled = true;
        changeColor(colorCode.GREEN);
        flowContinue = true;
        MainTimer = 60;
        playAndReplayButton.gameObject.SetActive(false);
    }

}
