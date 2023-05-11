using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class IngameMenuUI : MonoBehaviour
{
    [SerializeField] GameObject album;

    [SerializeField] DetailUI detail;

    [SerializeField] Transform speciesPanel;

    [SerializeField] GameObject speciePrefab;

    [SerializeField] Transform photoPanel;

    [SerializeField] GameObject photoPrefab;

    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        RenderSpecies();
    }
    
    private void OnEnable() {
        album.SetActive(true);
        detail.gameObject.SetActive(false);

    }

    private void RenderSpecies(){
        GameObject newObject;

        foreach (Animal.Order specie in 
            new Animal.Order[]{Animal.Order.Araneae, Animal.Order.Coleoptera, Animal.Order.Lepidoptera, Animal.Order.Odonata})
        {
            newObject = Instantiate(speciePrefab, speciesPanel.position, speciesPanel.rotation, speciesPanel);

        }
        newObject = Instantiate(speciePrefab, speciesPanel.position, speciesPanel.rotation, speciesPanel);

    }


    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f) {
           
        FileManager.LoadFromFile(FilePath, out Texture2D SpriteTexture);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),new Vector2(0,0), PixelsPerUnit);

        return NewSprite;
    }

   public void EmptyPanel(){
        int children = photoPanel.childCount;
        for (int i = children - 1; i >= 0; i--){
            Destroy(photoPanel.GetChild(i).gameObject);
        }
   }

    public void ActiveDetail(Animal pAnimal, Sprite pSprite, bool pTaken)
    {
        album.SetActive(false);
        detail.gameObject.SetActive(true);
        detail.ShowDetail(pAnimal, pSprite, pTaken);
    }

    public void ActiveAlbum()
    {
        album.SetActive(true);
        detail.gameObject.SetActive(false);
    }
}
