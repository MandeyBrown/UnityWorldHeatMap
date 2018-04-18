using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class GameManager : Singleton<GameManager> {

	[HideInInspector]
	public bool locationServicesIsRunning = false;

	public GameObject mainMap;
	public GameObject newMap;

	public GameObject player;
	public GeoPoint playerGeoPosition;
	public PlayerLocationService player_loc;
	public Transform cam;
	float max;
	float min;
	public Vector2 redRange, greenRange, blueRange, alphaRange;

	public GameObject drone;

	public GameObject drone_1;
	public GameObject drone_2;
	public GameObject drone_3;
	public GameObject drone_4;

	private bool gDrawn = false;
	public string date = "2018-04-14 02:30 PM";
	public string type = "historical";

	public enum PlayerStatus { TiedToDevice, FreeFromDevice }

	private PlayerStatus _playerStatus;
	public PlayerStatus playerStatus
	{
		get { return _playerStatus; }
		set { _playerStatus = value; }
	}

	void Awake (){

		Time.timeScale = 1;
		playerStatus = PlayerStatus.TiedToDevice;

		player_loc = player.GetComponent<PlayerLocationService>();
		newMap.GetComponent<MeshRenderer>().enabled = false;
		newMap.SetActive (false);

	}

	public GoogleStaticMap getMainMapMap () {
		return mainMap.GetComponent<GoogleStaticMap> ();
	}

	public GoogleStaticMap getNewMapMap () {
		return newMap.GetComponent<GoogleStaticMap> ();
	}

	IEnumerator Start () {

		getMainMapMap ().initialize ();
		yield return StartCoroutine (player_loc._StartLocationService ());
		StartCoroutine (player_loc.RunLocationService ());

		locationServicesIsRunning = player_loc.locServiceIsRunning;
		Debug.Log ("Player loc from GameManager: " + player_loc.loc);
		getMainMapMap ().centerMercator = getMainMapMap ().tileCenterMercator (player_loc.loc);
		getMainMapMap ().DrawMap ();

		mainMap.transform.localScale = Vector3.Scale (
			new Vector3 (getMainMapMap ().mapRectangle.getWidthMeters (), getMainMapMap ().mapRectangle.getHeightMeters (), 1.0f),
			new Vector3 (getMainMapMap ().realWorldtoUnityWorldScale.x, getMainMapMap ().realWorldtoUnityWorldScale.y, 1.0f));

		player.GetComponent<ObjectPosition> ().setPositionOnMap (player_loc.loc);

		// Setting positions on cubes
		GameObject[] objectsOnMap = GameObject.FindGameObjectsWithTag ("ObjectOnMap");

		foreach (GameObject obj in objectsOnMap) {
			obj.GetComponent<ObjectPosition> ().setPositionOnMap1 ();
		}
    }


	void reInit() {
		locationServicesIsRunning = player_loc.locServiceIsRunning;
		Debug.Log ("Player loc from GameManager: " + player_loc.loc);
		getMainMapMap ().centerMercator = getMainMapMap ().tileCenterMercator (player_loc.loc);
		getMainMapMap ().DrawMap ();

		mainMap.transform.localScale = Vector3.Scale (
			new Vector3 (getMainMapMap ().mapRectangle.getWidthMeters (), getMainMapMap ().mapRectangle.getHeightMeters (), 1.0f),
			new Vector3 (getMainMapMap ().realWorldtoUnityWorldScale.x, getMainMapMap ().realWorldtoUnityWorldScale.y, 1.0f));

		player.GetComponent<ObjectPosition> ().setPositionOnMap (player_loc.loc);

		// Setting positions on cubes
		GameObject[] objectsOnMap = GameObject.FindGameObjectsWithTag ("ObjectOnMap");

		foreach (GameObject obj in objectsOnMap) {
			obj.GetComponent<ObjectPosition> ().setPositionOnMap1 ();
		}
	}

    void Update () {
		
		if (Input.GetKeyDown (KeyCode.Q)) {
			GeoPoint nwp = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.NW);
			GeoPoint sep = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.SE);
			GeoPoint nep = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.NE);
			GeoPoint swp = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.SW);


			string nwG = NewGeohash.Encode (nwp.lat_d,nwp.lon_d,7);
			string neG = NewGeohash.Encode (nep.lat_d,nep.lon_d,7);
			string seG = NewGeohash.Encode (sep.lat_d,sep.lon_d,7);
			string swG = NewGeohash.Encode (swp.lat_d,swp.lon_d,7);
			string geohashes = NewGeohash.getInternalGeohashesAsString (nwG, neG, swG, seG);


			Debug.Log ("NW " +nwG+" "+ nwp);
			Debug.Log ("SE " +seG+" "+ sep);
			Debug.Log ("INTERNAL GEOHASHES " +geohashes);
			Debug.Log ("INTERNAL GEOHASHES LENGTH " +geohashes.Length);


		}


		/* MY CHANGES*/
		if (Input.GetKeyDown (KeyCode.M)) {

			string geohashes = "\"dr5rsmu\",\"dr5rsmv\"";
			string rsp = NewGeohash.queryJSON (geohashes, date, type);
			Debug.Log (rsp);
		}
		if (Input.GetKeyDown (KeyCode.N)) {

			GameObject[] others = GameObject.FindGameObjectsWithTag("Respawn");
			foreach (GameObject other in others)
			{
				Debug.Log (other);
				if (!other.name.Equals ("ghash")) {
					Destroy (other);
				}
			}
			gDrawn = false;


		}
		/* MY CHANGES*/
				



		// NEW UPDATED
		//if (Input.GetKeyDown (KeyCode.X)) {
		if (Input.GetButton ("Submit")) {
			if (!gDrawn) {
				int geohashLength = 7;

				if (getMainMapMap ().zoom <= 14) {
					geohashLength = 5;
				}

				Debug.Log ("GL: " + geohashLength);

				GeoPoint nwp = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.NW);
				GeoPoint sep = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.SE);
				GeoPoint nep = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.NE);
				GeoPoint swp = getMainMapMap ().mapRectangle.getCornerLatLon (GoogleStaticMap.MapRectangle.GetCorner.SW);


				string nwG = NewGeohash.Encode (nwp.lat_d, nwp.lon_d, geohashLength);
				string neG = NewGeohash.Encode (nep.lat_d, nep.lon_d, geohashLength);
				string seG = NewGeohash.Encode (sep.lat_d, sep.lon_d, geohashLength);
				string swG = NewGeohash.Encode (swp.lat_d, swp.lon_d, geohashLength);
				string geohashes = NewGeohash.getInternalGeohashesAsString (nwG, neG, swG, seG);


				Debug.Log ("NW " + nwG + " " + nwp);
				Debug.Log ("SE " + seG + " " + sep);
				Debug.Log ("INTERNAL GEOHASHES " + geohashes);
				Debug.Log ("INTERNAL GEOHASHES LENGTH " + geohashes.Length);

				string rsp = NewGeohash.queryJSON (geohashes, date, type);

				if (!String.IsNullOrEmpty (rsp)) {
					Dictionary<string, float> gmap = new Dictionary<string, float> ();
					Debug.Log ("RSP: " + rsp);
					NewGeohash.parseJsonRsp (rsp, gmap);
					Debug.Log ("RSP SIZE: " + gmap.Count);

					int count = 0;
					foreach (string sss in gmap.Keys) {
						
						//Debug.Log (sss + " " + gmap [sss]);
						GameObject drone1 = drone;
						double[] pts = NewGeohash.Decode (sss);

						float lat1 = (float)pts [0];
						float lat2 = (float)pts [1];
						float lon1 = (float)pts [2];
						float lon2 = (float)pts [3];
						//Debug.Log (lat1 + " " + lat2 + " " + lon1 + " " + lon2);

						Vector2 nw = GameManager.Instance.getMainMapMap ().getPositionOnMap (new GeoPoint (lat2, lon1));
						Vector2 ne = GameManager.Instance.getMainMapMap ().getPositionOnMap (new GeoPoint (lat2, lon2));
						Vector2 sw = GameManager.Instance.getMainMapMap ().getPositionOnMap (new GeoPoint (lat1, lon1));
						Vector2 se = GameManager.Instance.getMainMapMap ().getPositionOnMap (new GeoPoint (lat1, lon2));


						float length = nw.x - ne.x;
						float height = nw.y - sw.y;

						//Debug.Log ("LN:" + length);

						//transform.position = new Vector3 (tempPosition.x, transform.position.y, tempPosition.y);
						drone1.transform.localScale = new Vector3 (length, 0.3f, height);

						Debug.Log ("GM====================:"+gmap [sss]);
						//drone.GetComponent<Renderer> ().sharedMaterial.color = Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f);

						//++++++++++++++++++++++++++++++++++++++++++++++Fawad +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
						Color lerpedColor = Color.Lerp(Color.green, Color.red, Mathf.PingPong(Time.time, 1));
						lerpedColor.a = 0.3f;
						if (gmap [sss] <= 0) {
							//drone_1.GetComponent<Renderer> ().sharedMaterial.color = new Color (34, 255, 63, 0.3f);
							//drone1.GetComponent<Renderer> ().sharedMaterial.color = lerpedColor;
							//drone.GetComponent<Renderer> ().sharedMaterial.color = new Color (1f, 0.92f, 0.016f, 0.4f);
							//var go = Instantiate (drone_1, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
							/*GameObject newItem = UnityEngine.Object.Instantiate (drone_1)as GameObject;
							newItem.GetComponent<Renderer> ().sharedMaterial.color = new Color (1f, 0.92f, 0.016f, 0.4f);
							newItem.transform.position = new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2));*/

							//var go1 = UnityEngine.Object.Instantiate (Resources.Load("drone_1"))as GameObject;
							var go = Instantiate (drone_2, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
							go.transform.localScale = new Vector3 (length, 0.3f, height);
							go.GetComponent<Renderer> ().sharedMaterial.color = new Color (0.0f, 1f, 0f, 0.4f);
						}

						if (gmap [sss] > 0 && gmap [sss] < 5) {
							Debug.Log ("GGGGGGGGGGGGGGGGG");
							//drone_2.GetComponent<Renderer> ().sharedMaterial.color = new Color (11, 114, 3, 0.3f);
							//var go1 = Instantiate (drone_2, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);


							/*GameObject newItem = UnityEngine.Object.Instantiate (drone)as GameObject;
							newItem.GetComponent<Renderer> ().sharedMaterial.color = new Color (1f, 1f, 1f, 0.4f);
							newItem.transform.position = new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2));*/

							//drone.GetComponent<Renderer> ().sharedMaterial.color = new Color (0.4f, 0.3f, 0.2f, 0.4f);
							//var go = Instantiate (drone_2, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);


							var go = Instantiate (drone_1, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
							go.transform.localScale = new Vector3 (length, 0.3f, height);
							go.GetComponent<Renderer> ().sharedMaterial.color = new Color (1f, 0.92f, 0.016f, 0.4f);
						}

						if (gmap [sss] >= 5 && gmap [sss] < 10) {
							drone1.GetComponent<Renderer> ().sharedMaterial.color = new Color (Random.value, Random.value, Random.value, 0.3f);
							//drone.GetComponent<Renderer> ().sharedMaterial.color = new Color (0.4f, 0.4f, 0.2f, 0.4f);
							var go = Instantiate (drone_3, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
							go.transform.localScale = new Vector3 (length, 0.3f, height);
							go.GetComponent<Renderer> ().sharedMaterial.color = new Color (0.0f, 1f, 0f, 0.4f);
						}

						if (gmap [sss] >= 5 && gmap [sss] < 10) {
							drone1.GetComponent<Renderer> ().sharedMaterial.color = new Color (Random.value, Random.value, Random.value, 0.3f);
							//drone.GetComponent<Renderer> ().sharedMaterial.color = new Color (1f, 0f, 0f, 0.4f);
							var go = Instantiate (drone_4, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
							go.transform.localScale = new Vector3 (length, 0.3f, height);
							go.GetComponent<Renderer> ().sharedMaterial.color = new Color (0.0f, 1f, 0f, 0.4f);
						}


						//drone.GetComponent<Renderer> ().material.color.a = 0.5f;
						//var myNewObject = Instantiate (drone1, new Vector3 (nw.x - (length / 2), 0, nw.y - (height / 2)), Quaternion.identity);
						//myNewObject.GetComponent<MeshRenderer>().material.color = lerpedColor;
						//myNewObject.transform.name = "Head";
						//Instantiate (drone, new Vector3 (ne.x, 3F, ne.y), Quaternion.identity);
						//Instantiate (drone, new Vector3 (sw.x, 3F, sw.y), Quaternion.identity);
						//Instantiate (drone, new Vector3 (se.x, 3F, se.y), Quaternion.identity);
						//Debug.Log (mp);
					}

				} else {
					Debug.Log ("SERVER RETURNED NO RECORDS");
				}
				gDrawn = true;
			}

		}

		// THIS HAS BEEN MERGED WITH THE PREVIOUS IF
		if (Input.GetKeyDown (KeyCode.G)) {
			
			Debug.Log ("GETTING SERVER DATA...");
			string rsp = NewGeohash.queryJSON ("");

			if (!String.IsNullOrEmpty (rsp)) {
				Dictionary<string, float> gmap = new Dictionary<string, float> ();

				NewGeohash.parseJsonRsp (rsp, gmap);
				Debug.Log (rsp);
				foreach (string sss in gmap.Keys) {
					Debug.Log (sss);
				}
			} else {
				Debug.Log ("SERVER RETURNED NO RECORDS");
			}
		}


		if (Input.GetKeyDown (KeyCode.Y)) {
			Debug.Log ("SCALE: "+getMainMapMap ().realWorldtoUnityWorldScale);
		}













		if (Input.GetButtonDown ("Fire1")) {
			// DownScroll

			float val = Input.GetAxis ("Fire1");
			//Debug.Log("PEYECHI " + val);
			if (val < 0) {
				if (getMainMapMap ().zoom >= 5) {
					int num = getMainMapMap ().zoom;

					getMainMapMap ().zoom = num - 2;
					getMainMapMap ().initialize ();
					getMainMapMap ().DrawMap ();

					Debug.Log ("ZOOM: " + getMainMapMap ().zoom);

				}
			} else if (val > 0) {

				if (getMainMapMap ().zoom < 15) {
					Debug.Log ("ZOOM2: "+getMainMapMap ().zoom);
					if (getMainMapMap ().zoom == 14) {
						//Debug.Log ("DID THIS");
						getMainMapMap ().zoom = 15;
					} else {
						int num = getMainMapMap ().zoom;
						getMainMapMap ().zoom = num + 2;
					}
					getMainMapMap ().initialize ();
					getMainMapMap ().DrawMap ();
				}
			}


		}












		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			// DownScroll
			if (getMainMapMap ().zoom >= 5) {
				int num = getMainMapMap ().zoom;

				getMainMapMap ().zoom = num - 2;
				getMainMapMap ().initialize ();
				getMainMapMap ().DrawMap ();

				Debug.Log ("ZOOM: " + getMainMapMap ().zoom);

			}


		} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			// UpScroll
			if (getMainMapMap ().zoom < 15) {
				Debug.Log ("ZOOM2: "+getMainMapMap ().zoom);
				if (getMainMapMap ().zoom == 14) {
					//Debug.Log ("DID THIS");
					getMainMapMap ().zoom = 15;
				} else {
					int num = getMainMapMap ().zoom;
					getMainMapMap ().zoom = num + 2;
				}
				getMainMapMap ().initialize ();
				getMainMapMap ().DrawMap ();
			}

			//getMainMapMap ().DrawMap ();
		}
		cam.LookAt (player.transform);


		if(!locationServicesIsRunning){

			//TODO: Show location service is not enabled error. 
			return;
		}

		// playerGeoPosition = getMainMapMap ().getPositionOnMap(new Vector2(player.transform.position.x, player.transform.position.z));
		playerGeoPosition = new GeoPoint();
		// GeoPoint playerGeoPosition = getMainMapMap ().getPositionOnMap(new Vector2(player.transform.position.x, player.transform.position.z));
		if (playerStatus == PlayerStatus.TiedToDevice) {
			playerGeoPosition = player_loc.loc;
			player.GetComponent<ObjectPosition> ().setPositionOnMap (playerGeoPosition);
		} else if (playerStatus == PlayerStatus.FreeFromDevice){
			playerGeoPosition = getMainMapMap ().getPositionOnMap(new Vector2(player.transform.position.x, player.transform.position.z));
		}

		// SWITCHING OF MAPS
		var tileCenterMercator = getMainMapMap ().tileCenterMercator (playerGeoPosition);

		if(!getMainMapMap ().centerMercator.isEqual(tileCenterMercator)) {

			newMap.SetActive(true);
			//getNewMapMap ().initialize ();
			getNewMapMap ().copyFrom (getMainMapMap ());
			getNewMapMap ().zoom = getMainMapMap ().zoom;
			getNewMapMap ().centerMercator = tileCenterMercator;

			getNewMapMap ().DrawMap ();

			/*getNewMapMap ().transform.localScale = Vector3.Scale(
				new Vector3 (getNewMapMap ().mapRectangle.getWidthMeters (), getNewMapMap ().mapRectangle.getHeightMeters (), 1.0f),
				new Vector3(getNewMapMap ().realWorldtoUnityWorldScale.x, getNewMapMap ().realWorldtoUnityWorldScale.y, 1.0f));	*/

			getNewMapMap ().transform.localScale = Vector3.Scale(
				new Vector3 (getMainMapMap ().mapRectangle.getWidthMeters (), getMainMapMap ().mapRectangle.getHeightMeters (), 1.0f),
				new Vector3(getMainMapMap ().realWorldtoUnityWorldScale.x, getMainMapMap ().realWorldtoUnityWorldScale.y, 1.0f));	

			// THIS DETERMINES THE POSITION OF THE GAMEOBJECT ON THE NEW MAP
			Vector2 tempPosition = GameManager.Instance.getMainMapMap ().getPositionOnMap (getNewMapMap ().centerLatLon);
			newMap.transform.position = new Vector3 (tempPosition.x, 0, tempPosition.y);

			GameObject temp = newMap;
			newMap = mainMap;
			mainMap = temp;

		}

		/*if(!getMainMapMap ().centerMercator.isEqual(tileCenterMercator)) {

			newMap.SetActive(true);
			getNewMapMap ().initialize ();
			getNewMapMap ().zoom = getMainMapMap ().zoom;
			getNewMapMap ().centerMercator = tileCenterMercator;

			getNewMapMap ().DrawMap ();

			getNewMapMap ().transform.localScale = Vector3.Scale(
				new Vector3 (getNewMapMap ().mapRectangle.getWidthMeters (), getNewMapMap ().mapRectangle.getHeightMeters (), 1.0f),
				new Vector3(getNewMapMap ().realWorldtoUnityWorldScale.x, getNewMapMap ().realWorldtoUnityWorldScale.y, 1.0f));	


			Vector2 tempPosition = GameManager.Instance.getMainMapMap ().getPositionOnMap (getNewMapMap ().centerLatLon);
			newMap.transform.position = new Vector3 (tempPosition.x, 0, tempPosition.y);

			GameObject temp = newMap;
			newMap = mainMap;
			mainMap = temp;

		}*/


		if(getMainMapMap().isDrawn && mainMap.GetComponent<MeshRenderer>().enabled == false){
			mainMap.GetComponent<MeshRenderer>().enabled = true;
			newMap.GetComponent<MeshRenderer>().enabled = false;
			newMap.SetActive(false);
		}
	}

	public Vector3? ScreenPointToMapPosition(Vector2 point){
		var ray = Camera.main.ScreenPointToRay(point);
		//RaycastHit hit;
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0; 
		if (!hPlane.Raycast (ray, out distance)) {
			// get the hit point:
			return null;
		}
		Vector3 location = ray.GetPoint (distance);
		return location;
	}

}
