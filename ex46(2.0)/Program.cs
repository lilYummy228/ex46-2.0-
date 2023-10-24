using System;
using System.Collections.Generic;

namespace ex46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            bool isOpen = true;

            while (isOpen)
            {
                Fighter firstFighter = arena.ChooseFighters();
                Fighter secondFighter = arena.ChooseFighters();
                Fighter winner = arena.Fight(firstFighter, secondFighter);
                arena.DetermineWinner(winner);
                isOpen = arena.IsEmpty();

                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Arena
    {
        private List<Fighter> _fighters;

        public Arena()
        {
            _fighters = new List<Fighter>
            {
                CreateWarlock(),
                CreateRogue(),
                CreateWarrior(),
                CreatePaladin(),
                CreateMage(),
                CreateHunter(),
                CreateShaman(),
                CreateDruid(),
                CreatePriest()
            };
        }

        public Fighter ChooseFighters()
        {
            bool isChosen = false;

            while (isChosen == false)
            {
                ShowAllFighters();
                isChosen = TryToGetFighter(out Fighter fighter);

                if (isChosen)
                {
                    _fighters.Remove(fighter);
                    isChosen = true;
                    return fighter;
                }
                else
                {
                    Console.WriteLine("Такого бойца нет...");
                    Console.ReadKey();
                }
            }

            return null;
        }

        public Fighter Fight(Fighter firstFighter, Fighter secondFighter)
        {
            Console.CursorVisible = false;
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
                firstFighter.UseAbility(secondFighter);
                DrawBorder();

                Console.WriteLine("\nПротив\n");

                DrawBorder();
                secondFighter.ShowCurrentHealth();
                secondFighter.UseAbility(firstFighter);

                Console.WriteLine("\nНажмите любую клавишу чтобы сделать удар...");
                Console.ReadKey();

                firstFighter.TakeDamage(secondFighter.Damage);
                secondFighter.TakeDamage(firstFighter.Damage);
            }

            Fighter winner = FindSurvivor(firstFighter, secondFighter);
            return winner;
        }       

        public void DetermineWinner(Fighter winner)
        {
            Console.Clear();

            if (winner != null)
            {
                DrawBorder();
                winner.ShowCurrentHealth();
                DrawBorder();

                Console.WriteLine($"Победа за {winner.Name}ом");
            }
            else
            {
                Console.WriteLine("Оба бойца погибли...");
            }

            Console.CursorVisible = true;
        }

        public bool IsEmpty()
        {
            int lastFighter = 1;

            if (GetFightersCount() <= lastFighter)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetFightersCount()
        {
            return _fighters.Count;
        }

        private void ShowAllFighters()
        {
            Console.Clear();
            int index = 1;

            foreach (Fighter fighter in _fighters)
            {
                Console.Write($"{index}. ");
                fighter.ShowStats();
                index++;
            }
        }

        private void DrawBorder()
        {
            Console.WriteLine($"{new string('-', 20)}");
        }

        private Fighter FindSurvivor(Fighter firstFighter, Fighter secondFighter)
        {
            if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth <= 0)
            {
                Console.WriteLine("Ничья");
                return null;
            }
            else if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth > 0)
            {
                Console.WriteLine($"Победа за {secondFighter.Name}ом");
                return secondFighter;
            }
            else if (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth <= 0)
            {
                Console.WriteLine($"Победа за {firstFighter.Name}ом");
                return firstFighter;
            }
            else
            {
                return null;
            }
        }

        private bool TryToGetFighter(out Fighter fighter)
        {
            Console.Write("Выберете бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                if (fighterIndex <= _fighters.Count && fighterIndex > 0)
                {
                    fighter = _fighters[fighterIndex - 1];
                    return true;
                }
                else
                {
                    fighter = null;
                    return false;
                }
            }
            else
            {
                fighter = null;
                return false;
            }
        }

        private Fighter CreateWarlock()
        {
            string name = "Чернокнижник";
            int health = 1000;
            int damage = 100;
            return new Warlock(name, health, damage);
        }

        private Fighter CreateRogue()
        {
            string name = "Разбойник";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Rogue(name, health, damage);
            return fighter;
        }

        private Fighter CreateWarrior()
        {
            Random random = new Random();
            string name = "Воин";
            int health = 1000;
            int damage = 100;
            int minArmorValue = 10;
            int maxArmorValue = 25;
            Fighter fighter = new Warrior(name, health, damage, random.Next(minArmorValue, maxArmorValue));
            return fighter;
        }

        private Fighter CreatePaladin()
        {
            string name = "Паладин";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Paladin(name, health, damage);
            return fighter;
        }

        private Fighter CreateMage()
        {
            string name = "Маг";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Mage(name, health, damage);
            return fighter;
        }

        private Fighter CreateHunter()
        {
            string name = "Охотник";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Hunter(name, health, damage);
            return fighter;
        }

        private Fighter CreateShaman()
        {
            string name = "Шаман";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Shaman(name, health, damage);
            return fighter;
        }

        private Fighter CreateDruid()
        {
            string name = "Друид";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Druid(name, health, damage);
            return fighter;
        }

        private Fighter CreatePriest()
        {
            string name = "Жрец";
            int health = 1000;
            int damage = 100;
            Fighter fighter = new Priest(name, health, damage);
            return fighter;
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
        private int _minLifesteal = 10;
        private int _maxLifesteal = 31;

        public Warlock(string name, int health, int damage) : base(name, health, damage) { }

        public void StealLife()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            _lifesteal = _random.Next(_minLifesteal, _maxLifesteal);
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
        private int _critDamageChance;

        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
            _critDamageChance = 4;
        }

        public void DealCriticalDamage()
        {
            Random random = new Random();
            Damage = _initialDamage;
            int critDamageMultiplier = 2;
            int chance = random.Next(_critDamageChance);
            int critDamage = Damage * critDamageMultiplier;

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
        private int _healingChance;
        private int _healingValue;

        public Paladin(string name, int health, int damage) : base(name, health, damage)
        {
            _healingChance = 2;
            _healingValue = 25;
        }

        public void HealYourself()
        {
            Random random = new Random();
            int chance = random.Next(_healingChance);

            if (chance == 0)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Священная милость");
                Console.ForegroundColor = defaultColor;
                CurrentHealth += _healingValue;
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
        private int _periodicDamage;

        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            _hitCount = 0;
            _initialDamage = Damage;
            _periodicDamage = 7;
        }

        public void BurnEnemy()
        {
            Damage = _initialDamage;
            _hitCount++;

            for (int i = 0; i < _hitCount; i++)
                Damage += _periodicDamage;

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
        private string _wolfName;
        private int _wolfHealth;
        private int _wolfDamage;
        private int _minWolfDamage;
        private int _maxWolfDamage;

        public Hunter(string name, int health, int damage) : base(name, health, damage)
        {
            Random random = new Random();
            _initialDamage = Damage;
            _wolfHealth = 100;
            _minWolfDamage = 50;
            _maxWolfDamage = 81;
            _wolfDamage = random.Next(_minWolfDamage, _maxWolfDamage);
            _wolfName = "Волк";
        }

        public void SummonWolf()
        {
            Damage = _initialDamage;
            int halfHealth = MaxHealth / 2;

            if (CurrentHealth <= halfHealth)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Призыв волка");
                Console.ForegroundColor = ConsoleColor.Green;
                Wolf wolf = new Wolf(_wolfName, _wolfHealth, _wolfDamage);
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
        private int _getSpontaneousEffectChance;
        private int _minFireDamage;
        private int _maxFireDamage;
        private int _minRainHeal;
        private int _maxRainHeal;
        private int _minFloraDamage;
        private int _maxFloraDamage;
        private int _minFloraHeal;
        private int _maxFloraHeal;

        public Shaman(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
            _getSpontaneousEffectChance = 4;
            _minFireDamage = 50;
            _maxFireDamage = 101;
            _minRainHeal = 50;
            _maxRainHeal = 76;
            _minFloraDamage = 25;
            _maxFloraDamage = 101;
            _minFloraHeal = 0;
            _maxFloraHeal = 101;
        }

        public void GetSpontaneousEffect(Fighter enemyFighter)
        {
            Damage = _initialDamage;
            Random random = new Random();
            ConsoleColor defaultColor = Console.ForegroundColor;
            int chance = random.Next(_getSpontaneousEffectChance);
            int windSpell = 0;
            int fireSpell = 1;
            int waterSpell = 2;
            int floraSpell = 3;

            if (chance == windSpell)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Попутный ветер");
                CurrentHealth += enemyFighter.Damage;
            }
            else if (chance == fireSpell)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Жар солнца");
                Damage += random.Next(_minFireDamage, _maxFireDamage);
                CurrentHealth -= Damage;
            }
            else if (chance == waterSpell)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Целительный дождь");
                CurrentHealth += random.Next(_minRainHeal, _maxRainHeal);
            }
            else if (chance == floraSpell)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Защита флоры");
                Damage -= random.Next(_minFloraDamage, _maxFloraDamage);
                CurrentHealth += random.Next(_minFloraHeal, _maxFloraHeal);
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
        private int _beastForms;
        private int _initialDamage;
        private bool _isTurningInto;
        private int _bearFormHealth;
        private int _owlFormHealth;
        private int _foxDamage;
        private int _wolfDamage;

        public Druid(string name, int health, int damage) : base(name, health, damage)
        {
            Random random = new Random();
            _beastForms = random.Next(4);
            _initialDamage = Damage;
            _bearFormHealth = 1000;
            _owlFormHealth = 500;
            _foxDamage = 70;
            _wolfDamage = 20;
        }

        public void TurnIntoBeast(Fighter enemyFighter)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            int healthForTransformations = MaxHealth / 10 * 7;
            int bearForm = 0;
            int owlForm = 1;
            int foxForm = 2;
            int wolfForm = 3;

            if (_isTurningInto == false && CurrentHealth <= healthForTransformations)
            {
                if (_beastForms == bearForm)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Форма медведя");
                    CurrentHealth = _bearFormHealth;
                    _isTurningInto = true;
                }
                else if (_beastForms == owlForm)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Форма совы");
                    CurrentHealth = _owlFormHealth;
                    Damage += enemyFighter.Damage;
                    _isTurningInto = true;
                }
                else if (_beastForms == foxForm)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Форма лисы");
                    Damage = _foxDamage;
                    CurrentHealth += enemyFighter.CurrentHealth;
                    _isTurningInto = true;
                }
                else if (_beastForms == wolfForm)
                {
                    Damage = _initialDamage;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Форма волка");
                    Damage += _wolfDamage;

                    if (CurrentHealth <= MaxHealth / 2)
                    {
                        Damage += _wolfDamage;
                    }
                    else if (CurrentHealth <= MaxHealth / 4)
                    {
                        Damage += _wolfDamage;
                    }
                }
            }

            Console.ForegroundColor = defaultColor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Связь {Name}а с лесом помогает ему принимать различные зверинные формы, что позволяет ему защищать лес от врагов\n");
        }

        public override void UseAbility(Fighter enemyFighter)
        {
            TurnIntoBeast(enemyFighter);
        }
    }

    class Priest : Fighter
    {
        private bool _isRised;
        private int _lowerDamage;
        private int _healthThreshold;

        public Priest(string name, int health, int damage) : base(name, health, damage)
        {
            _isRised = false;
            _lowerDamage = Damage / 2;
            _healthThreshold = 100;
        }

        public void RiseAgain()
        {
            int halfHealth = MaxHealth / 2;

            if (_isRised == false)
            {
                if (CurrentHealth <= _healthThreshold)
                {
                    ConsoleColor defaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    CurrentHealth += halfHealth;
                    Console.WriteLine("Возрождение");
                    _isRised = true;
                    Console.ForegroundColor = defaultColor;
                }
            }

            if (_isRised)
            {
                Damage = _lowerDamage;
            }
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
