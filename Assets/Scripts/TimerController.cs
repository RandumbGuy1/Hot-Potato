using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] KeyCode passKey;
    [SerializeField] SliderTimer timer;
    [SerializeField] TextMeshPro text;

    [SerializeField] List<GameObject> players = new List<GameObject>();

    void Start()
    {
        Configure();
    }

    void Update()
    {
        if (Input.GetKeyDown(passKey))
        {
            timer.Finish();
        }
    }

    public void RemovePlayer()
    {
        if (players.Count <= 0) return;

        int index = timer.Turn % players.Count;
        players[index].SetActive(false);
        players.RemoveAt(index);

        index = timer.Turn % players.Count;

        if (players.Count <= 1)
        {
            Configure();
            timer.enabled = false;
            text.text = players[index].name + " Wins!";
        }
    }

    public void Configure()
    {
        if (players.Count == 0) return;

        int index = timer.Turn % players.Count;
        for (int i = 0; i < players.Count; i++) players[i].SetActive(i == index);
        text.text = players[index].name;
    }
}
