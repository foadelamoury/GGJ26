using TMPro; // Assuming TextMeshPro is used, fallback to Text if not
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Slider healthSlider;
    [SerializeField] GameObject fillHealthSlider;
    [SerializeField] GameObject gameOverPanel;

    [Header("Navigation")]
    [SerializeField] private RectTransform navigationArrow;
    [SerializeField] private TextMeshProUGUI distanceText;

    private TaxiController taxiController;
    private Vehicle playerVehicle;

    private void Start()
    {
        // Find the player vehicle facade
        playerVehicle = FindFirstObjectByType<Vehicle>();

        if (playerVehicle != null)
        {
            if (playerVehicle.Collider != null)
            {
                playerVehicle.Collider.OnHealthChanged += UpdateHealth;
                playerVehicle.Collider.OnDeath += ShowGameOver;
            }

            if (playerVehicle.Taxi != null)
            {
                taxiController = playerVehicle.Taxi;
                taxiController.OnMoneyChanged.AddListener(UpdateMoney);
            }
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (navigationArrow != null) navigationArrow.gameObject.SetActive(false);
        if (distanceText != null) distanceText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (taxiController != null && taxiController.IsCarryingPassenger && taxiController.CurrentDestination != null)
        {
            UpdateNavigation(taxiController.CurrentDestination.transform.position);
        }
        else
        {
            if (navigationArrow != null && navigationArrow.gameObject.activeSelf) navigationArrow.gameObject.SetActive(false);
            if (distanceText != null && distanceText.gameObject.activeSelf) distanceText.gameObject.SetActive(false);
        }
    }

    private void UpdateNavigation(Vector3 destinationPos)
    {
        if (playerVehicle == null) return;

        if (navigationArrow != null)
        {
            if (!navigationArrow.gameObject.activeSelf) navigationArrow.gameObject.SetActive(true);

            // Calculate direction from player to destination
            Vector3 direction = destinationPos - playerVehicle.transform.position;

            // Calculate angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply rotation (Assuming arrow sprite points Right by default, 0 degrees)
            navigationArrow.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (distanceText != null)
        {
            if (!distanceText.gameObject.activeSelf) distanceText.gameObject.SetActive(true);

            float dist = Vector3.Distance(playerVehicle.transform.position, destinationPos);
            distanceText.text = $"{Mathf.CeilToInt(dist)}m";
        }
    }

    private void UpdateHealth(float health, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }
    }

    private void UpdateMoney(float money)
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: ${money:F0}";
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            fillHealthSlider.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }
}
