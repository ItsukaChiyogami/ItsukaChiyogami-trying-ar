using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem; 

public class CarManager : MonoBehaviour
{
    public GameObject CarPrefab;
    public ReticleBehaviour Reticle;
    public DrivingSurfaceManager DrivingSurfaceManager;
    public CarBehaviour Car;

    void Start()
    {
        Debug.Log("========== CAR MANAGER START ==========");
        Debug.Log("CarPrefab assigned: " + (CarPrefab != null));
        Debug.Log("Reticle assigned: " + (Reticle != null));
        Debug.Log("DrivingSurfaceManager assigned: " + (DrivingSurfaceManager != null));
        Debug.Log("======================================");
    }

    void Update()
    {
        // Debug plane detection
        if (Reticle != null && Reticle.CurrentPlane != null)
        {
            Debug.Log("✅ Plane detected: " + Reticle.CurrentPlane.name);
        }

        // Cek kondisi spawn
        bool carIsNull = (Car == null);
        bool wasTapped = WasTapped();
        bool planeExists = (Reticle != null && Reticle.CurrentPlane != null);

        if (wasTapped)
        {
            Debug.Log("🖱️ TAP DETECTED!");
            Debug.Log("  - Car is null: " + carIsNull);
            Debug.Log("  - Plane exists: " + planeExists);
        }

        // Spawn mobil
        if (carIsNull && wasTapped && planeExists)
        {
            Debug.Log("🚗🚗🚗 SPAWNING CAR NOW! 🚗🚗🚗");
            
            var obj = GameObject.Instantiate(CarPrefab);
            Car = obj.GetComponent<CarBehaviour>();
            Car.Reticle = Reticle;
            Car.transform.position = Reticle.transform.position;
            Car.transform.rotation = Reticle.transform.rotation;
            
            DrivingSurfaceManager.LockPlane(Reticle.CurrentPlane);
            
            Debug.Log("✅ Car spawned at: " + Car.transform.position);
        }
    }

    private bool WasTapped()
    {
        // NEW INPUT SYSTEM - Pakai Touchscreen
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Debug.Log("Touch detected via New Input System");
                return true;
            }
        }

        // Fallback untuk mouse (testing di editor)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse click detected via New Input System");
            return true;
        }

        return false;
    }
}