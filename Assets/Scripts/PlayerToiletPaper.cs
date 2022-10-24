using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToiletPaper : MonoBehaviour
{
    public GameObject toiletPaper;
    public float toiletPaperSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePos = Input.mousePosition;
            GameObject thrownToiletPaper = Instantiate<GameObject>(toiletPaper, gameObject.transform);
            thrownToiletPaper.GetComponent<Rigidbody2D>().velocity = new Vector2(mousePos.x * toiletPaperSpeed, mousePos.y * toiletPaperSpeed);
        }
    }
}
