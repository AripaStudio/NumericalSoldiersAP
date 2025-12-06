using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;

namespace NumericalSoldiersAP.Service
{
    public class UImanager
    {
        private Panel MessagePanel;
        private Panel EntireGameScreen;
        private Panel playerInfoTable;
        private string currentMessage = "Game Started";
        private string currentPlayerInfo = "";

        public void CreateUI()
        {
            Table table = new Table();
            table.Expand = true;
            table.AddColumn("Welcome to Numerical Soldier AP (Aripa Pars Studio) Version 1.1.0");

            EntireGameScreen = new Panel(table);
            EntireGameScreen.Header = new PanelHeader("Numerical Soldier");
            EntireGameScreen.Border = BoxBorder.Double;
            EntireGameScreen.Padding = new Padding(2, 2, 2, 2);
            EntireGameScreen.Expand = true;

            playerInfoTable = new Panel(currentPlayerInfo);
            playerInfoTable.Header = new PanelHeader("[yellow]PlayerInfo[/]");
            playerInfoTable.Expand = true;
            table.AddRow(playerInfoTable);

            Panel CenterGamePanel = new Panel(CreateBuildingUI(4));
            CenterGamePanel.Expand = true;
            CenterGamePanel.Header = new PanelHeader("[bold]GameMap and Building[/]");
            table.AddRow(CenterGamePanel);

            MessagePanel = new Panel(new Markup($"[bold white]{currentMessage}[/]"));
            MessagePanel.Header = new PanelHeader("[yellow]Game Messages[/]");
            MessagePanel.Expand = true;
            table.AddRow(MessagePanel);

            AnsiConsole.Clear();
            AnsiConsole.Write(EntireGameScreen);

        }

        public Table CreateBuildingUI(short ValueBuilding)
        {
            List<Markup> contentMarkups = new List<Markup>();
            List<Panel> buildingPanels = new List<Panel>();
            for (int i = 0; i < ValueBuilding; i++)
            {
                var content = new Markup($"[yellow] Building {i + 1} [/]");
                contentMarkups.Add(content);
                var building = new Panel(contentMarkups[i]);
                buildingPanels.Add(building);
            }

            Table buildingLayoutRowOneTable = new Table();
            buildingLayoutRowOneTable.AddColumn("Building Section up");
            Table buildingLayoutRowTwoTable = new Table();
            buildingLayoutRowTwoTable.AddColumn("Building Section down");
            Table WayEnemyBase = new Table();
            for (int i = 0; i < 10; i++)
            {
                WayEnemyBase.AddColumn(" " +(i + 1));
            }
            WayEnemyBase.AddColumn("Enemy Base");

            List<Markup> rowCells =new List<Markup>();
            for (int i = 0; i < 10; i++)
            {
                rowCells.Add(new Markup($" {i + 1}"));
            }
            rowCells.Add(new Markup("[red] Enemy Base [/]"));

            WayEnemyBase.AddRow(rowCells);








            Columns centerLayoutRowOne = new Columns(buildingLayoutRowOneTable);
            Columns centerLayoutRowTwo = new Columns(buildingLayoutRowTwoTable);
            Columns centerLayoutRowEnemy = new Columns(WayEnemyBase);
            
            centerLayoutRowOne.Padding = new Padding(2, 2, 2, 2);
            centerLayoutRowTwo.Padding = new Padding(2, 2, 2, 2);

            for (int i = 0; i < ValueBuilding ; i++)
            {
                if (i % 2 == 0)
                {
                    buildingLayoutRowOneTable.AddRow(buildingPanels[i ]);
                }
                else
                {
                    buildingLayoutRowTwoTable.AddRow(buildingPanels[i]);
                }
            }
          

            var verticalLayout = new Table();
            verticalLayout.Expand = true;
            verticalLayout.Border = TableBorder.None;

            verticalLayout.AddColumn(new TableColumn("Game Content").NoWrap());
            verticalLayout.AddRow(centerLayoutRowOne);
            verticalLayout.AddRow(centerLayoutRowEnemy);
            verticalLayout.AddRow(centerLayoutRowTwo);

            return verticalLayout;
        }

        public void NewMessage(string message)
        {
            currentMessage = message;

            CreateUI();
        }

        public short SelectBuildingId()
        {
            var Prompt = AnsiConsole.Prompt(new SelectionPrompt<short>()
                .PageSize(4)
                .Title("Select (type [green]Number[/]) the desired [blue]building.[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more NameBuilding)[/]")
                .AddChoices(new short[] {
                   1,2 ,
                   3 ,4
                }));
            int output = Prompt - 1;


            return Convert.ToInt16(output);

        }


        public int AskForTroops(short buildingId, int RemainingNumber)
        {
            var minCapacity = CurrentBuildings.buildings[buildingId].MinCapacityBuilding;
            var maxCapacity = CurrentBuildings.buildings[buildingId].MaxCapacityBuilding;
            var prompt = AnsiConsole.Prompt(new TextPrompt<int>("How many of your [green]troops[/] do you want to put in this [yellow]building?[/]")
                .PromptStyle("cyan")
                .ValidationErrorMessage($"[red]Error[/]: The entered numbers must be between [green]{minCapacity}[/] and [green]{maxCapacity}.[/]")
                .Validate(troops =>
                {
                    return troops >= minCapacity && troops <= maxCapacity && troops <= RemainingNumber;
                }));

            return prompt;
        }

        public void UpdatePlayerInfo(PlayerInfo playerInfo)
        {
            currentPlayerInfo = $"Total player troops : {playerInfo.PlayerArmy} ,,  remaining soldiers : {playerInfo.ArmyAvailable} ,, PlayerWin : {PlayerManager.PlayerWin} ,, PlayerLose : {PlayerManager.PlayerLose}";

            CreateUI();
        }

        public string InputYesNo(string MessageText)
        {
            var ReadLine = AnsiConsole.Prompt(
                new TextPrompt<string>(MessageText)
                    .PromptStyle("cyan")
                    .AddChoice("yes")
                    .AddChoice("no")
                    .DefaultValue("yes"));
            return ReadLine;
        }





    }


    public class UIManagerWar
    {
        public UImanager UImanager = new UImanager();
        public void StartWar(GameStateEnum gameState) {
        
            AnsiConsole.Clear();
            var UIwar = UImanager.CreateBuildingUI(4);
            AnsiConsole.Write(UIwar);

            ShowResultWar(gameState);

            Thread.Sleep(2000);

            AnsiConsole.Clear();
            UImanager.CreateUI();
        }



        public void ShowResultWar(GameStateEnum warResult)
        {
            FigletText StateGameText = new FigletText("");
            AnsiConsole.Clear();


            switch (warResult)
            {
                case GameStateEnum.Win:
                    {
                        StateGameText = new FigletText("WINNER (:")
                            .Centered()
                            .Color(Color.GreenYellow);
                        break;
                    }
                case GameStateEnum.Lose:
                    {
                        StateGameText = new FigletText("WINNER (:")
                            .Centered()
                            .Color(Color.GreenYellow);
                        break;
                    }
            }

            AnsiConsole.Write(StateGameText);

            Thread.Sleep(3000);

            AnsiConsole.Clear();

            UImanager.CreateUI();

        }
    }
}
