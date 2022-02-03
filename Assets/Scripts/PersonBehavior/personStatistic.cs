using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personStatistic
{
    int hungry;     //0 .. 10
    int health;     //0 .. 10
    int happy;      //0 .. 10

    public personStatistic(int hungry, int health, int happy)
    {
        this.hungry = hungry;
        this.health = health;
        this.happy = happy;
    }
}
