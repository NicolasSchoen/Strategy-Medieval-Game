using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameStatistic : MonoBehaviour{
    public int wood;
    public int stone;
    public int iron;
    public int food;
    public int population;
    public int beds;
    public int foodStorage;
    public int ressourceStorage;

    public List<GameObject> persons = new List<GameObject>();
    public List<GameObject> placedObjects = new List<GameObject>();
    public List<GameObject> enemys = new List<GameObject>();

    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI ironText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI populationText;
    public Canvas deadCanvas;

    public static gameStatistic GS;

    private void Start()
    {
        globalVariables.prepareWorld();
        wood = globalVariables.startWood;
        stone = globalVariables.startStone;
        iron = globalVariables.startIron;
        food = globalVariables.startFood;
        population = 0;
        beds = 0;
        foodStorage = 0;
        ressourceStorage = 0;
        updateText();
        GS = this;
    }

    /*public gameStatistic(int wood, int stone, int iron, int food, int population)
    {
        this.wood = wood;
        this.stone = stone;
        this.iron = iron;
        this.food = food;
        this.population = population;
    }*/

    public void updateText()
    {
        if(woodText != null)woodText.text = wood.ToString() + " / " + ressourceStorage.ToString();
        if (stoneText != null) stoneText.text = stone.ToString() + " / " + ressourceStorage.ToString();
        if (ironText != null) ironText.text = iron.ToString() + " / " + ressourceStorage.ToString();
        if (foodText != null) foodText.text = food.ToString() + " / " + foodStorage.ToString();
        if (populationText != null) populationText.text = population.ToString() + " / " + beds.ToString();
    }

    public void removeAllPersons()
    {
        foreach (GameObject person in persons)
            Destroy(person);

        persons.Clear();

        foreach (GameObject person in enemys)
            Destroy(person);

        enemys.Clear();
    }

    public void removeAllEnemys()
    {
        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
        enemys.Clear();
    }

    public void resetValues()
    {
        ressourceStorage = 0;
        foodStorage = 0;
    }

    public void checkRessourceLimitations()
    {
        if (wood > ressourceStorage) wood = ressourceStorage;
        if (stone > ressourceStorage) stone = ressourceStorage;
        if (iron > ressourceStorage) iron = ressourceStorage;
        if (food > foodStorage) food = foodStorage;
    }

    public void collectWood(int amount = 1)
    {
        wood += amount;
        if (wood > ressourceStorage) wood = ressourceStorage;
        updateText();
    }

    public void collectStone(int amount = 1)
    {
        stone += amount;
        if (stone > ressourceStorage) stone = ressourceStorage;
        updateText();
    }

    public void collectIron(int amount = 1)
    {
        iron += amount;
        if (iron > ressourceStorage) iron = ressourceStorage;
        updateText();
    }

    public void collectFood(int amount = 1)
    {
        food += amount;
        if (food > foodStorage) food = foodStorage;
        updateText();
    }

    public void personBorn(GameObject person)
    {
        population++;
        persons.Add(person);
        updateText();
    }

    public void addBed(int amount)
    {
        beds += amount;
        updateText();
    }

    public void removeBed(int amount)
    {
        beds -= amount;
        if (beds < 0) beds = 0;
        updateText();
    }

    public bool takeWood(int amount = 1)
    {
        if(wood >= amount)
        {
            wood -= amount;
            updateText();
            return true;
        }
        return false;
    }

    public bool takeStone(int amount = 1)
    {
        if (stone >= amount)
        {
            stone -= amount;
            updateText();
            return true;
        }
        return false;
    }

    public bool takeIron(int amount = 1)
    {
        if (iron >= amount)
        {
            iron -= amount;
            updateText();
            return true;
        }
        return false;
    }

    public bool eatFood(int amount)
    {
        if(food >= 0+amount)
        {
            food-= amount;
            updateText();
            return true;
        }
        return false;
    }

    public bool personDied(GameObject person)
    {
        if(population > 0)
        {
            population -= 1;
            persons.Remove(person);
            updateText();
            if(population <= 0) allPersonsDead();
            return true;
        }
        return false;
    }

    public int getWood()
    {
        return wood;
    }

    public int getStone()
    {
        return stone;
    }

    public int getIron()
    {
        return iron;
    }

    public int getFood()
    {
        return food;
    }

    public bool buildPossible(int w, int s, int i)  //checks if a building could be build with the ressources
    {
        if (wood >= w && stone >= s && iron >= i) return true;
        return false;
    }

    public bool useRessources(int w, int s, int i)
    {
        if (buildPossible(w, s, i))
        {
            takeWood(w);
            takeStone(s);
            takeIron(i);
            updateText();
            return true;
        }
        return false;
    }

    public void nextDay()
    {
        foreach (GameObject person in persons)
        {
            person.GetComponent<personController>().eat();
        }
    }

    public void allPersonsDead()
    {
        if(deadCanvas != null)deadCanvas.enabled = true;
    }
}
