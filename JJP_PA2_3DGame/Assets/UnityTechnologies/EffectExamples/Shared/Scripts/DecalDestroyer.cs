using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public float lifeTime = 5.0f;
	public bool addRandom;
	public float QuantityRandom;

	private IEnumerator Start()
	{
		if(addRandom) yield return new WaitForSeconds(lifeTime + Random.Range(-QuantityRandom , QuantityRandom));
		else yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
