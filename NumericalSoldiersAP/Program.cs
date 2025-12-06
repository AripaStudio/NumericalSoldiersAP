// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using NumericalSoldiersAP.Service;


Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);


DataBuilder.BuildGameData();

GameManager gameManager = new GameManager();

gameManager.StartGame();



