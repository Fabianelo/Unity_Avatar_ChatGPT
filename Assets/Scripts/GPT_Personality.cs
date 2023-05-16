using ChatGPTWrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPT_Personality : MonoBehaviour
{
    public bool enabled;
    public enum Peronajes_Enum { DarthVader, LukeSkywalker, Yoda, Chewbacca, CapitanAmerica, IronMan, Thor, Hulk,  ViudaNegra, Gandalf, Sauron, Frodo, Gollum}   
    public enum Peliculas_Enum { StarWars, Avengers, SeñorDeLosAnillos}
    public Peronajes_Enum personaje;
    public Peliculas_Enum pelicula;
    public ChatGPTConversation conversation;
    [TextArea(10,10)]
    public string prompt;
    public int selection;
    private List<string> personajes = new List<string> { "Darth Vader", "Luke Skywalker","Yoda", "Chewbacca", "Capitan America", "Iron Man", "Thor", "Hulk", "Viuda Negra", "Gandalf", "Sauron", "Frodo Bolsón", "Gollum" };
    private List<string> peliculas = new List<string> { "StarWars", "Avengers", "The Lord of the Rings" };
    void Awake()
    {
        if (enabled)
        {
            conversation._chatbotName = personajes[(int)personaje];
            string composePromt = prompt.Replace("#Personality#", personajes[(int)personaje]);
            composePromt = composePromt.Replace("#Film#", peliculas[(int)pelicula]);
            conversation._initialPrompt = composePromt;
        }
    }
}
