using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

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
                arena.Fight(firstFighter, secondFighter);
                arena.DetermineWinner(firstFighter, secondFighter);
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
                new Warlock("Чернокнижник", 1000, 100),
                new Rogue("Разбойник", 1000, 100),
                new Warrior("Воин", 1000, 100),
                new Paladin("Паладин", 1000, 100),
                new Mage("Маг", 1000, 100),
                new Hunter("Охотник", 1000, 100),
                new Shaman("Шаман", 1000, 100),
                new Druid("Друид", 1000, 100),
                new Priest("Жрец", 1000, 100)
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

        public void Fight(Fighter firstFighter, Fighter secondFighter)
        {
            int deathThreshold = 0;
            Console.CursorVisible = false;
            Console.Clear();

            firstFighter.ShowCurrentHealth();

            Console.WriteLine("\nПротив\n");

            secondFighter.ShowCurrentHealth();

            Console.WriteLine("\nНажмите любую клавишу чтобы начать битву...");
            Console.ReadKey();

            while (firstFighter.CurrentHealth > deathThreshold && secondFighter.CurrentHealth > deathThreshold)
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
        }

        public void DetermineWinner(Fighter firstFighter, Fighter secondFighter)
        {
            Console.Clear();
            Fighter winner = FindSurvivor(firstFighter, secondFighter);

            if (winner != null)
            {
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

            if (_fighters.Count <= lastFighter)
                return false;
            else
                return true;
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
        private int _lifesteal;
        private int _minLifesteal = 10;
        private int _maxLifesteal = 31;
        private string _skillName = "Кража здоровья";

        public Warlock(string name, int health, int damage) : base(name, health, damage) { }

        public void StealLife()
        {
            Random random = new Random();
            _lifesteal = random.Next(_minLifesteal, _maxLifesteal);
            CurrentHealth += _lifesteal;
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(_skillName);
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
        private int _critDamage;
        private int _critDamageChance = 4;
        private int _critDamageMultiplier = 2;
        private int _critDamageValue = 0;
        private string _skillName = "Критический урон";

        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
            _critDamage = Damage * _critDamageMultiplier;
        }

        public void DealCriticalDamage()
        {
            Damage = _initialDamage;
            Random random = new Random();
            int chance = random.Next(_critDamageChance);

            if (chance == _critDamageValue)
            {
                Damage = _critDamage;
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(_skillName);
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
        private int _minArmorValue;
        private int _maxArmorValue;
        private string _skillName;

        public Warrior(string name, int health, int damage) : base(name, health, damage)
        {
            _minArmorValue = 10;
            _maxArmorValue = 31;
            _skillName = "Каменная кожа";
        }

        public void BlockDamage()
        {
            Random random = new Random();
            _armor = random.Next(_minArmorValue, _maxArmorValue);
            CurrentHealth += _armor;
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(_skillName);
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
        private int _healingChance = 2;
        private int _healingValue = 25;
        private int _healingChanceValue = 0;
        private string _skillName = "Священная милость";

        public Paladin(string name, int health, int damage) : base(name, health, damage) { }

        public void HealYourself()
        {
            Random random = new Random();
            int chance = random.Next(_healingChance);

            if (chance == _healingChanceValue)
            {
                CurrentHealth += _healingValue;
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(_skillName);
                Console.ForegroundColor = defaultColor;
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
        private int _initialDamage;
        private int _hitCount = 0;
        private int _periodicDamage = 5;
        private string _skillName = "Мороз по коже";

        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
        }

        public void BurnEnemy()
        {
            Damage = _initialDamage;
            _hitCount++;

            for (int i = 0; i < _hitCount; i++)
                Damage += _periodicDamage;

            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(_skillName);
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
        private string _wolfName = "Волк";
        private int _wolfHealth = 100;
        private int _minWolfDamage = 50;
        private int _maxWolfDamage = 81;
        private int _wolfDamage;
        private int _halfHealth;
        private string _skillName = "Призыв волка";

        public Hunter(string name, int health, int damage) : base(name, health, damage)
        {
            Random random = new Random();
            _wolfDamage = random.Next(_minWolfDamage, _maxWolfDamage);
            _halfHealth = MaxHealth / 2;
            _initialDamage = Damage;
        }

        public void SummonWolf()
        {
            Damage = _initialDamage;

            if (CurrentHealth <= _halfHealth)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(_skillName);
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
        private int _getSpontaneousEffectChance = 4;
        private int _minFireDamage = 50;
        private int _maxFireDamage = 101;
        private int _minRainHeal = 50;
        private int _maxRainHeal = 76;
        private int _minFloraDamage = 25;
        private int _maxFloraDamage = 71;
        private int _minFloraHeal = 0;
        private int _maxFloraHeal = 101;
        private int _windSpellChanceValue = 0;
        private int _fireSpellChanceValue = 1;
        private int _waterSpellChanceValue = 2;
        private int _floraSpellChanceValue = 3;
        private string _windSkillName = "Попутный ветер";
        private string _fireSkillName = "Жар солнца";
        private string _waterSkillName = "Целительный дождь";
        private string _floraSkillName = "Защита флоры";

        public Shaman(string name, int health, int damage) : base(name, health, damage)
        {
            _initialDamage = Damage;
        }

        public void GetSpontaneousEffect(Fighter enemyFighter)
        {
            Damage = _initialDamage;
            Random random = new Random();
            ConsoleColor defaultColor = Console.ForegroundColor;
            int chance = random.Next(_getSpontaneousEffectChance);

            if (chance == _windSpellChanceValue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(_windSkillName);
                CurrentHealth += enemyFighter.Damage;
            }
            else if (chance == _fireSpellChanceValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(_fireSkillName);
                Damage += random.Next(_minFireDamage, _maxFireDamage);
                CurrentHealth -= Damage;
            }
            else if (chance == _waterSpellChanceValue)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(_waterSkillName);
                CurrentHealth += random.Next(_minRainHeal, _maxRainHeal);
            }
            else if (chance == _floraSpellChanceValue)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(_floraSkillName);
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
        private int _beastForm;
        private int _initialDamage;
        private int _maxBeastForms = 4;
        private bool _isTurningInto = false;
        private int _healthForTransformations;
        private int _bearFormHealth = 1000;
        private int _owlFormHealth = 500;
        private int _foxDamage = 70;
        private int _wolfDamage = 20;
        private int _bearForm = 0;
        private int _owlForm = 1;
        private int _foxForm = 2;
        private int _wolfForm = 3;
        private string _bearSkillName = "Форма медведя";
        private string _owlSkillName = "Форма совы";
        private string _foxSkillName = "Форма лисы";
        private string _wolfSkillName = "Форма волка";

        public Druid(string name, int health, int damage) : base(name, health, damage)
        {
            Random random = new Random();
            _beastForm = random.Next(_maxBeastForms);
            _initialDamage = Damage;
            _healthForTransformations = MaxHealth / 10 * 7;
        }

        public void TurnIntoBeast(Fighter enemyFighter)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            if (_isTurningInto == false && CurrentHealth <= _healthForTransformations)
            {
                if (_beastForm == _bearForm)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(_bearSkillName);
                    CurrentHealth = _bearFormHealth;
                    _isTurningInto = true;
                }
                else if (_beastForm == _owlForm)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(_owlSkillName);
                    CurrentHealth = _owlFormHealth;
                    Damage += enemyFighter.Damage;
                    _isTurningInto = true;
                }
                else if (_beastForm == _foxForm)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(_foxSkillName);
                    Damage = _foxDamage;
                    CurrentHealth += enemyFighter.CurrentHealth;
                    _isTurningInto = true;
                }
                else if (_beastForm == _wolfForm)
                {
                    Damage = _initialDamage;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(_wolfSkillName);
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
        private bool _isRised = false;
        private int _healthThreshold = 100;
        private int _halfHealth;
        private int _lowerDamage;
        private string _skillName = "Воскрешение";

        public Priest(string name, int health, int damage) : base(name, health, damage)
        {
            _lowerDamage = Damage / 2;
            _halfHealth = MaxHealth / 2;
        }

        public void RiseAgain()
        {
            if (_isRised == false)
            {
                if (CurrentHealth <= _healthThreshold)
                {
                    CurrentHealth += _halfHealth;
                    _isRised = true;
                    ConsoleColor defaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(_skillName);
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
