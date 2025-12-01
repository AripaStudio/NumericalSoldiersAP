using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            table.AddColumn("Welcome to Numerical Solders AP");

            EntireGameScreen = new Panel(table);
            EntireGameScreen.Header = new PanelHeader("Numerical Solders");
            EntireGameScreen.Border = BoxBorder.Double;
            EntireGameScreen.Padding = new Padding(2, 2, 2, 2);
            EntireGameScreen.Expand = true;

            playerInfoTable = new Panel(currentPlayerInfo);
            playerInfoTable.Header = new PanelHeader("[yellow]PlayerInfo[/]");
            playerInfoTable.Expand = true;
            table.AddRow(playerInfoTable);



            //building : 
            var content1 = new Markup("[yellow] Building 1 [/]");
            var content2 = new Markup("[yellow] Building 2 [/]");
            var content3 = new Markup("[yellow] Building 3 [/]");
            var content4 = new Markup("[yellow] Building 4 [/]");
            var building1Panel = new Panel(content1);
            var building2Panel = new Panel(content2);
            var building3Panel = new Panel(content3);
            var building4Panel = new Panel(content4);
            var centerLayoutRowOne = new Columns(building1Panel, building2Panel);
            var centerLayoutRowTwo = new Columns(building3Panel, building4Panel);
            centerLayoutRowOne.Padding = new Padding(2, 2, 2, 2);
            centerLayoutRowTwo.Padding = new Padding(2, 2, 2, 2);

            var verticalLayout = new Table();
            verticalLayout.Expand = true;
            verticalLayout.Border = TableBorder.None;

            verticalLayout.AddColumn(new TableColumn("Game Content").NoWrap());
            verticalLayout.AddRow(centerLayoutRowOne);
            verticalLayout.AddRow(centerLayoutRowTwo);
            


            Panel CenterGamePanel = new Panel(verticalLayout);
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
            int output = Prompt -1;

            
            return Convert.ToInt16(output);

        }


        public int AskForTroops(short buildingId , int RemainingNumber)
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



    public void ShowWinAnimation()
        {
            AnsiConsole.Clear();

            var winText = new FigletText("WINNER (:")
                .Centered()
                .Color(Color.GreenYellow);

            AnsiConsole.Write(winText);

            Thread.Sleep(3000);

            AnsiConsole.Clear();
            
            CreateUI();
            
        }

        public void ShowLoseAnimation()
        {
            AnsiConsole.Clear();

            var winText = new FigletText("LOSE ):")
                .Centered()
                .Color(Color.DarkSeaGreen2_1);

            AnsiConsole.Write(winText);

            Thread.Sleep(3000);

            AnsiConsole.Clear();

            CreateUI();
        }

    }
}
