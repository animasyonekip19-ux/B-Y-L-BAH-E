using UnityEngine;
using DG.Tweening; // DOTween'i kullanabilmek için bu satır şart!

public class GridManager : MonoBehaviour
{
    [Header("Tahta Boyutları")]
    public int genislik = 8;
    public int yukseklik = 8;
    
    [Header("Görsel Ayarlar")]
    public float bosluk = 1.2f;

    [Header("Oyun Objeleri")]
    public GameObject[] tasPrefableri; 

    private GameObject[,] tahta;
    private int[,] tahtaTipleri; 

    void Start()
    {
        tahta = new GameObject[genislik, yukseklik];
        tahtaTipleri = new int[genislik, yukseklik]; 
        TahtayiOlustur();
    }

    void TahtayiOlustur()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                float posX = (x - (genislik / 2f)) * bosluk;
                float posY = (y - (yukseklik / 2f)) * bosluk;
                Vector2 pozisyon = new Vector2(posX, posY);

                int rastgeleIndex;
                bool guvenliMi;
                int sonsuzDonguKorumasi = 0;

                do
                {
                    guvenliMi = true; 
                    rastgeleIndex = Random.Range(0, tasPrefableri.Length); 
                    
                    if (x >= 2 && tahtaTipleri[x - 1, y] == rastgeleIndex && tahtaTipleri[x - 2, y] == rastgeleIndex)
                        guvenliMi = false; 
                    
                    if (y >= 2 && tahtaTipleri[x, y - 1] == rastgeleIndex && tahtaTipleri[x, y - 2] == rastgeleIndex)
                        guvenliMi = false; 

                    sonsuzDonguKorumasi++;
                } 
                while (!guvenliMi && sonsuzDonguKorumasi < 100); 

                tahtaTipleri[x, y] = rastgeleIndex;

                GameObject secilenTas = tasPrefableri[rastgeleIndex];
                GameObject yeniTas = Instantiate(secilenTas, pozisyon, Quaternion.identity);
                
                yeniTas.transform.parent = this.transform;
                yeniTas.name = $"Tas {x},{y}";
                
                // YENİ: Taşa kendi koordinatlarını öğretiyoruz
                Tas tasScripti = yeniTas.GetComponent<Tas>();
                tasScripti.sutunX = x;
                tasScripti.satirY = y;

                tahta[x, y] = yeniTas;
            }
        }
    }

    // YENİ: Taşların yerini değiştiren ve animasyonu oynatan fonksiyon
    public void TasiYerDegistir(int baslangicX, int baslangicY, Vector2 yon)
    {
        // Hedef koordinatları bul
        int hedefX = baslangicX + (int)yon.x;
        int hedefY = baslangicY + (int)yon.y;

        // Tahtanın dışına kaydırmayı engelle (Sınır kontrolü)
        if (hedefX < 0 || hedefX >= genislik || hedefY < 0 || hedefY >= yukseklik)
            return;

        // Hafızadaki objeleri al
        GameObject tas1 = tahta[baslangicX, baslangicY];
        GameObject tas2 = tahta[hedefX, hedefY];

        // Tahta matrisini güncelle (Arka planda yerlerini değiştir)
        tahta[baslangicX, baslangicY] = tas2;
        tahta[hedefX, hedefY] = tas1;

        // Taşların içindeki X ve Y değerlerini de güncelle
        tas1.GetComponent<Tas>().sutunX = hedefX;
        tas1.GetComponent<Tas>().satirY = hedefY;
        tas2.GetComponent<Tas>().sutunX = baslangicX;
        tas2.GetComponent<Tas>().satirY = baslangicY;

        // DOTween ile pürüzsüz yer değiştirme animasyonu (0.3 saniyede)
        tas1.transform.DOMove(tas2.transform.position, 0.3f);
        tas2.transform.DOMove(tas1.transform.position, 0.3f);
    }
}
