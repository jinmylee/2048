using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float pgThreshold = 0.5f;
    public float easing = 0.5f;
    Vector3 PageLocation;
    RectTransform rect;
    private int currentPg = 1;
    

    void Start()
    {
   
        PageLocation = transform.position;
    }
    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = PageLocation - new Vector3(difference, 0, 0);
    }
    public void OnEndDrag(PointerEventData data)
    {
        //PageLocation = gameObject.transform.position; //스와이프 이동 
        float pageper = (data.pressPosition.x - data.position.x);
        
        
        ///Screen.width;
        if (Mathf.Abs(pageper) >= pgThreshold)
        {
            Vector3 NewLocation = PageLocation;
            if (pageper > 0)
            {
                NewLocation += new Vector3(-280, 0, 0);
                ///NewLocation += new Vector3(-Screen.width, 0, 0);               
            }
            else if (pageper < 0)
            {

                NewLocation += new Vector3(280, 0, 0);
                //NewLocation += new Vector3(Screen.width, 0, 0);

            }
            //transform.position = NewLocation;
            //transform.position = new Vector3(NewLocation.x -280, rect.anchoredPosition.y, 0);

           StartCoroutine(SmoothMove(transform.position, NewLocation, easing));

            

            //PageLocation = NewLocation;
            PageLocation.x = NewLocation.x;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, PageLocation, easing));



            //transform.position = PageLocation;
            transform.position = new Vector3(PageLocation.x , 0, 0);
        }
        }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}


