using System;

enum Shape { Rock, Paper, Scissors, Spock, Lizard }

class Program
{
    const int WinningScore = 3;
    static int playerScore = 0;
    static int agentScore = 0;
    static readonly Random rng = new Random();

    static void Main()
    {
        WriteIntro();

        while (playerScore < WinningScore && agentScore < WinningScore)
        {
            var player = ReadPlayerShape();
            var agent  = GetAgentShape();

            int result = Resolve(player, agent); // 1=player, -1=agent, 0=tie
            UpdateUi(player, agent, result);     // udskriver rundens resultat + score
        }

        Console.WriteLine(playerScore == WinningScore ? "\nYou WIN the match! 🏆"
                                                      : "\nAgent wins the match. 🤖");
    }

    static void WriteIntro()
    {
        Console.WriteLine("RPSSL — Rock Paper Scissors Spock Lizard");
        Console.WriteLine($"First to {WinningScore} wins.\nValid shapes: rock, paper, scissors, spock, lizard.");
        Console.WriteLine("Type 'q' to quit.\n");
    }

    // --- funktioner ---
    // 1) Afgørelseslogik via switch
    static int Resolve(Shape p, Shape a) => (p, a) switch
    {
        var t when t.p == t.a                          => 0,
        (Shape.Rock,     Shape.Scissors)               => 1,
        (Shape.Rock,     Shape.Lizard)                 => 1,
        (Shape.Paper,    Shape.Rock)                   => 1,
        (Shape.Paper,    Shape.Spock)                  => 1,
        (Shape.Scissors, Shape.Paper)                  => 1,
        (Shape.Scissors, Shape.Lizard)                 => 1,
        (Shape.Spock,    Shape.Scissors)               => 1,
        (Shape.Spock,    Shape.Rock)                   => 1,
        (Shape.Lizard,   Shape.Spock)                  => 1,
        (Shape.Lizard,   Shape.Paper)                  => 1,
        _                                               => -1
    };

    // 2) UI-opdatering efter hver runde
    static void UpdateUi(Shape player, Shape agent, int result)
    {
        if (result == 1)  playerScore++;
        if (result == -1) agentScore++;

        string outcome = result switch { 1 => "You win the round.",
                                         -1 => "Agent wins the round.",
                                          _ => "Tie." };

        Console.WriteLine($"You: {player} | Agent: {agent}  ->  {outcome}");
        Console.WriteLine($"Score  You {playerScore} : {agentScore} Agent\n");
    }

    // 3) Agentens tilfældige valg
    static Shape GetAgentShape()
    {
        int n = rng.Next(0, 5); // 0..4
        return (Shape)n;
    }

    // 4) Læs og parse spillerens input
    static Shape ReadPlayerShape()
    {
        while (true)
        {
            Console.Write("Your shape? ");
            string? s = Console.ReadLine();

            if (string.Equals(s, "q", StringComparison.OrdinalIgnoreCase))
                Environment.Exit(0);

            if (TryParseShape(s, out var shape)) return shape;

            Console.WriteLine("Invalid. Type: rock, paper, scissors, spock, lizard (or q to quit).");
        }
    }

    static bool TryParseShape(string? s, out Shape shape)
    {
        shape = Shape.Rock;
        if (s == null) return false;

        s = s.Trim().ToLowerInvariant();
        return s switch
        {
            "rock"     => (shape = Shape.Rock)     is Shape,
            "paper"    => (shape = Shape.Paper)    is Shape,
            "scissors" => (shape = Shape.Scissors) is Shape,
            "spock"    => (shape = Shape.Spock)    is Shape,
            "lizard"   => (shape = Shape.Lizard)   is Shape,
            _ => false
        };
    }
}
