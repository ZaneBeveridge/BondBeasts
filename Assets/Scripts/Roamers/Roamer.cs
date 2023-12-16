using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Roamer : MonoBehaviour
{
    public int id;
    public RoamerType roamerType;
    [Header("Refs")]

    public Node currentNode;
    public GameManager GM;


    public abstract void Init(Node node, GameManager g, RoamerController c);
    public abstract void DoTurn();

}



public enum RoamerType
{
    Alpha,
    Healer,
    Trader
    
}
