using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class ItemComponent : MonoBehaviour
{
    public EItem item = EItem.NONE;
    [HideInInspector] public EItemState state = EItemState.NONE;
    public int componentId = 0;

    //public Rigidbody rb;
    public MeshRenderer meshRenderer;

    public ItemComponent SnapComponent;

    float distance = 10;

    float mZPos;

    ObjectControl control;

    public Material defaultMaterial;
    private void Awake()
    {
        control = transform.GetComponentInParent<ObjectControl>();
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }

    private void Start()
    {
        SetObjectProperties();
    }

    public void SetObjectProperties()
    {
        switch (state)
        {
            case EItemState.FIXED:
                meshRenderer.material = defaultMaterial;
                break;

            case EItemState.SILHOUETTE:
                meshRenderer.material = PlayerController.instance.silhouetteMaterial;
                break;

            case EItemState.BROKEN:
                meshRenderer.material = defaultMaterial;
                break;

            case EItemState.PICKEDUP:
                meshRenderer.material = defaultMaterial;
                break;
        }
    }

    public void SetState(EItemState itemState)
    {
        if (state == itemState)
        {
            return;
        }

        state = itemState;
        SetObjectProperties();
    }
    private void OnMouseDown()
    {
        if (state != EItemState.BROKEN || !GameManager.instance.inputEnabled)
        {
            return;
        }

        for (int i = 0; i < control.brokenObjs.Count; ++i)
        {
            if (this.gameObject == control.brokenObjs[i] && state == EItemState.BROKEN)
            {
                mZPos = Camera.main.WorldToScreenPoint(control.oldPositions[i].transform.position).z;
            }
        }
    }
    private void OnMouseDrag()
    {
        if(state != EItemState.BROKEN || !GameManager.instance.inputEnabled)
        {
            return;
        }

        for (int i = 0; i < control.brokenObjs.Count; ++i)
        {
            if (this.gameObject == control.brokenObjs[i])
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mZPos);
                Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                objectPos.z = control.oldPositions[i].transform.position.z;
                transform.position = objectPos;

                if(SnapComponent != null)
                {
                    if (Vector3.Distance(SnapComponent.transform.position, transform.position) < 0.2f)
                    {
                        transform.DOMove(SnapComponent.transform.position, 0.2f);
                        transform.DORotate(SnapComponent.transform.rotation.eulerAngles, 0.2f);
                        transform.parent = PlayerController.instance.control.transform;
                        SetState(EItemState.FIXED);
                        Destroy(SnapComponent.gameObject);
                        SnapComponent = null;
                        PlayerController.instance.silhouetteGenerator.silCount++;
                        float fill = (float)PlayerController.instance.silhouetteGenerator.silCount / (float)PlayerController.instance.silhouetteGenerator.totalBrokenCount;
                        GameManager.instance.UpdateFill(fill);
                        SoundManager.instance.PlayClip(EAudioClip.SUCCESS_SFX,1);
                        if(PlayerController.instance.silhouetteGenerator.silCount == PlayerController.instance.silhouetteGenerator.totalBrokenCount)
                        {
                            GameManager.instance.OnLevelCompleted();
                        }
                    }
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (state != EItemState.BROKEN || !GameManager.instance.inputEnabled)
        {
            return;
        }

        for (int i = 0; i < control.brokenObjs.Count; ++i)
        {
            if (this.gameObject == control.brokenObjs[i])
            {
                transform.DOMove(control.placmeents[i].transform.position, 0.2f).SetEase(Ease.OutBack);
                SoundManager.instance.PlayClip(EAudioClip.FAILURE_SFX,.25f);
            }
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(state == EItemState.SILHOUETTE)
    //    {
    //        SnapComponent = other.gameObject.GetComponent<ItemComponent>();
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if(SnapComponent == null || state != EItemState.SILHOUETTE)
    //    {
    //        return;
    //    }

    //    if(SnapComponent.componentId == componentId)
    //    {
    //        if(Vector3.Distance(SnapComponent.transform.position,transform.position) < 0.2f)
    //        {
    //            SnapComponent.transform.DOMove(transform.position, 0.2f);
    //            SnapComponent.transform.DORotate(transform.rotation.eulerAngles, 2.0f);
    //            SnapComponent.SetState(EItemState.FIXED);
    //            SnapComponent = null;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (state != EItemState.SILHOUETTE)
    //    {
    //        return;
    //    }
    //    SnapComponent = null;
    //}
}

public enum EItem
{
    NONE = -1,
    CUBE,
}

public enum EItemState
{
    NONE = -1,
    FIXED,
    SILHOUETTE,
    PICKEDUP,
    BROKEN,

}
