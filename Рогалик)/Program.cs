using System;

class Program
{
    static void Main(string[] args)
    {
        Random rnd = new Random();

        // Создание игрока
        Console.WriteLine("Добро пожаловать, воин!");
        Console.WriteLine("Придумай себе имя:");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName);

        // Инициализация оружия и аптечки
        player.Weapon = new Weapon("Фламберг", 52, 100);
        player.AidPack = new Aid("Средняя аптечка", 20);

        Console.WriteLine($"Теперь тебя зовут {player.Name}!");
        Console.WriteLine($"Вам был ниспослан меч {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.AidPack.Name} ({player.AidPack.Recovery}hp).");
        Console.WriteLine($"У Вас {player.Health}hp.");

        // Основной игровой цикл
        while (player.IsAlive)
        {
            Enemy enemy = GenerateEnemy(rnd);
            Console.WriteLine($"{player.Name} встречает врага {enemy.Name} ({enemy.Health}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");
            player.Fight(enemy);
        }

        Console.WriteLine("Игра окончена!");
    }

    static Enemy GenerateEnemy(Random rnd)
    {
        string[] enemyNames = { "Варвар", "Орк", "Тролль", "Скелет", "Гоблин" };
        int health = rnd.Next(30, 100);
        return new Enemy(enemyNames[rnd.Next(0, enemyNames.Length)], health);
    }
}

class Player
{
    public string Name { get; private set; }
    public int Health { get; set; }
    public int MaxHealth { get; private set; }
    public int Points { get; private set; }
    public Aid AidPack { get; set; }
    public Weapon Weapon { get; set; }

    public bool IsAlive => Health > 0;

    public Player(string name)
    {
        Name = name;
        Health = 100;
        MaxHealth = 100;
        Points = 0;
    }

    public void Fight(Enemy enemy)
    {
        while (IsAlive && enemy.IsAlive)
        {
            Console.WriteLine("Что Вы будете теперь делать?");
            Console.WriteLine("1. Ударить");
            Console.WriteLine("2. Пропустить ход");
            Console.WriteLine("3. Использовать аптечку");

            int action = int.Parse(Console.ReadLine());

            switch (action)
            {
                case 1:
                    Attack(enemy);
                    if (enemy.IsAlive)
                    {
                        enemy.Attack(this);
                    }
                    break;
                case 2:
                    Console.WriteLine($"{Name} пропустил ход.");
                    if (enemy.IsAlive)
                    {
                        enemy.Attack(this);
                    }
                    break;
                case 3:
                    UseAidPack();
                    if (enemy.IsAlive)
                    {
                        enemy.Attack(this);
                    }
                    break;
            }
        }

        if (!enemy.IsAlive)
        {
            Points += 10;
            Console.WriteLine($"{Name} победил врага {enemy.Name} и заработал 10 очков!");
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.Health -= Weapon.Damage;
        Console.WriteLine($"{Name} ударил противника **{enemy.Name}**");
        Console.WriteLine($"У противника {enemy.Health}hp, у Вас {Health}hp");
    }

    private void UseAidPack()
    {
        if (Health < MaxHealth)
        {
            Health += AidPack.Recovery;
            if (Health > MaxHealth)
                Health = MaxHealth;

            Console.WriteLine($"{Name} использовал аптечку");
            Console.WriteLine($"У противника -- за Вас {Health}hp");
        }
        else
        {
            Console.WriteLine($"{Name}, у Вас полное здоровье. Аптечка не может быть использована.");
        }
    }
}

class Enemy
{
    public string Name { get; private set; }
    public int Health { get; set; }
    public Weapon Weapon { get; private set; }

    public bool IsAlive => Health > 0;

    public Enemy(string name, int health)
    {
        Name = name;
        Health = health;
        Weapon = new Weapon("Экскалибур", 40, 100);
    }

    public void Attack(Player player)
    {
        player.Health -= Weapon.Damage;
        Console.WriteLine($"Противник **{Name}** ударил вас!");
        Console.WriteLine($"У противника {Health}hp, у Вас {player.Health}hp");
    }
}

class Aid
{
    public string Name { get; private set; }
    public int Recovery { get; private set; }

    public Aid(string name, int recovery)
    {
        Name = name;
        Recovery = recovery;
    }
}

class Weapon
{
    public string Name { get; private set; }
    public int Damage { get; private set; }
    public int Durability { get; private set; }

    public Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
    }
}

