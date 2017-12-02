using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FillLetterPanel : MonoBehaviour
{
    public GameObject Tile, TileParent;

    private string[] correctAnswer = new string[] { "owels", "lphabet" };

    private const string k_allLetters = "qwertyuiopasdfghjklzxcvbnm";

    void Start()
    {
        string singleCorrectAnswer = string.Join("", correctAnswer);

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject newTile = Instantiate(Tile, TileParent.transform);
                newTile.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 125, -j * 125, 0);
            }
        }

        int counter = 0;
        System.Random r = new System.Random();
        foreach (Text tile in TileParent.transform.GetComponentsInChildren<Text>().OrderBy(x => r.Next()))
        {
            if (counter < singleCorrectAnswer.Length)
            {
                tile.text = singleCorrectAnswer[counter].ToString();
                tile.GetComponentInChildren<Text>().color = Color.yellow;
            }
            else
            {
                tile.text = k_allLetters[Random.Range(0, 25)].ToString();
            }
            counter++;
        }
    }
}
