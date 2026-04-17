using UnityEngine;

public class HarfDansi : MonoBehaviour
{
    void Start()
    {
        // 1. Rastgele Yavaþça Süzülme (Suyun içindeymiþ gibi aþaðý-yukarý)
        float randTimeY = Random.Range(2f, 4f);
        float randDistY = Random.Range(-0.15f, 0.15f);

        LeanTween.moveLocalY(gameObject, transform.localPosition.y + randDistY, randTimeY)
            .setEaseInOutSine()
            .setLoopPingPong();

        // 2. Kameraya Doðru Hafifçe Yaklaþma ve Uzaklaþma (Derinlik algýsý)
        float randTimeZ = Random.Range(3f, 5f);
        float randDistZ = Random.Range(-0.2f, 0.2f);

        LeanTween.moveLocalZ(gameObject, transform.localPosition.z + randDistZ, randTimeZ)
            .setEaseInOutSine()
            .setLoopPingPong();

        // 3. Hafifçe "Nefes Alma" Efekti (Büyüyüp Küçülme)
        // Bu, harflerin kaskatý durmasýný engeller
        LeanTween.scale(gameObject, Vector3.one * 1.1f, Random.Range(1f, 2f))
            .setEaseInOutSine()
            .setLoopPingPong();

        // 4. Yatayda çok hafif kayma
        LeanTween.moveLocalX(gameObject, transform.localPosition.x + Random.Range(-0.05f, 0.05f), 2.5f)
            .setEaseInOutSine()
            .setLoopPingPong();
    }
}
