using UnityEngine;
using System.Collections;

public class Columns : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bird>() != null)
        {
            GameController.instance.BirdScored();
        }
    }
}
