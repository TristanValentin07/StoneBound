
public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 }

public static class GameSettings
{
    public static Difficulty difficulty = Difficulty.Medium;

    public static float TimerDuration(Difficulty d)
    {
        switch (d)
        {
            case Difficulty.Easy:   return 900f;
            case Difficulty.Medium: return 600f;
            case Difficulty.Hard:   return 400f;
            default: return 900f;
        }
    }

    public static float SpawnRate(Difficulty d)
    {
        switch (d)
        {
            case Difficulty.Easy:   return 5f;
            case Difficulty.Medium: return 3f;
            case Difficulty.Hard:   return 1f;
            default: return 3f;
        }
    }
    
    public static int MaxEnemies(Difficulty d)
    {
        switch (d)
        {
            case Difficulty.Easy:   return 15;
            case Difficulty.Medium: return 50;
            case Difficulty.Hard:   return 100;
            default: return 15;
        }
    }
}