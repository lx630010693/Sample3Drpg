using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BoChi;
public class Package : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GridList grids;
    public int length;
    public int width;
    public int gridSize;

    public bool isUsing = false;

    public RectTransform dragPanel;

    public List<PackageItemData> itemList = new List<PackageItemData>();
    private List<PackageItemData> tempItemList = new List<PackageItemData>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        isUsing = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUsing = false;
    }

    private void Awake()
    {
        grids = new GridList(length, width, gridSize, this.transform.position);
        dragPanel = transform.Find("DragPanel").transform as RectTransform;
        PackageManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        PackageManager.Instance.Remove(this);
    }

    public void Save(string fileName)
    {
        JsonManager.Instance.SaveData(itemList,fileName);
    }
    public void Load(string fileName)
    {
        tempItemList= JsonManager.Instance.LoadData<List<PackageItemData>>(fileName);
        for (int i = 0; i < tempItemList.Count; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("PackageItem/" + tempItemList[i].Name));
            obj.transform.SetParent(dragPanel.transform, false);
            PackageItem objItem = obj.GetComponent<PackageItem>();
            objItem.itemData = tempItemList[i];
            //obj.transform.position = grids.GetWorldPosition(objItem.itemData.originPos.x, objItem.itemData.originPos.y);

        }
    }
}
