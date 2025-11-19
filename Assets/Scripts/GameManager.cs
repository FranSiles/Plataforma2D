using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton : to use methods from this class I dont need to get a component
    public static GameManager Instance; //Capitalize this because is a static reference to my object

    //Points manager
    private int playerPoints;

    public int PlayerPoints { get => playerPoints; set => playerPoints = value; }

    private void Awake()
    {
        //If the gameObject is already created destroy new and mantain old
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Assign the static instance to the current gameobject
            Instance = this;
            //maintain the instance to the current gameobject to the next scene
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
