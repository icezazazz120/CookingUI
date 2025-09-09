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
    public GameObject donutPrefab;   // ǧ���ⴹѷ
    public RectTransform activeDot;  // �شǧ����ֺ

    [Header("Config")]
    public int itemsPerPage = 4;
    public int maxItems = 30;

    private int currentPage = 0;
    private int totalPages;

    public FoodDatabase foodDatabase;
    private List<FoodItem> foodItems;
    private List<RectTransform> donutList = new List<RectTransform>();

    [Header("Ingredients UI")]
    public Transform ingredientContainer;   // parent �ͧ�ѵ�شԺ
    public GameObject ingredientPrefab;     // prefab �ӹǹ�ͤ͹

    public Inventory inventory;

    void Start()
    {
        foodItems = foodDatabase.foods;

        totalPages = Mathf.CeilToInt((float)foodItems.Count / itemsPerPage);

        createPageIndicators();
        showPage(0);

        buttonPrev.onClick.AddListener(() => changePage(-1));
        buttonNext.onClick.AddListener(() => changePage(1));
    }

    void createPageIndicators()
    {
        if (donutList.Count > 0) return;

        donutList.Clear();

        for (int i = 0; i < totalPages; i++)
        {
            GameObject donut = Instantiate(donutPrefab, pageIndicatorContainer);
            donutList.Add(donut.GetComponent<RectTransform>());
        }

        Canvas.ForceUpdateCanvases();

        // ���� Dot �ҷ�� index 0
        if (donutList.Count > 0)
        {
            activeDot.SetParent(donutList[0].parent);
            activeDot.position = donutList[0].position;
        }
    }

    void showPage(int page)
    {
        currentPage = Mathf.Clamp(page, 0, totalPages - 1);

        // ź�������
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        // �ʴ���������
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, foodItems.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject item = Instantiate(menuItemPrefab, gridContainer);

            // �� index ����� closure bug
            int index = i;

            // --- �������� ---
            // ����
            var texts = item.GetComponentsInChildren<Text>();
            foreach (var t in texts)
            {
                if (t.name == "NameText") t.text = foodItems[index].name;
            }

            // �ٻ
            var images = item.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img.name == "Icon") img.sprite = foodItems[index].icon;
            }

            // ���
            Transform starsParent = item.transform.Find("Stars");
            if (starsParent != null)
            {
                for (int s = 0; s < starsParent.childCount; s++)
                {
                    starsParent.GetChild(s).gameObject.SetActive(s < foodItems[index].stars);
                }
            }

            // --- Event: ��ԡ���� > ����ѵ�شԺ ---
            Button btn = item.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    showIngredients(foodItems[index]);
                });
            }
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

    void showIngredients(FoodItem food)
    {
        // ź�ͧ���
        foreach (Transform child in ingredientContainer)
            Destroy(child.gameObject);

        // �ʴ��ͧ����
        foreach (var req in food.requirements)
        {
            GameObject ing = Instantiate(ingredientPrefab, ingredientContainer);

            var img = ing.transform.Find("Icon").GetComponent<Image>();
            var txt = ing.transform.Find("AmountText").GetComponent<Text>();

            img.sprite = req.ingredient.icon;

            int have = inventory.items.ContainsKey(req.ingredient.name) ? inventory.items[req.ingredient.name] : 0;
            txt.text = have + "/" + req.amount;

            // ��ᴧ�������
            if (have < req.amount)
                txt.color = Color.red;
            else
                txt.color = Color.green;
        }
    }

    public void Cook(FoodItem food)
    {
        //Debug.Log("�ѵ�شԺ����");
        foreach (var req in food.requirements)
        {
            if (!inventory.hasEnough(req.ingredient.name, req.amount))
            {
                Debug.Log("�ѵ�شԺ����");
                return;
            }
        }

        // �ѡ�ѵ�شԺ
        foreach (var req in food.requirements)
        {
            inventory.useItem(req.ingredient.name, req.amount);
        }

        Debug.Log("�� " + food.name + " �����!");
        showIngredients(food); // �Ѿവ�ӹǹ
    }
}
