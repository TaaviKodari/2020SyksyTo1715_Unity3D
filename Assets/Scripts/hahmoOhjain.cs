using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hahmoOhjain : MonoBehaviour
{
    public float juoksuNopeus = 3;
    public float hiirenNopeus = 3;
    [SerializeField]
    private float hyppyVoima = 100f;
    public float painovoima = 10;
    [Header("hiiren asetukset")]
    public float horisontaalinenPyorinta = 0;
    public float vertikaalinenPyorinta = 0;
    public float maxKaannosAsteet = 60;
    public CursorLockMode haluttuMoodi;

    private Vector3 liikesuunta = Vector3.zero;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = haluttuMoodi;

        Cursor.visible = (CursorLockMode.Locked != haluttuMoodi);
    }

    // Update is called once per frame
    void Update(){
        horisontaalinenPyorinta += Input.GetAxis("Mouse X")  * hiirenNopeus;
        vertikaalinenPyorinta -= Input.GetAxis("Mouse Y") * hiirenNopeus;
        //Debug.Log($"asteet {horisontaalinenPyorinta}");
        vertikaalinenPyorinta = Mathf.Clamp(vertikaalinenPyorinta,-maxKaannosAsteet, maxKaannosAsteet);
        transform.localRotation = Quaternion.Euler(vertikaalinenPyorinta,horisontaalinenPyorinta,0);

        float nopeusEteen =  Input.GetAxis("Vertical");
        float nopeusSivulle = Input.GetAxis("Horizontal");
        Vector3 nopeus = new Vector3(nopeusSivulle, 0,nopeusEteen);

        nopeus = transform.rotation * nopeus;
        controller.SimpleMove(nopeus * juoksuNopeus);

        liikesuunta.y -= painovoima * Time.deltaTime;
        controller.Move(liikesuunta* Time.deltaTime);

        if(controller.isGrounded == true && Input.GetButtonDown("Jump")){
            //tee hyppy
            liikesuunta.y += hyppyVoima;
        }
    }

    void OnTriggerEnter(Collider tormattyMalli){
        if(tormattyMalli.tag == "Coin"){
            print("hurraa, kerättiin kolikko");
            Destroy(tormattyMalli.gameObject);
        }
    }
}
