using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageZoomer : MonoBehaviour
{
    [SerializeField] private RectTransform image;

    private Touch touchOne, touchTwo;
    private Vector2 touchOneOrigin, touchTwoOrigin;

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            
            touchOne = Input.GetTouch(0);
            touchTwo = Input.GetTouch(1);
            
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                touchOneOrigin = touchOne.position;
                touchTwoOrigin = touchTwo.position;
                
                Debug.Log("ys");
            }

            Debug.Log(Vector2.Distance(touchOne.position, touchTwo.position) + ", " + Vector2.Distance(touchOneOrigin, touchTwoOrigin));
            
            if (Vector2.Distance(touchOne.position, touchTwo.position) >
                Vector2.Distance(touchOneOrigin, touchTwoOrigin))
            {
                image.localScale *= 1.025f;
            }

            else if (Vector2.Distance(touchOne.position, touchTwo.position) <
                Vector2.Distance(touchOneOrigin, touchTwoOrigin))
            {
                image.localScale /= 1.025f;
            }
        }
    }
}
