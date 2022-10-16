using Racer.Utilities;

internal class Metrics
{
    public const float InactiveDelay = 1.25f;
    public const float TouchRadius = .5f;
    public const int MaxSearchCount = 500;


    // Animator Id's
    public static int X = Utility.GetAnimId("X");
    public static int O = Utility.GetAnimId("O");
    public static int Win = Utility.GetAnimId("Win");
    public static int Draw = Utility.GetAnimId("Draw");
    public static int Fade = Utility.GetAnimId("Fade");

}