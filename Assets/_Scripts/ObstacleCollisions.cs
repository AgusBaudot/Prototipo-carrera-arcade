using UnityEngine;
using UnityEngine.Events;

public class ObstacleCollisions : MonoBehaviour
{
    [SerializeField] private UnityEvent onWin;
    
    private SpeedBoost _speedBoost;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _speedBoost??=  GetComponent<SpeedBoost>();
            _speedBoost.SetLocked(true);
        }

        if (other.CompareTag("End"))
        {
            onWin?.Invoke();
            Destroy(gameObject);
        }
    }
}