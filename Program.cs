using System;

public class Program
{
    public static void Main()
    {
        int totalTargets = 5; // Загальна кількість ворожих цілей для збиття

        // Створюємо системи ППО з певними ймовірностями і загальною кількістю цілей
        C300_PPO c300System = new C300_PPO(totalTargets);
        D200_PPO d200System = new D200_PPO(totalTargets);

        // Виконуємо говоловний метод, де виконуються події збиття ворожих цілей і обчислення кількості збитих і не збитих
        c300System.Fire();
        d200System.Fire();

        // Виводимо статистику про те, скільки кожна система збила цілей
        Console.WriteLine($"Збито цілей {c300System.SystemName}: {c300System.TargetsHit}");
        Console.WriteLine($"Збито цілей {d200System.SystemName}: {d200System.TargetsHit}");

        // Обчислюємо, скільки цілей разом не було збито
        int targetsMissed = totalTargets - c300System.TargetsHit - d200System.TargetsHit;
        Console.WriteLine($"Не збито цілей разом: {targetsMissed}");
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
    private int probability; // Ймовірність збиття цілі при пострілі (від 1 до 100)
    private int availableTargets; // Доступна кількість цілей для обох систем
    private int targetsHit; // Лічильник збитих цілей
    private int targetsMissed; // Лічильник не збитих цілей

    public string SystemName { get; } // Назва системи ППО

    // Оголошення подій для збитих і не збитих цілей
    public event TargetHitEventHandler? TargetHit;
    public event TargetHitEventHandler? TargetMissed;

    public delegate void TargetHitEventHandler(object sender, TargetHitEventArgs e);

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

    // Конструктор для створення системи ППО з певною ймовірністю, загальною кількістю цілей та назвою
    public PPO_system(int probability, int totalTargets, string systemName)
    {
        Probability_Shoot = probability;
        availableTargets = totalTargets;
        SystemName = systemName;
    }

    // Властивість для доступу до лічильника збитих цілей
    public int TargetsHit => targetsHit;

    // Метод для обчислення збитих/не збитих цілей системою ППО
    public void Fire()
    {
        Random random = new Random(); 

        for (int i = 0; i < availableTargets; i++)
        {
            int randomValue = random.Next(1, 101); // ціль буде мати значення від 1 до 100

            if (randomValue <= Probability_Shoot) // якщо значення ворожої цілі менше ніж ймовірність збиття системою ППО 
            {
                OnTargetHit("Ціль збито системою ППО."); // Подія - ціль збита
                targetsHit++; // Збільшуємо лічильник збитих цілей
                availableTargets--; // Зменшуємо кількість доступних цілей
            }
            else
            {
                OnTargetMissed("Ціль не було збито системою ППО."); //Подія - ціль не збита
                targetsMissed++; // Збільшуємо лічильник не збитих цілей
            }
        }
    }

    // Метод для запуску події, коли ціль збито
    protected virtual void OnTargetHit(string message)
    {
        TargetHit?.Invoke(this, new TargetHitEventArgs(message, SystemName));
    }

    // Метод для запуску події, коли ціль не було збито
    protected virtual void OnTargetMissed(string message)
    {
        TargetMissed?.Invoke(this, new TargetHitEventArgs(message, SystemName));
    }
}


//Метод для передачі додаткової інформації під час сповіщення про події
public class TargetHitEventArgs : EventArgs
{
    public string Message { get; } // Повідомлення про результат (ціль збито чи не збито)
    public string SystemName { get; } // Назва системи ППО

    public TargetHitEventArgs(string message, string systemName)
    {
        Message = message;
        SystemName = systemName;
    }
}
