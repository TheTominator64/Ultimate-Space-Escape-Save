using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUIAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;
    // Start is called before the first frame update
    void Start()
    {
        cachedScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = cachedScale;
    }
    public void OnDisable()
    {
        transform.localScale = cachedScale;
    }
    private IEnumerator EnlargePanel()
    {
        yield return null;
    }
}
