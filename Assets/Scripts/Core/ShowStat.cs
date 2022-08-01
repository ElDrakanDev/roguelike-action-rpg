using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;
using Game.Events;
using Game.Items;

public class ShowStat : MonoBehaviour
{
    public Stat stat; 
    [SerializeField] float statAmount = -1.111111111f;

    private void Start()
    {
        stat = new Stat(10, this);

        FindObjectOfType<StatsItem>().Interact(gameObject);
    }
    void Update()
    {
        statAmount = stat.Value;

        /*var keyboard = Keyboard.current;

        if (keyboard.fKey.wasPressedThisFrame)
        {
            var modifier = new StatModifier(1, gameObject, stat, StatType.Flat);
            stat.Add(modifier);
            statAmount = stat.Value;
        }
        else if (keyboard.mKey.wasPressedThisFrame)
        {
            var modifier = new StatModifier(0.15f, gameObject, stat, StatType.Mult);
            stat.Add(modifier);
            statAmount = stat.Value;
        }
        else if (keyboard.sKey.wasPressedThisFrame)
        {
            string root = "data";
            var data = stat;
            QuickSaveWriter.Create(root)
                .Write("stat", data)
                .Commit();
            Debug.Log($"Guardado en root {root}");
            Debug.Log($"Se escribió tipo {data.GetType()}: {data}");
        }
        else if (keyboard.rKey.wasPressedThisFrame)
        {
            string root = "data";
            var reader = QuickSaveReader.Create(root);
            var data = reader.Read<Stat>("stat");
            Debug.Log($"Se leyó en {root} datos = {data} tipo {data.GetType()}");
            Debug.Log($"STAT: {data.Value}");
        }

        */
    }

    public void ASD(EventData data){
        Debug.Log("ASDASD");
    }
}
