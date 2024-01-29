using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PackageItemData
{
    public string Name;
    public Vector2Int originPos;
    public int length;
    public int width;
}

public class PackageItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform thisRect;
    public DragPanel dragPanel;
    public Package package;

    
    public PackageItemData itemData;

    /*public Vector2Int originPos;
    public int length;
    public int width;*/

    private void Awake()
    {
        itemData.Name = this.gameObject.name;
    }
    
    public bool TryRefreshOriginPos()
    {
        Vector2Int gridPos = package.grids.GetPosXY(this.transform.position);
        Vector2Int targetPos = new Vector2Int(gridPos.x,gridPos.y);
        if(targetPos.x < 0 || targetPos.y < 0||targetPos.x>package.grids.length||targetPos.y>package.grids.width)
        {
            return false;
        }
        itemData.originPos = targetPos;
        return true;
    }
    private void Start()
    {
        thisRect = this.GetComponent<RectTransform>();
        dragPanel = this.transform.parent.GetComponent<DragPanel>();
        package = dragPanel.transform.parent.GetComponent<Package>();

        package.itemList.Add(itemData);
        this.transform.position = package.grids.GetWorldPosition(itemData.originPos.x,itemData.originPos.y);
        for (int i = 0; i <itemData.length; i++)
        {
            for (int k = 0; k <itemData.width; k++)
            {
                package.grids.gridList[i + itemData.originPos.x, k + itemData.originPos.y].isUsed = true;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPanel.OnBeginDraging(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragPanel.OnDraging(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragPanel.OnEndDraging(this);
    }
}
