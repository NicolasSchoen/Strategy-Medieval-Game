using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAttributes : MonoBehaviour
{
    public string buildingName;
    [Multiline]
    public string description;
    public int blockWidth = 1;
    public int blockHeight = 1;
    public int modelType = 0;
    public int subtype = 0;

    public int requiredWood = 0;
    public int requiredStone = 0;
    public int requiredIron = 0;

    public int providedBeds = 0;
    public int providedFoodStorage = 0;
    public int providedRessourceStorage = 0;

    public string interactiontext = "press E to ";

    public bool requireBuildPlace = false;

    public bool isPlaced = false;

    public int getLength()
    {
        return blockHeight;
    }

    public int getWidth()
    {
        return blockWidth;
    }

    public int getModelType()
    {
        return modelType;
    }

    public int getRequiredWood()
    {
        return requiredWood;
    }

    public int getRequiredStone()
    {
        return requiredStone;
    }

    public int getRequiredIron()
    {
        return requiredIron;
    }

    public bool getRequireBuildplace()
    {
        return requireBuildPlace;
    }

    public void placeObject()
    {
        isPlaced = true;
        gameStatistic.GS.foodStorage += providedFoodStorage;
        gameStatistic.GS.ressourceStorage += providedRessourceStorage;
        gameStatistic.GS.addBed(providedBeds);
    }

    public bool checkRessources()
    {
        if (globalVariables.GodMode) return true;
        return gameStatistic.GS.buildPossible(requiredWood, requiredStone, requiredIron);
    }

    public bool useRessources()
    {
        if (globalVariables.GodMode) return true;
        return gameStatistic.GS.useRessources(requiredWood,requiredStone,requiredIron);
    }

    public bool destroyBuilding()
    {
        gameStatistic.GS.placedObjects.Remove(gameObject);
        float ownrotation = transform.rotation.eulerAngles.y;
        int ownWidth, ownLength;
        if(ownrotation == 90 || ownrotation == 270)
        {
            ownWidth = blockHeight;
            ownLength = blockWidth;
            Debug.Log("Rotation = 90 or 270");
        }
        else
        {
            ownWidth = blockWidth;
            ownLength = blockHeight;
        }
        MapController.MC.changeMapGridDirectly(transform.position,ownWidth, ownLength, 0);
        Destroy(gameObject);
        gameStatistic.GS.foodStorage -= providedFoodStorage;
        gameStatistic.GS.ressourceStorage -= providedRessourceStorage;
        gameStatistic.GS.collectWood(requiredWood/globalVariables.returnFactor);
        gameStatistic.GS.collectStone(requiredStone / globalVariables.returnFactor);
        gameStatistic.GS.collectIron(requiredIron / globalVariables.returnFactor);
        gameStatistic.GS.checkRessourceLimitations();
        gameStatistic.GS.removeBed(providedBeds);
        return true;
    }
}
