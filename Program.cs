using System;

public class Program
{
    public static void Main()
    {
        C300_PPO c300System = new C300_PPO(60);
        D200_PPO d200System = new D200_PPO(40);

        c300System.Fire();
        d200System.Fire();

    }
}

public class C300_PPO  : PPO_system
{
    public C300_PPO (int probability) : base(60)
    {
    }
}

public class D200_PPO  : PPO_system
{
    public D200_PPO (int probability) : base(40)
    {
    }
}

public abstract class PPO_system
{
    private int probability; // Ймовірність збити ворожу ціль (%)
    public delegate void TargetHitEventHandler(object sender, EventArgs e); // делегат для обробки подій
    public event TargetHitEventHandler TargetHit; // Подія для "ціль збито"
    public event TargetHitEventHandler TargetMissed; // Подія для "ціль не збито"


    public int Probability_Shoot
    {
        get { return probability; }
        set
        {
            // Перевірка, щоб значення було в діапазоні від 1 до 100
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

    // Конструктор для ініціалізації ймовірності
    public PPO_system(int probability)
    {
        Probability_Shoot = probability; // визначаємо початковий % ймовирності попадання у ворожу ціль
    }

    public virtual void Fire()
    {
        // Використовуємо випадковий генератор для генерації випадкового числа від 1 до 100
        Random random = new Random();
        int randomValue = random.Next(1, 101);

        if (randomValue <= Probability_Shoot)
        {
            Console.WriteLine("Ціль збито системою ППО.");
            OnTargetHit();
        }
        else
        {
            Console.WriteLine("Ціль не було збито системою ППО.");
            OnTargetMissed();
        }
    }

    protected virtual void OnTargetHit()
    {
        TargetHit?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnTargetMissed()
    {
        TargetMissed?.Invoke(this, EventArgs.Empty);
    }
}
