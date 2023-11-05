using UnityEngine;
using UnityEngine.UI;

public class RandomColor : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value);
    }
}
