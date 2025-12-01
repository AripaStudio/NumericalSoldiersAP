using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace NumericalSoldiersAP.Service
{

    public class GameManager
    {
        private UImanager UiManager = new UImanager();
        private BuildingManager buildingManager = new BuildingManager();
        public void StartGame()
        {


            var constants = GetGameConstantsClass.GetGameConstants();
            for (short i = 0; i < constants.BuildingTypesLength; i++)
            {
                var staticData = constants.BuildingTypes(i).Value;

                short path = (i < 2) ? (short)1 : (short)2;

                CurrentBuildings.buildings[i] = new Building(staticData, path);
            }
            //start game
            while (true)
            {
                PlayerInfo playerInfo = new PlayerInfo();
                var StartGameReadLine = UiManager.InputYesNo("StartGame? ([yellow]yes = start[/]) ([red]no = exit[/])");
                if (string.IsNullOrEmpty(StartGameReadLine))
                {
                    AnsiConsole.WriteLine("Please Enter Yes Or No");
                }
                else if (StartGameReadLine == "yes")
                {

                    UiManager.CreateUI();
                    UiManager.UpdatePlayerInfo(playerInfo);
                    ShowHelp();
                    UiManager.NewMessage("Start Game ...");

                    var attackEnemyPath = BuildRandomData.CreateRandomAttackEnemyPath();
                    var randomNumberSoldiers = BuildRandomData.CreateRandomNumberSoldiers();
                    playerInfo.PlayerArmy = randomNumberSoldiers.PlayerArmy;
                    playerInfo.ArmyAvailable = randomNumberSoldiers.PlayerArmy;
                    UiManager.InputYesNo("Shall we start the game?");
                    var StartGame = UiManager.InputYesNo("(ExitGame = no ,, StartGame = yes)");
                    
                    if (StartGame == "yes")
                    {
                        UiManager.NewMessage($"Number of soldiers in your army: {randomNumberSoldiers.PlayerArmy}");
                        Thread.Sleep(3000);
                        while (true)
                        {
                            UiManager.NewMessage("Choose a building with numbers 1, 2, 3, and 4.");
                            var selectBuilding = UiManager.SelectBuildingId();
                            var addTroops = UiManager.AskForTroops(selectBuilding, playerInfo.ArmyAvailable);
                            buildingManager.SetSolders(selectBuilding, addTroops);

                            playerInfo.ArmyAvailable -= addTroops;
                            if (playerInfo.ArmyAvailable < 100)
                            {
                                break;
                            }
                            UiManager.UpdatePlayerInfo(playerInfo);
                            var closeWhile =UiManager.InputYesNo($"Do you want to value buildings? (Continue = Yes, Start War = No)");
                            if (closeWhile == "no")
                            {
                                break;
                            }
                            
                            
                        }

                        UiManager.NewMessage("Game Started!....");
                        Thread.Sleep(2000);
                        UiManager.NewMessage($"Number of soldiers in the enemy army: {randomNumberSoldiers.EnemyArmy}\r\nEnemy commanders' chosen route for attack: {attackEnemyPath} ,, War Started ..");
                        var CurrentPlayerTroops = buildingManager.GetTroopsPlayerOnBuilding(attackEnemyPath);
                        WarStateStruct warState = new WarStateStruct()
                        {
                          ArmyPlayer   = CurrentPlayerTroops.currentAllocatedTroops,
                          ArmyEnemy = randomNumberSoldiers.EnemyArmy,
                          HealthsBuilding = CurrentPlayerTroops.currentHealth
                        };
                        var GameState = WarManager.StartWar(warState);

                        if (GameState == GameStateEnum.Win)
                        {
                            PlayerManager.PlayerWin++;
                            UiManager.ShowWinAnimation();
                        }
                        else
                        {
                            PlayerManager.PlayerLose++;
                            UiManager.ShowLoseAnimation();
                        }

                    }
                    else
                    {
                        break;
                    }



                }
                else if (StartGameReadLine == "no")
                {
                    break;
                }
            }



        }

        public void ShowHelp()
        {
            var zero = UiManager.InputYesNo("Do you need help? : (skip = no ,, Continue = yes)");
            if (zero == "no")
            {
                return;
            }

            UiManager.NewMessage("Welcome, Commander! Your mission is to defend the four fortress buildings against the advancing enemy. \nThe enemy will only attack one of the two paths: \n- **Path 1:** Buildings 1 & 3 are located here.\n- **Path 2:** Buildings 2 & 4 are located here. Your goal is to strategically allocate your troops to these buildings to match or exceed the incoming enemy force on the chosen path.");
            var one = UiManager.InputYesNo("(skip = no ,, Continue = yes)");
            if (one == "no")
            {
                return;
            }

            UiManager.NewMessage("Each building has a specific function:\n1.  **Soldier Type:** Provides high defensive health points (HP) and high troop capacity.\n2.  **Shooter Type:** Provides lower capacity but enhances the attack factor of your troops.");
            var two = UiManager.InputYesNo("(skip = no ,, Continue = yes)");
            if (two == "no")
            {
                return;
            }
            UiManager.NewMessage("The number of enemy troops and the path they choose are **randomly determined** each turn. Analyze the situation, deploy your army, and repel the invasion!\"");

        }


    }
}
