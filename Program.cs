using System;

public class Program
{
    public static void Main()
    {
        int N = 5; // Загальна кількість ворожих цілей для збиття

        C300_PPO c300System = new C300_PPO(N); // probability буде 60 за умовчуванням
        D200_PPO d200System = new D200_PPO(N); // probability буде 40 за умовчуванням

        c300System.Fire();
        d200System.Fire();

        c300System.PrintTargetStatistics();
        d200System.PrintTargetStatistics();
    }
}

public class C300_PPO : PPO_system
{
    public C300_PPO(int totalTargets) : base(60, totalTargets, "C300_PPO")
    {
    }
}

public class D200_PPO : PPO_system
{
    public D200_PPO(int totalTargets) : base(40, totalTargets, "D200_PPO")
    {
    }
}

public abstract class PPO_system
{
    private int probability;
    public int TotalTargets { get; set; } // Загальна кількість цілей для збиття
    private int targetsHit; // кількість збитих цілей
    private int targetsMissed; // кількість не збитих цілей

    public string SystemName { get; }

    public int Probability_Shoot
    {
        get { return probability; }
        set
        {
            if (value >= 1 && value <= 100)
            {
                probability = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Ймовірність має бути в діапазоні від 1 до 100.");
            }
        }

    }

    public PPO_system(int probability, int totalTargets, string systemName)
    {
        Probability_Shoot = probability;
        TotalTargets = totalTargets;
        SystemName = systemName;
    }

    // Оголошення подій
    public event TargetHitEventHandler? TargetHit;
    public event TargetHitEventHandler? TargetMissed;

    public delegate void TargetHitEventHandler(object sender, TargetHitEventArgs e);

    public virtual void Fire()
    {
        Random random = new Random();

        for (int i = 0; i < TotalTargets; i++)
        {
            if (targetsHit >= TotalTargets)
            {
                // Всі цілі вже збиті, виходимо з циклу
                break;
            }

            int randomValue = random.Next(1, 101);

            if (randomValue <= Probability_Shoot)
            {
                OnTargetHit("Ціль збито системою ППО.");
                targetsHit++; // Збільшуємо лічильник збитих цілей
            }
            else
            {
                OnTargetMissed("Ціль не було збито системою ППО.");
                targetsMissed++; // Збільшуємо лічильник не збитих цілей
            }
        }
    }

    protected virtual void OnTargetHit(string message)
    {
        TargetHit?.Invoke(this, new TargetHitEventArgs(message, SystemName));
    }

    protected virtual void OnTargetMissed(string message)
    {
        TargetMissed?.Invoke(this, new TargetHitEventArgs(message, SystemName));
    }

    public void PrintTargetStatistics()
    {
        Console.WriteLine($"Збито цілей {SystemName}: {targetsHit}");
        Console.WriteLine($"Не збито цілей {SystemName}: {targetsMissed}");
    }
}

public class TargetHitEventArgs : EventArgs
{
    public string Message { get; }
    public string SystemName { get; }

    public TargetHitEventArgs(string message, string systemName)
    {
        Message = message;
        SystemName = systemName;
    }
}
