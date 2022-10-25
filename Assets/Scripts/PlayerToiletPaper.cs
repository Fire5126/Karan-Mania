using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToiletPaper : MonoBehaviour
{
    public GameObject toiletPaper;
    public float toiletPaperSpeed;
    private Camera mainCam;
    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - transform.position;
            GameObject thrownToiletPaper = Instantiate<GameObject>(toiletPaper, gameObject.transform);
            thrownToiletPaper.GetComponent<Rigidbody2D>().velocity = rotation.normalized * toiletPaperSpeed;
        }
    }
}
