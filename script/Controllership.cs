/*Main scrit to Handle the Creation of Object/Obstacles using Object pooling */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Controllership : MonoBehaviour {
	private Rigidbody rb;
	public Transform ShipCube,ShipSphere;
	private GameObject GameShip;
	private float positionZ;
	public Text Score,Timer;
	private int Count,j;
    public int maxPool = 15;
	private List<GameObject>mylist;
    public Queue<string> positionQueue;
	private List<GameObject>myOtherList;
	float moveHorizontal,moveVertical,Location;
	float itt = 1.0f;
    public float DefaultSpeed = 1f;
    public float SpeedIncreament = 0.01f;
	float distanceCamera;
    public float maxVisibleDistance = 100.0f;
    float theHalfGameobjectDistanceValue = 200.0f;
    Vector3 objPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
		Location = 10.0f;
		mylist = new List<GameObject>();
		myOtherList = new List<GameObject>();
        positionQueue = new Queue<string>();
        string thePositionBoundString = string.Empty;
        for (int i=0; i<10; i++) {
            objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
            while (positionQueue.Contains(thePositionBoundString))
            {
                objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
            }
            GameShip = Instantiate (ShipCube.gameObject, objPosition, Quaternion.identity)as GameObject;
            positionQueue.Enqueue(thePositionBoundString);
            myOtherList.Add(GameShip);
			if(i%2 == 0) {
                objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                while (positionQueue.Contains(thePositionBoundString))
                {
                    objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                    thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
                }
                GameShip = Instantiate (ShipSphere.gameObject, objPosition, Quaternion.identity)as GameObject;
                positionQueue.Enqueue(thePositionBoundString);
                myOtherList.Add(GameShip);
			
			}
			j = i;
			Location=Location+10.0f;
			distanceCamera =Mathf.Abs(Vector3.Distance (transform.position,Camera.main.transform.position));
		}

		for (j=j+1; j<20; j++) {

            objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
            while (positionQueue.Contains(thePositionBoundString))
            {
                objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
            }
            GameShip = Instantiate (ShipCube.gameObject, objPosition, Quaternion.identity)as GameObject;
            positionQueue.Enqueue(thePositionBoundString);
            mylist.Add(GameShip);
			if(j % 2 == 0){
                objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                while (positionQueue.Contains(thePositionBoundString))
                {
                    objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), transform.position.z + Location);
                    thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
                }
                GameShip = Instantiate (ShipSphere.gameObject, objPosition, Quaternion.identity)as GameObject;
                positionQueue.Enqueue(thePositionBoundString);
                mylist.Add(GameShip);
			}
			Location=Location+10.0f;
		}

		positionZ = this.transform.position.z;
		rb=this.GetComponent<Rigidbody>();
		Score.text="score=00";
		Count = 0;
	}
	

	void FixedUpdate () {

        if (!timer.flowContinue)
        {
            if (Count > 0)
            {
                Count = 0;
            }
            return;
        }

        float distance;

		distance = Mathf.Abs(positionZ-this.transform.position.z) ;

		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");


		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Rotate(new Vector3(0.0f,-15.0f,0.0f) * Time.deltaTime);
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			transform.Rotate(new Vector3(0.0f,15.0f,0.0f) * Time.deltaTime);
		}
		else{
			transform.Rotate(new Vector3(0.0f,0.0f,0.0f) * Time.deltaTime);
		}

        if (Input.GetKey(KeyCode.Space))
            itt = itt + SpeedIncreament;
        else
            itt = DefaultSpeed;

        Vector3 move = new Vector3(moveHorizontal, moveVertical, 50.0f * (itt) * Time.deltaTime);
        rb.AddForce (move);

		if ((transform.position.z > mylist [maxPool - 1].transform.position.z) && (distance > maxVisibleDistance)) {
			//Debug.Log("sending first list");
			positionZ = this.transform.position.z;
			Regenerate (mylist);
		} else if ((transform.position.z > myOtherList [maxPool - 1].transform.position.z )&& (distance > maxVisibleDistance)) {
			//Debug.Log("sending other list");
			positionZ = this.transform.position.z;
			Regenerate(myOtherList);
		}

		Score.text = "score:" + Count.ToString ();
		Camera.main.transform.position =new Vector3 (transform.position.x,transform.position.y+3.0f,transform.position.z-distanceCamera);
	}

	void OnTriggerEnter(Collider other) {
        if (!timer.flowContinue)
            return;
        ParticleSystem Prend;
		//Debug.Log ("on trigger");
		if (other.tag == "Danger") {
			Count = Count - 5;
            if (Count < 0)
                Count = 0;
			//Debug.Log("in danger");
		}
		else{
			Count = Count + 10;
		}
		Prend=other.GetComponent<ParticleSystem>();
		Prend.Play();

	}


	void Regenerate(List<GameObject>myList)
	{
        
        string thePositionBoundString = string.Empty;
		for (int i=0; i < maxPool; i++) {
            objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), myList[i].transform.position.z + theHalfGameobjectDistanceValue);
            thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
            //Debug.Log("thePositionBoundString --" + thePositionBoundString);
            while (positionQueue.Contains(thePositionBoundString))
            {
                objPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), myList[i].transform.position.z + theHalfGameobjectDistanceValue);
                thePositionBoundString = objPosition.x.ToString() + ";" + objPosition.y.ToString();
            }
            //Location = Location + 10.0f;
            positionQueue.Enqueue(thePositionBoundString);
            positionQueue.Dequeue();
            myList[i].transform.position = objPosition; // new Vector3(Random.Range (-2.0f, 2.0f),Random.Range (-2.0f, 2.0f),myList[i].transform.position.z + 200.0f);
        }
	}

}
