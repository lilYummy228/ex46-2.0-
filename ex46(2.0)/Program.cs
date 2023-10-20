using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ex46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();

            arena.ShowAllFighters();
            arena.Fight();
        }
    }

    class Arena
    {
        private List<Fighter> _fighters;

        public Arena()
        {
            Random random = new Random();
            _fighters = new List<Fighter>
            {
                new Warlock("Чернокнижник", 1000, 100),
                new Rogue("Разбойник", 1000, 100),
                new Warrior("Воин", 1000, 100, random.Next(10, 25)),
                new Paladin("Паладин", 1000, 100),
                new Mage("Маг", 1000, 100),
                new Hunter("Охотник", 1000, 100),
                new Shaman("Шаман", 1000, 100),
                new Druid("Друид", 1000, 100),
                new Priest("Жрец", 1000, 100)
            };
        }

        public void ShowAllFighters()
        {
            int index = 1;

            foreach (Fighter fighter in _fighters)
            {
                Console.Write($"{index}. ");
                fighter.ShowStats();
                index++;
            }
        }

        public void Fight()
        {
            Console.CursorVisible = false;
            Fighter firstFighter = ChooseFirstFighter();
            Fighter secondFighter = ChooseSecondFighter();

            if (firstFighter != secondFighter)
            {
                Console.Clear();
                firstFighter.ShowCurrentHealth();
                Console.WriteLine("\nПротив\n");
                secondFighter.ShowCurrentHealth();
                Console.WriteLine("\nНажмите любую клавишу чтобы начать битву...");
                Console.ReadKey();

                while (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth > 0)
                {
                    Console.Clear();

                    DrawBorder();
                    firstFighter.ShowCurrentHealth();
                    firstFighter.UseAbility(secondFighter);
                    DrawBorder();

                    Console.WriteLine();

                    DrawBorder();
                    secondFighter.ShowCurrentHealth();
                    secondFighter.UseAbility(firstFighter);
                    DrawBorder();

                    Console.WriteLine("\nНажмите любую клавишу чтобы сделать удар...");
                    Console.ReadKey();
                    firstFighter.TakeDamage(secondFighter.Damage);
                    secondFighter.TakeDamage(firstFighter.Damage);
                }

                DetermineWinner(firstFighter, secondFighter);
            }
            else
            {
                Console.WriteLine("Двух одиннаковых бойцов выбрать нельзя");
            }

            Console.CursorVisible = true;
            Console.ReadKey();
            Console.Clear();
        }

        private void DrawBorder()
        {
            Console.WriteLine($"{new string('-', 20)}");
        }

        private void DetermineWinner(Fighter firstFighter, Fighter secondFighter)
        {
            Console.Clear();
            DrawBorder();
            firstFighter.ShowCurrentHealth();
            DrawBorder();
            Console.WriteLine();
            DrawBorder();
            secondFighter.ShowCurrentHealth();
            DrawBorder();
            Console.WriteLine();

            if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth <= 0)
                Console.WriteLine("Ничья");
            else if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth > 0)
                Console.WriteLine($"Победа за {secondFighter.Name}ом");
            else if (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth <= 0)
                Console.WriteLine($"Победа за {firstFighter.Name}ом");
        }

        private Fighter ChooseFirstFighter()
        {
            Console.Write("Выберете первого бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter firstFighter = _fighters[fighterIndex - 1];
                return firstFighter;
            }

            return null;
        }

        private Fighter ChooseSecondFighter()
        {
            Console.Write("Выберете второго бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter secondFighter = _fighters[fighterIndex - 1];
                return secondFighter;
            }

            return null;
        }

    }

    class Fighter
    {
        public Fighter(string name, int health, int damage)
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            Damage = damage;
        }

        public string Name { get; protected set; }
        public int CurrentHealth { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Damage { get; protected set; }

        public virtual void ShowStats()
        {
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}\nУрон: {Damage}");
        }

        public void ShowCurrentHealth()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{Name}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Здоровье: {CurrentHealth}");
            Console.ForegroundColor = defaultColor;
        }

        public virtual void UseAbility(Fighter enemyFighter) { }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }

    class Warlock : Fighter
    {
        private Random _random = new Random();
        private int _lifesteal;

        public Warlock(string name, int health, int damage) : base(name, health, damage) { }

        public void StealLife()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            _lifesteal = _random.Next(10, 41);
            CurrentHealth += _lifesteal;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Кража здоровья");
            Console.ForegroundColor = defaultColor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} имеет способность высасывать жизненную энергию из врага при нанесении урона\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            StealLife();
        }
    }

    class Rogue : Fighter
    {
        private int _initialDamage;

        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
        }

        public void DealCriticalDamage()
        {
            Random random = new Random();
            Damage = _initialDamage;
            int chance = random.Next(0, 4); //25%
            int critDamage = Damage * 2;

            if (chance == 0)
            {
                Damage = critDamage;
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Критический урон!");
                Console.ForegroundColor = defaultColor;
            }
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} часто опережает своего противника, получая преимущество в бою с ними в виде шанса на нанесение двойного урона при атаке\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            DealCriticalDamage();
        }
    }

    class Warrior : Fighter
    {
        private int _armor;

        public Warrior(string name, int health, int damage, int armor) : base(name, health, damage)
        {
            _armor = armor;
        }

        public void BlockDamage()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Каменная кожа");
            CurrentHealth += _armor;
            Console.ForegroundColor = defaultColor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Благодаря своей толстой коже, {Name} меньше получает урона от противников\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            BlockDamage();
        }
    }

    class Paladin : Fighter
    {
        public Paladin(string name, int health, int damage) : base(name, health, damage) { }

        public void HealYourself()
        {
            Random random = new Random();
            int chance = random.Next(0, 4);

            if (chance == 0)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Священная милость");
                Console.ForegroundColor = defaultColor;
                CurrentHealth += 50;
            }
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} освящает себя, получая благословение и исцеляя себя\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            HealYourself();
        }
    }

    class Mage : Fighter
    {
        private int _hitCount;
        private int _initialDamage;

        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            _hitCount = 0;
            _initialDamage = Damage;
        }

        public void BurnEnemy()
        {
            Damage = _initialDamage;
            _hitCount++;

            for (int i = 0; i < _hitCount; i++)
                Damage += 7;

            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"Мороз по коже");
            Console.ForegroundColor = defaultColor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} ловко обращается с магией льда, что может одним лишь холодным взглядом остудить пыл своих врагов\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            BurnEnemy();
        }
    }

    class Hunter : Fighter
    {
        private int _initialDamage;

        public Hunter(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
        }

        public void SummonWolf()
        {
            Damage = _initialDamage;

            if (CurrentHealth <= MaxHealth / 2)
            {

                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Призыв волка");
                Console.ForegroundColor = ConsoleColor.Green;
                Wolf wolf = new Wolf("Волк", 100, 50);


                Console.WriteLine();
                wolf.ShowStats();
                Damage += wolf.Damage;

                Console.ForegroundColor = defaultColor;
            }
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"У {Name}а есть верный компаньон, горный волк, готовый растерзать противников своего хозяина \n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            SummonWolf();
        }

        class Wolf : Fighter
        {
            public Wolf(string name, int health, int damage) : base(name, health, damage) { }

            public override void ShowStats()
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Name}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Здоровье: {CurrentHealth}");
                Console.ForegroundColor = defaultColor;
            }
        }
    }

    class Shaman : Fighter
    {
        private int _initialDamage;

        public Shaman(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
        }

        public void GetSpontaneousEffect(Fighter enemyFighter)
        {
            Damage = _initialDamage;
            Random random = new Random();
            int chance = random.Next(0, 4); //25%
            ConsoleColor defaultColor = Console.ForegroundColor;

            if (chance == 0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Попутный ветер");
                CurrentHealth += enemyFighter.Damage;
            }
            else if (chance == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Жар солнца");
                Damage += random.Next(50, 101);
                CurrentHealth -= Damage;
            }
            else if (chance == 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Целительный дождь");
                CurrentHealth += random.Next(50, 76);
            }
            else if (chance == 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Защита флоры");
                Damage -= random.Next(25, 101);
                CurrentHealth += random.Next(0, 101);
            }

            Console.ForegroundColor = defaultColor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Силы стихий часто несут за собой такие же стихийные последствия для {Name}а, чем он и пользуется \n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            GetSpontaneousEffect(enemyFighter);
        }
    }

    class Druid : Fighter
    {
        public Druid(string name, int health, int damage) : base(name, health, damage) { }

        public void TurnIntoBeast()
        {

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Связь {Name}а с лесом помогает ему оставаться невредимым даже на поле сражений\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            TurnIntoBeast();
        }
    }

    class Priest : Fighter
    {
        public Priest(string name, int health, int damage) : base(name, health, damage) { }

        public void RiseAgain()
        {

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Благородный {Name}, примкнувший к силам света, никогда не оставит ни себя, ни своих союзников умирать на поле боя\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            RiseAgain();
        }
    }
}
