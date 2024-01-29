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

    private Vector2 resultPos;//һ����������������λ��ת��Ϊ��Ӧ��UIλ�����ƶ�����
    
    private bool canPut;


    private void Awake()
    {
        package = this.transform.parent.GetComponent<Package>();
    }

    public void OnDraging(PackageItem packageItem)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(package.transform.parent.transform as RectTransform, Input.mousePosition, null, out resultPos);
        //�϶�ʱ������λ��ת��Ϊ��Ӧ��UIRect����λ��,�϶�ʱ����ĸ���������DragPanel����Canvas��������CanvasΪ�ο�Rect

        resultPos -= new Vector2(packageItem.thisRect.rect.width / 2, packageItem.thisRect.rect.height / 2);//���������ê�����������½ǣ�����һ��ƫ��
        
        packageItem.transform.localPosition = resultPos;
    }

    public void OnBeginDraging(PackageItem packageItem)
    {
        packageItem.package.itemList.Remove(packageItem.itemData);
        packageItem.transform.SetParent(package.transform.parent);//���屻��ʼ��קʱ�����丸���ΪCanvas�����ⱻ������������ڵ�ס
        
        Color color = packageItem.GetComponent<Image>().color;
        color.a = 0.7f;
        packageItem.GetComponent<Image>().color = color;//�Ӿ�Ч��
        
        packageItem.GetComponent<Image>().raycastTarget = false;
        //�ȽϹؼ���һ�����������ϵ����߼��رյ�����Ϊ�����Ӱ��
        //����������ı�������ʱ�Ľ����¼������жϣ�������ػᵼ�¼�ⲻ����
        
        //�����屻����󣬸������ĳ�������ռ�ĸ��ӵ�isUsed����Ϊfalse(���Ӳ��ٱ�ռ��)
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
        packageItem.GetComponent<Image>().color = color;//�Ӿ�Ч��

        endPackage = PackageManager.Instance.FindUsingPackage();//����ק�������жϵ�ǰ�����ĸ�������(���������¼�)

        if (endPackage == null)//������ڱ��������Ǿͷ���ԭ��
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

      

        packageItem.transform.SetParent(endPackage.dragPanel, true);//�����������Ŀ�ı�����DragPanel��
        
        
        Vector2Int lastOrigin = packageItem.itemData.originPos;
        Package lastPackage = packageItem.package;//�������Ǽ�¼����֮ǰ�����ı����Լ�λ�ã�����λ�ò��Ϸ����ܻ�Ż�ȥ
        
        packageItem.package = endPackage;
        packageItem.dragPanel = packageItem.transform.parent.GetComponent<DragPanel>();
        canPut= packageItem.TryRefreshOriginPos();//�������������ı����Լ������Ϣ,���λ�ò��Ϸ��ǾͲ��÷�

        for (int i = 0; i < packageItem.itemData.length; i++)
        {
            for (int k = 0; k < packageItem.itemData.width; k++)
            {
                if (packageItem.package.grids.gridList[i + packageItem.itemData.originPos.x, k + packageItem.itemData.originPos.y].isUsed == true)
                {
                    canPut = false;//�ж������´��ڵı���,�ӵ�ǰԭ���������ĳ���,�ж���û�и��ӱ�ռ�á�
                    break;     //�о�����ѭ�������ж�
                }
            }
        }
        if (!canPut)//������ܷ��þͷ���֮ǰ��¼��ԭ���뱳��
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
            canPut = true;//����һ��canPut
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
        packageItem.GetComponent<Image>().raycastTarget = true;//���Խ����߼�⿪���ˣ�������һ����ק

    }
  

}
