using Racer.SaveManager;
using static Racer.Utilities.SingletonPattern;

internal class ScoreManager : Singleton<ScoreManager>
{
    protected override void Awake()
    {
        base.Awake();

        LoadScore();
    }

    public void SetXCount(int value)
    {
        GetXCount += value;

        SaveManager.SaveInt("X", GetXCount);
    }

    public void SetOCount(int value)
    {
        GetOCount += value;

        SaveManager.SaveInt("O", GetOCount);
    }

    public int GetXCount { get; private set; }

    public int GetOCount { get; private set; }


    private void LoadScore()
    {
        GetXCount = SaveManager.GetInt("X");
        GetOCount = SaveManager.GetInt("O");
    }

    public void ClearData()
    {
        SaveManager.ClearAllPrefs();
    }
}
