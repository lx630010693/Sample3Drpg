using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour
{

    //private PackageItem dragItem;
    private Package package;
    private Package endPackage;

    private Vector2 resultPos;//一般用于拿来将鼠标的位置转换为对应的UI位置来移动物体
    
    private bool canPut;


    private void Awake()
    {
        package = this.transform.parent.GetComponent<Package>();
    }

    public void OnDraging(PackageItem packageItem)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(package.transform.parent.transform as RectTransform, Input.mousePosition, null, out resultPos);
        //拖动时将鼠标的位置转换为对应的UIRect本地位置,拖动时物体的父对象不再是DragPanel而是Canvas，所以以Canvas为参考Rect

        resultPos -= new Vector2(packageItem.thisRect.rect.width / 2, packageItem.thisRect.rect.height / 2);//由于物体的锚点设置在左下角，会有一个偏移
        
        packageItem.transform.localPosition = resultPos;
    }

    public void OnBeginDraging(PackageItem packageItem)
    {
        packageItem.package.itemList.Remove(packageItem.itemData);
        packageItem.transform.SetParent(package.transform.parent);//物体被开始拖拽时，将其父类改为Canvas，以免被其他层的物体遮挡住
        
        Color color = packageItem.GetComponent<Image>().color;
        color.a = 0.7f;
        packageItem.GetComponent<Image>().color = color;//视觉效果
        
        packageItem.GetComponent<Image>().raycastTarget = false;
        //比较关键的一步，将物体上的射线检测关闭掉，因为这个会影响
        //鼠标进入另外的背包界面时的进入事件检测的判断，如果不关会导致检测不到。
        
        //当物体被拖起后，根据他的长与宽把所占的格子的isUsed给改为false(格子不再被占用)
        for (int i = 0; i < packageItem.itemData.length; i++)
        {
            for (int k = 0; k < packageItem.itemData.width; k++)
            {
                packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed = false;
            }
        }
    }

    public void OnEndDraging(PackageItem packageItem)
    {
        Color color = packageItem.GetComponent<Image>().color;
        color.a = 1f;
        packageItem.GetComponent<Image>().color = color;//视觉效果

        endPackage = PackageManager.Instance.FindUsingPackage();//当拖拽结束后，判断当前落在哪个背包里(背包进入事件)

        if (endPackage == null)//如果不在背包区域那就返回原地
        {
            packageItem.transform.SetParent(packageItem.package.dragPanel, true);
            packageItem.transform.position= package.grids.GetWorldPosition(packageItem.itemData.originPos.x,packageItem.itemData.originPos.y);
            packageItem.GetComponent<Image>().raycastTarget = true;
            
            for (int i = 0; i < packageItem.itemData.length; i++)
            {
                for (int k = 0; k < packageItem.itemData.width; k++)
                {
                    packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed = true;
                }
            }
            packageItem.package.itemList.Add(packageItem.itemData);
            return;
        }

      

        packageItem.transform.SetParent(endPackage.dragPanel, true);//将物体放置在目的背包的DragPanel内
        
        
        Vector2Int lastOrigin = packageItem.itemData.originPos;
        Package lastPackage = packageItem.package;//这两句是记录放置之前所处的背包以及位置，后面位置不合法可能会放回去
        
        packageItem.package = endPackage;
        packageItem.dragPanel = packageItem.transform.parent.GetComponent<DragPanel>();
        canPut= packageItem.TryRefreshOriginPos();//更新物体所处的背包以及相关信息,如果位置不合法那就不让放

        for (int i = 0; i < packageItem.itemData.length; i++)
        {
            for (int k = 0; k < packageItem.itemData.width; k++)
            {
                if (packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed == true)
                {
                    canPut = false;//判断物体新处于的背包,从当前原点根据物体的长宽,判断有没有格子被占用。
                    break;     //有就跳出循环不再判断
                }
            }
        }
        if (!canPut)//如果不能放置就返回之前记录的原点与背包
        {
            packageItem.package = lastPackage;
            packageItem.itemData.originPos = lastOrigin;
            packageItem.transform.SetParent(packageItem.package.dragPanel,true);
            packageItem.transform.position = package.grids.GetWorldPosition(packageItem.itemData.originPos.x, packageItem.itemData.originPos.y);
            packageItem.GetComponent<Image>().raycastTarget = true;
            for (int i = 0; i < packageItem.itemData.length; i++)
            {
                for (int k = 0; k < packageItem.itemData.width; k++)
                {
                    packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed = true;
                }
            }
            packageItem.package.itemList.Add(packageItem.itemData);
            canPut = true;//重置一下canPut
            return;
        }

        
        if (canPut)
        {
            packageItem.transform.position = packageItem.package.grids.GetWorldPosition(packageItem.itemData.originPos.x, packageItem.itemData.originPos.y);

            for (int i = 0; i < packageItem.itemData.length; i++)
            {
                for (int k = 0; k < packageItem.itemData.width; k++)
                {
                    packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed = true;
                }
            }
            packageItem.package.itemList.Add(packageItem.itemData);
        }
        packageItem.GetComponent<Image>().raycastTarget = true;//可以将射线检测开启了，方便下一次拖拽

    }
  

}
