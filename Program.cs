using System;

public class Program
{
    public static void Main()
    {
        int enemy = -10; // Початкове значення ймовірності

        // Створюємо об'єкт класу PPO_target і передаємо значення в конструктор
        PPO_target target = new PPO_target(enemy);

        // Отримуємо значення з об'єкта та виводимо його на консоль
        int probabilityValue = target.Probability_Shoot;
        Console.WriteLine("Ймовірність: " + probabilityValue);
    }
}

public class PPO_target : PPO_system
{
    public PPO_target(int probability) : base(probability)
    {
    }

    public override void PerformAction()
    {
        // Реалізація дії в похідному класі
    }
}

public abstract class PPO_system
{
    private int probability; // Ймовірність збити ворожу ціль (%)

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

    // Абстрактний метод для реалізації в похідних класах
    public abstract void PerformAction();
}
