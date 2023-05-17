using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour {
	private int waveNumber;
	public float distanceX, distanceZ;
	public float[] waveAmplitude;
	public float magnitudeDivider;
	public Vector2[] impactPos;
	public float[] distance;
	public float speedWaveSpread;
	

	Mesh mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<8; i++){ // Change for loop bound with more balls, depends on WaterRippleShader

			waveAmplitude[i] = GetComponent<Renderer>().material.GetFloat("_WaveAmplitude" + (i+1));
			// print("Float" + GetComponent<Renderer>().material.GetFloat("_WaveAmplitude1"));
			if (waveAmplitude[i] > 0)
			{
				distance[i] += speedWaveSpread;
				GetComponent<Renderer>().material.SetFloat("_Distance" + (i+1), distance[i]);
				GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + (i+1), waveAmplitude[i] * 0.98f); // Decreases the wave amplitude by 2% every time it updates so it keeps getting smaller
			}
			if (waveAmplitude[i] < 0.01) // If the wave is pretty small, just set it to 0
			{
				GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + (i+1), 0);
				distance[i] = 0;
			}

		}
	}

	void OnCollisionEnter(Collision col){
		if (col.rigidbody)
		{
			waveNumber++;
			if (waveNumber == 9){ // One more than the max number of waves you want to allow, we have 8 variables in WaterRippleShader.shader, so we do 9
				waveNumber = 1;
			}
			waveAmplitude[waveNumber-1] = 0;
			distance[waveNumber-1] = 0;
			// Distance in the realspace of the object that hits the plane
			distanceX = this.transform.position.x - col.gameObject.transform.position.x;
						//distance of this object   something?? not exactly sure what this does
			distanceZ = this.transform.position.z - col.gameObject.transform.position.z;
			
			GetComponent<Renderer>().material.SetFloat("_OffsetX" + waveNumber, distanceX / mesh.bounds.size.x * 2.5f);
			GetComponent<Renderer>().material.SetFloat("_OffsetZ" + waveNumber, distanceZ / mesh.bounds.size.z * 2.5f);

			GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + waveNumber, col.rigidbody.velocity.magnitude * magnitudeDivider);

			impactPos[waveNumber - 1].x = col.transform.position.x;
			impactPos[waveNumber - 1].y = col.transform.position.z;

			GetComponent<Renderer>().material.SetFloat("_xImpact" + waveNumber, col.transform.position.x);
			GetComponent<Renderer>().material.SetFloat("_zImpact" + waveNumber, col.transform.position.z);

			GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + waveNumber, col.rigidbody.velocity.magnitude * magnitudeDivider);

		}
	}
}
