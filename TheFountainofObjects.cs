var size = WorldSize();
FountainOfObjects fountainOfObjects = new FountainOfObjects(new Grid(size.X , size.Y));
fountainOfObjects.Start();
(int X, int Y) WorldSize()
{
    Console.Write("""
    Small -> 4*4 World
    Medium -> 6*6 World
    Large -> 8*8 World

    Choose The World Size: 
    """);
    var Coordinate = Console.ReadLine()!.ToLower() switch
    {
        "small" => (4, 4),
        "medium" => (6, 6),
        "large" => (8, 8)
    };
    Console.Clear();
    return Coordinate;
}

class FountainOfObjects
{
    Grid Grid;
    Player Player;
    public void Start()
    {
        InitializeGame();
        while (true)
        {
            if (Player.PlayerhasWon == true || Player.PlayerhasWon == false) return;  // checking Winning condititon(Temporary hardcoded reason) 
            Color.Write($"You are in the room at {Player.PlayerCurrentPosition}", ColorOptions.Neutral);
            Console.Write("What do you want to do?: ");
            Player.Command(Console.ReadLine()!, Grid);
        }
        void InitializeGame()
        {
            Grid.MakingOriginalGrid();
            Color.Write("You see light coming from the cavern entrance", ColorOptions.SunShine);
            //  Grid.DangerCoordinate((2, 1)); CAN USE LATER
        }
    }
    public FountainOfObjects(Grid grid)
    {
        Grid = grid; 
        Player = new Player(Grid);
    }
}
class Player
{
    Grid Grid { get; set; }
    private bool IsFountainEnable { get; set; }
    public bool? PlayerhasWon { get; private set; } = null; // can use enums here 
    public (int X, int Y) PlayerCurrentPosition;

    public void Command(string command, Grid grid )
    {

        if (command == "move north") { PlayerCurrentPosition = (PlayerCurrentPosition.X + 1, PlayerCurrentPosition.Y); }
        else if (command == "move south") { PlayerCurrentPosition = (PlayerCurrentPosition.X - 1, PlayerCurrentPosition.Y); }
        else if (command == "move east") { PlayerCurrentPosition = (PlayerCurrentPosition.X, PlayerCurrentPosition.Y + 1); }
        else if (command == "move west") { PlayerCurrentPosition = (PlayerCurrentPosition.X, PlayerCurrentPosition.Y - 1); }

        if(command == "enable fountain" && PlayerCurrentPosition == grid.FountainOfObjects()) { IsFountainEnable = true; }
        Console.WriteLine("-------------------------------");
        Rules(Grid, this);
        if(command == "enable fountain" && PlayerCurrentPosition != grid.FountainOfObjects()) 
        {
            Color.Write("Player Must In Fountain Room to enable Fountain", ColorOptions.Alert);
        }
    }

    public static void Rules(Grid grid, Player player) 
    {
        // Cavern Entrance
        if (player.PlayerCurrentPosition == (0, 0) && !player.IsFountainEnable) Color.Write("You see light coming from the cavern entrance" , ColorOptions.SunShine);
        IsDanger();
        Fountain();
        Winning();
        

        void IsDanger()
        {
            // If the player's current position is a danger coordinate, they are dead.
            if (grid.CoordinatesList.Contains(player.PlayerCurrentPosition) == false) { player.PlayerhasWon = false;Color.Write("You Died\r\n\"Reason: Player Went Out Of Map\"", ColorOptions.Goodbye); }

            // Check adjacent cells to see if any are danger zones
            if (
                grid.CoordinatesList.Contains((player.PlayerCurrentPosition.X + 1, player.PlayerCurrentPosition.Y)) == false ||
                grid.CoordinatesList.Contains((player.PlayerCurrentPosition.X - 1, player.PlayerCurrentPosition.Y)) == false ||
                grid.CoordinatesList.Contains((player.PlayerCurrentPosition.X, player.PlayerCurrentPosition.Y + 1)) == false ||
                grid.CoordinatesList.Contains((player.PlayerCurrentPosition.X, player.PlayerCurrentPosition.Y - 1)) == false)
            {
                // return "Warning: Near danger zone:"; // FOR LATER USE..
            }
        }
        
        void Fountain()
        {
            // Enable Fountain Of Objects
            if (player.PlayerCurrentPosition == grid.FountainOfObjects() && player.IsFountainEnable) Color.Write("You hear the rushing waters from the Fountain of Objects. It has been reactivated!", ColorOptions.Success);

            // Room Fountain Of Objects
            else if (player.PlayerCurrentPosition == grid.FountainOfObjects())
            {
                Color.Write("You hear Water Dripping in this room. The Fountain of Objects is here! ", ColorOptions.Water);
            }
        }
        
        void Winning()
        {
            // Winning
            if (player.PlayerCurrentPosition == (0, 0) && player.IsFountainEnable)
            {
                player.PlayerhasWon = true;
                Color.Write("The Fountain of Objects has been reactivated, and you have escaped with your life! \r\nYou win!", ColorOptions.Success);
            }
        }
        
    }

    public Player(Grid grid)
    {
        Grid = grid;
        PlayerCurrentPosition = (0, 0); // Starting position
    }
}

class Grid
{
    int RowSize { get; set; }
    int ColumnSize { get; set; }
    int[,] GridMatrix { get; set; }
   public List<(int X, int Y)> CoordinatesList = new List<(int, int)>();
    int CurrentRow { get; set; }
    int CurrentColumb { get; set; }

    public void RemoveCoordinate((int, int) dangerCoordinate)
    {
        CoordinatesList.Remove(dangerCoordinate); // Removes the first occurrence of the specified item
    }
    public (int, int) FountainOfObjects() => (0, 2);
    public void MakingOriginalGrid()
    {
        for ( CurrentColumb = 0 ;  CurrentColumb < GridMatrix.GetLength(0); CurrentColumb++)
        {
            for (CurrentRow = 0 ;  CurrentRow < GridMatrix.GetLength(1); CurrentRow++)
            {
                var Coordinates = (CurrentColumb, CurrentRow);
                CoordinatesList.Add(Coordinates);
               // Console.Write(Coordinates); TO SHOW GRID (Helpful for Debbugging)
            }
            // Console.WriteLine(); // New line after each row
        }
        Console.WriteLine("--------------------------------------------");
        // Console.WriteLine();
    }
    public Grid(int row, int col)
    {
        RowSize = row;
        ColumnSize = col;
        GridMatrix = new int[RowSize, ColumnSize]; // Initialize with default values (0)
    }
}

class Color 
{
    public static void Write(string message , ColorOptions color) 
    {
        Console.ForegroundColor = color switch
        {
            ColorOptions.Alert => ConsoleColor.Red,
            ColorOptions.Water => ConsoleColor.Blue,
            ColorOptions.SunShine => ConsoleColor.Yellow,
            ColorOptions.Success => ConsoleColor.Green,
            ColorOptions.Goodbye => ConsoleColor.Magenta,
            ColorOptions.Neutral => ConsoleColor.Cyan,
            _ => ConsoleColor.White,
        };
        Console.WriteLine(message);
        Default();
    }
    private static void Default() { Console.ForegroundColor = ConsoleColor.White; } 
}
enum ColorOptions{Success, Alert, Neutral, Goodbye, Water, SunShine}