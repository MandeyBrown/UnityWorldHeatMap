using UnityEngine;
using System.Collections;

public class ObjectPosition : MonoBehaviour {
	GoogleStaticMap mainMap;


	public float lat_d = 0.0f, lon_d = 0.0f;

	private GeoPoint pos;


	void Awake (){
		pos = new GeoPoint ();
		pos.setLatLon_deg (lat_d, lon_d);
	}

	public void setPositionOnMap () {
		Vector2 tempPosition = GameManager.Instance.getMainMapMap ().getPositionOnMap (this.pos);
		transform.position = new Vector3 (tempPosition.x, transform.position.y, tempPosition.y);
	}

	public void setPositionOnMap (GeoPoint pos) {
		this.pos = pos;
		setPositionOnMap ();
	}
	//RIKI
	public void setPositionOnMap1 () {
		
		pos = new GeoPoint ();
		float x = 40.576243f;
		float y = -105.080823f;
		//Random rnd = new Random ();
		float xadd = Random.value;
		float yadd = Random.value;
		pos.setLatLon_deg (x+xadd, y+yadd);
		this.pos = pos;

		setPositionOnMap ();
	}

}
