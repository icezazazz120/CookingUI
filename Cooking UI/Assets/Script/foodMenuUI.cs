using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class foodMenuUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform gridContainer;
    public GameObject menuItemPrefab;
    public Button buttonPrev;
    public Button buttonNext;

    [Header("Page Indicator")]
    public Transform pageIndicatorContainer;
    public GameObject donutPrefab;   // วงกลมโดนัท
    public RectTransform activeDot;  // วงกลมทึบ (เพียงอันเดียว)

    [Header("Config")]
    public int itemsPerPage = 4;
    public int maxItems = 30;

    private int currentPage = 0;
    private int totalPages;
    private List<string> foodNames = new List<string>();
    private List<RectTransform> donutList = new List<RectTransform>();

    void Start()
    {
        // data ตัวอย่าง
        for (int i = 1; i <= maxItems; i++)
        {
            foodNames.Add("อาหาร " + i);
        }

        totalPages = Mathf.CeilToInt((float)foodNames.Count / itemsPerPage);

        createPageIndicators();
        showPage(0);

        buttonPrev.onClick.AddListener(() => changePage(-1));
        buttonNext.onClick.AddListener(() => changePage(1));
    }

    void createPageIndicators()
    {
        //foreach (Transform child in pageIndicatorContainer)
        //    Destroy(child.gameObject);
        if (donutList.Count > 0) return;

        donutList.Clear();

        for (int i = 0; i < totalPages; i++)
        {
            GameObject donut = Instantiate(donutPrefab, pageIndicatorContainer);
            donutList.Add(donut.GetComponent<RectTransform>());
        }

        Canvas.ForceUpdateCanvases();

        // ย้าย activeDot มาที่ index 0
        if (donutList.Count > 0)
        {
            activeDot.SetParent(donutList[0].parent);
            activeDot.position = donutList[0].position;
        }
    }

    void showPage(int page)
    {
        currentPage = Mathf.Clamp(page, 0, totalPages - 1);

        // ลบเมนูเก่า
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        // แสดงเมนูใหม่
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, foodNames.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject item = Instantiate(menuItemPrefab, gridContainer);
            item.GetComponentInChildren<Text>().text = foodNames[i];
        }

        updateIndicators();
    }

    void changePage(int direction)
    {
        int newPage = currentPage + direction;
        if (newPage >= 0 && newPage < totalPages)
        {
            showPage(newPage);
        }
    }

    void updateIndicators()
    {
        if (currentPage < donutList.Count && donutList[currentPage] != null)
        {
            activeDot.position = donutList[currentPage].position;
        }
    }

    [System.Serializable]
    public class FoodItem
    {
        public string name;      // ชื่ออาหาร
        public Sprite icon;      // รูปภาพ
        public int stars;        // จำนวนดาว

    }
}
