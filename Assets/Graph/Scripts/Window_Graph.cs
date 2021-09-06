

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random=UnityEngine.Random;


public class Window_Graph : MonoBehaviour {
    public List<float> valueList = new List<float>() { 5f };
    [SerializeField] private Sprite circleSprite;
    public Transform graphContainer2;
    private RectTransform graphContainer;
    private bool x;
    public float PercentMoved;
    public TMP_Text PercentMoved_text;
    public TMP_Text Amount_text;
    public TMP_Text PriceOfStock_text;
    public bool Bought;
    public float Amount;

        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }
    private void Awake() {
    graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
    PercentMoved = 0.0f;
    ShowGraph(valueList);
    x = true;    
    Bought = false; 
    Amount = 100.0f; 
    }


    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void ShowGraph(List<float> valueList) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 15f;
        float xSize = 75f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }
    public void ShowGraphHere()
    {
    foreach (Transform child in graphContainer2.transform) {
    GameObject.Destroy(child.gameObject);
    }
    print(valueList.Count);
    PercentMoved = Random.Range(-0.25f,0.25f);
    valueList.Add(valueList[valueList.Count-1]+(valueList[valueList.Count-1]*PercentMoved));
    ShowGraph(valueList);
    }
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }


    private void Update() {
    PercentMoved_text.text = "%"+(PercentMoved*100).ToString();
    Amount_text.text = "$"+Amount.ToString();
    PriceOfStock_text.text = valueList[valueList.Count-1].ToString();
    if(x==true)
    {
    StartCoroutine(waiter());    
    }
    }
IEnumerator waiter()
{
    x = false;
    yield return new WaitForSeconds(2);
    ShowGraphHere();
    if(Bought==true)
    {
        Amount = (Amount*PercentMoved)+Amount;
    }
    x = true;
}

}
