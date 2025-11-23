using TMPro;
using UnityEngine;

public class ShowTextTutorial : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI TutorialText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TutorialText.gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TutorialText.gameObject.SetActive(false);
    }
}
