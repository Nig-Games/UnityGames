using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

    private bool isDead = false;
    private Rigidbody2D rb2d; //se refiere al RigidBody2D al que este script esta adjunto.
                              //En este caso, se anadira a Bird y entonces, cogera el component rigidbody2d de bird
    private Animator anim;

    public float upForce = 200f;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isDead)
        {
            if (Input.GetMouseButtonDown(0)) //boton izquierdo del raton
            {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0, upForce));
                anim.SetTrigger("Flap");
            }
        }
	}

    //Unity siempre va a estar "escuchando" esta funcion, asi que cuando "se produzca una colision", esta funcion se va a ejecutar.
    //Si se ejecuta esta funcion, Update() evaluara la condicion, y el usuario no podra volar el pajaro mas.
    void OnCollisionEnter2D() //Esto es una funcion de Unity API
    {
        rb2d.velocity = Vector2.zero;
        isDead = true;
        anim.SetTrigger("Die");
        GameController.instance.BirdDied();
    }


}
