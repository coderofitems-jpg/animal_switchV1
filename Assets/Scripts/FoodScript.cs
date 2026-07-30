using UnityEngine;

public class FoodScript : MonoBehaviour {

    [SerializeField] private float healthValue;

    public float GetHealthValue() {
        return healthValue;
    }
}