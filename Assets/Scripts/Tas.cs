using UnityEngine;

public class Tas : MonoBehaviour
{
    private Vector2 ilkDokunmaPozisyonu;
    private Vector2 sonDokunmaPozisyonu;
    public float kaydirmaHassasiyeti = 0.5f;

    public int sutunX;
    public int satirY;

    private GridManager gridManager; // Yöneticiyi tanıyacak değişken

    void Start()
    {
        // Oyun başladığında sahnede GridManager'ı bul ve bağlan
        gridManager = FindObjectOfType<GridManager>();
    }

    void OnMouseDown()
    {
        ilkDokunmaPozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        sonDokunmaPozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AciHesapla();
    }

    void AciHesapla()
    {
        float farkX = sonDokunmaPozisyonu.x - ilkDokunmaPozisyonu.x;
        float farkY = sonDokunmaPozisyonu.y - ilkDokunmaPozisyonu.y;

        if (Mathf.Abs(farkX) > kaydirmaHassasiyeti || Mathf.Abs(farkY) > kaydirmaHassasiyeti)
        {
            if (Mathf.Abs(farkX) > Mathf.Abs(farkY))
            {
                if (farkX > 0) Kaydir(Vector2.right);
                else Kaydir(Vector2.left);
            }
            else
            {
                if (farkY > 0) Kaydir(Vector2.up);
                else Kaydir(Vector2.down);
            }
        }
    }

    void Kaydir(Vector2 yon)
    {
        // Yönü bulduk, şimdi yer değiştirme işlemini yöneticiye devrediyoruz
        gridManager.TasiYerDegistir(sutunX, satirY, yon);
    }
}