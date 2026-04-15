using UnityEngine;
using System.Collections;

public class HarfUcurucu : MonoBehaviour
{
    void Start()
    {
        // Obje açýldýktan 3 saniye sonra HarfiUcur fonksiyonunu çalýþtýr
        Invoke("HarfiUcur", 3f);
    }
    public void HarfiUcur()
    {
        Vector3 rastgeleYon = new Vector3(Random.Range(-2f, 2f), Random.Range(2f, 5f), Random.Range(1f, 3f));

        // Hareketi baþlat
        LeanTween.move(gameObject, transform.position + rastgeleYon, 1.5f).setEase(LeanTweenType.easeOutQuad);

        // Þeffaflaþtýrýp yok et
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            LeanTween.alphaCanvas(cg, 0f, 1.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => {
                gameObject.SetActive(false);
            });
        }
    }
}