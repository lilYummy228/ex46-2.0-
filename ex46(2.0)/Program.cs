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
                new Warrior("Воин", 1000, 100, random.Next(0, 100)),
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

                    firstFighter.ShowCurrentHealth();
                    firstFighter.UseAbility();
                    firstFighter.TakeDamage(secondFighter.Damage);
                    firstFighter.ShowRecievedDamage(secondFighter.Damage);

                    Console.WriteLine("\n\n");
                    secondFighter.ShowCurrentHealth();
                    secondFighter.UseAbility();
                    secondFighter.TakeDamage(firstFighter.Damage);
                    secondFighter.ShowRecievedDamage(firstFighter.Damage);
                    Console.WriteLine();

                    if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth <= 0)
                        Console.WriteLine("Ничья");
                    else if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth > 0)
                        Console.WriteLine($"Победа за {secondFighter.Name}ом");
                    else if (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth <= 0)
                        Console.WriteLine($"Победа за {firstFighter.Name}ом");

                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Двух одиннаковых бойцов выбрать нельзя");
            }


            Console.ReadKey();
            Console.Clear();
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
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}");
        }

        public void ShowRecievedDamage(int damage)
        {
            Console.WriteLine($"Полученный урон - {damage}");
        }

        public virtual void UseAbility() { }

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
            _lifesteal = _random.Next(10, 30);
            CurrentHealth += _lifesteal;
            Console.WriteLine($"Здоровья получено: {_lifesteal}");
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} имеет способность высасывать жизненную энергию из врага при нанесении урона\n");
        }

        public override void UseAbility()
        {
            StealLife();
        }
    }

    class Rogue : Fighter
    {
        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            InitialDamage = Damage;
        }

        public int InitialDamage { get; private set; }

        public void DealCriticalDamage()
        {
            int initialDamage = InitialDamage;
            Damage = initialDamage;
            Random random = new Random();
            int chance = random.Next(5); //20%
            int critDamage = Damage * 3;

            if (chance == 0)
                Damage = critDamage;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} часто опережает своего противника, получая преимущество в бою с ними в виде шанса на нанесение двойного урона при атаке\n");
        }

        public override void UseAbility()
        {
            DealCriticalDamage();
        }
    }

    class Warrior : Fighter
    {
        public Warrior(string name, int health, int damage, int armor) : base(name, health, damage)
        {
            Armor = armor;
        }

        public int Armor { get; private set; }

        public void BlockDamage()
        {

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Благодаря своим тяжелым доспехам и военному мастерству, {Name} меньше получает урона от противников\n");
        }

        public override void UseAbility()
        {
            BlockDamage();
        }
    }

    class Paladin : Fighter
    {
        public Paladin(string name, int health, int damage) : base(name, health, damage) { }

        public void EvadeDamage()
        {
            Random random = new Random();
            int chance = random.Next(20); //5%

            if (chance == 0)
            {

            }

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} имеет шанс полностью заблокировать атаку противника благодаря божественному щиту\n");
        }

        public override void UseAbility()
        {
            EvadeDamage();
        }
    }

    class Mage : Fighter
    {
        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            HitCount = 0;
        }

        public int HitCount { get; private set; }

        public void BurnEnemy()
        {
            HitCount++;

            for (int i = 0; i < HitCount; i++)
                Damage += Damage / 10;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} способен держать в страхе своих врагов, не желающих быть сожженными дотла\n");
        }

        public override void UseAbility()
        {
            BurnEnemy();
        }
    }

    class Hunter : Fighter
    {
        public Hunter(string name, int health, int damage) : base(name, health, damage) { }

        public void HitToWounds()
        {

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"У {Name}а меткий не только выстрел, но и взор, благодаря которому он может находить слабые места противников и пользоваться этим\n");
        }

        public override void UseAbility()
        {
            HitToWounds();
        }
    }

    class Shaman : Fighter
    {
        public Shaman(string name, int health, int damage) : base(name, health, damage) { }

        public void GetSpontaneousEffect()
        {
            Random random = new Random();
            int chance = random.Next(4); //25%
            int randomValue = random.Next(MaxHealth / 20, MaxHealth / 10);

            if (chance == 0) { }

            else if (chance == 1) { }

            else if (chance == 2) { }

            else if (chance == 3) { }

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Силы стихий часто несут за собой такие же стихийные последствия для {Name}а, чем он и пользуется \n");
        }

        public override void UseAbility()
        {
            GetSpontaneousEffect();
        }
    }

    class Druid : Fighter
    {
        public Druid(string name, int health, int damage) : base(name, health, damage) { }

        public void HealYourself()
        {

        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Связь {Name}а с лесом помогает ему оставаться невредимым даже на поле сражений\n");
        }

        public override void UseAbility()
        {
            HealYourself();
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

        public override void UseAbility()
        {
            RiseAgain();
        }
    }
}
