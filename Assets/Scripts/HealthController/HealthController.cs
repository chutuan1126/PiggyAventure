using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    public float fontSize = 7f;
    public Color color = Color.white;

    public float fontSizeCritical = 10f;
    public Color colorCritical = Color.yellow;

    public Image fillBar;
    public TextMeshProUGUI valueText;

    [SerializeField] GameObject prefab;

    private void Start()
    {
        fillBar = transform.Find("FillBar").GetComponentInChildren<Image>();
        valueText = transform.Find("ValueText").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateBar(float currentValue, float maxValue)
    {
        valueText.text = currentValue.ToString();
        fillBar.fillAmount = currentValue / maxValue;
    }

    public void ShowDamage(float damage, bool isCritical)
    {
        StartCoroutine(ShowTextAfterAnyTime(damage, isCritical));
    }

    public IEnumerator ShowTextAfterAnyTime(float value, bool isCritical)
    {
        yield return new WaitForSeconds(.2f);
        SpawnPrefab(value, isCritical);
    }

    public void SpawnPrefab(float value, bool isCritical)
    {
        Vector3 position = new(transform.position.x, transform.position.y, transform.position.z);

        GameObject valueText = Instantiate(prefab, position, Quaternion.identity);

        valueText.transform.SetParent(transform);

        valueText.transform.localScale = new Vector3(1, 1, 0);

        TextMeshProUGUI textMesh = valueText.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        textMesh.text = "-" + value;

        if (isCritical)
        {
            textMesh.color = colorCritical;
            textMesh.fontSize = fontSizeCritical;
        }
        else
        {
            textMesh.color = color;
            textMesh.fontSize = fontSize;
        }
    }
}
