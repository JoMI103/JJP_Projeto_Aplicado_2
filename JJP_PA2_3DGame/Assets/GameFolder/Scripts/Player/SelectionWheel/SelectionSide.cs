using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionSide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] ItemSO[] items;
    int currentItemId;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerHand hand;

    private Vector3 startScale;

    private void Awake()
    {
        currentItemId = 0;
        startScale = transform.localScale;
       
    }

    private void Start()
    {
        inputManager.changeActionMaps.SelectionWheel.canceled += ctx => deSelect(0);
    }

    private void Update()
    {
        if (!isOver) return;
        ProcessScroll(inputManager.selectionWheel.ScrollDown.ReadValue<float>());
        hand.changeItem = items[currentItemId];
        //Update handSystem items[currentItemId]

    }


    public void ProcessScroll(float scroll)
    {
       if (scroll == 0) return;

        if (scroll > 0)
            upDownCurrentItem(1);
        if (scroll < 0)
            upDownCurrentItem(-1);

        updateUISlot();
    }

    private void upDownCurrentItem(int num) {
        if (items.Length == 1) return;

        currentItemId += num;
        if (currentItemId < 0) currentItemId = items.Length - 1;
        if (currentItemId >= items.Length) currentItemId = 0;

    }

    [SerializeField] private Image toolImage;

    private void updateUISlot()
    {
        toolImage.sprite = items[currentItemId].image;
    }

    public bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, startScale * 1.2f, 0.2f);

        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deSelect(0.2f);
    }


    private void deSelect(float speed)
    {
        LeanTween.scale(this.gameObject, startScale, speed);
        isOver = false;
    }
}
