        
        
        
        Hrac hrac = new Hrac("Hrac", 100, 10, 30);
        Weapon mec = new Sword("Stinger", 1, 20, hrac);
        Armor armor = new HeavyArmor("OPS", 1, 20, hrac);
        Potion potion = new Potion("Heal Potion", 1, 30, hrac);
        Main();

        void Main()
        {
            try
            {
                
                
                while (hrac.Health > 0)
                {
                    try
                    {
                        hrac.OpenInventory();
                        PickFight();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
                
            }
        }



        void PickFight()
        {
            int chose;
            Console.WriteLine("1-Goblin\n2-Warrior\n3-Dragon");
            bool ok = int.TryParse(Console.ReadLine(), out chose);
            while (!ok)
            {
                Console.WriteLine("You must enter a valid number");
                ok = int.TryParse(Console.ReadLine(), out chose);
            }
            switch (chose)
            {
                case 1:
                    Goblin goblin = new Goblin("Goblin", 45, 5, 15);
                    Fight(hrac, goblin,  potion);
                    break;
                case 2:
                    Warrior warrior = new Warrior("Warrior", 70, 15, 25);
                    Fight(hrac, warrior, potion);
                    break;
                case 3:
                    Dragon dragon = new Dragon("Dragon", 100, 25, 45);
                    Fight(hrac, dragon, potion);
                    break;
                default:
                    Console.WriteLine("INVALID INPUT");
                    break;
            }
        }

        void Fight(Hrac hrac, Enemy enemy, Potion potion)
        {
            int startDefense = hrac.Defense;
            try
            {
                bool confused = false;
                bool ran = false;
                
                while (enemy.Health > 0 && hrac.Health > 0 && !ran)
                {
                    hrac.ShowInfo(hrac);
                    enemy.ShowInfo(enemy);

                    ran = PlayerTurn(hrac, enemy, potion, confused);
                    if (!ran)
                        confused = enemy.Act(hrac);
                    else break;



                }

                if (ran)
                {
                    Console.WriteLine(
                        "You little pussy really ran to your mommy.\nYou should have fought to the bitter end.");
                }

                else if (hrac.Health > 0 && enemy.Health <= 0)
                {
                    Console.WriteLine("You have won, you are now mighty warrior");
                    hrac.AddItem(potion);
                }
                else if (hrac.Health <= 0 && enemy.Health <= 0)
                    Console.WriteLine("You have managed a draw with the enemy.");
                else
                {
                    Console.WriteLine("You have lost.....\n...pussy");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                hrac.Defense = startDefense;
            }
        }

        

                                                                                                                                   
        bool PlayerTurn(Hrac hrac, Enemy enemy, Potion potion,bool confused)
        {
            if (confused)
                hrac.Attack -= 30;
            
            int chose;
            Console.WriteLine("1-Attack\n2-Use Potion\n3-Counter attack\n4-Run");
            bool ok = int.TryParse(Console.ReadLine(), out chose) && chose >=1 && chose <= 4;
            while (!ok)
            {
                Console.WriteLine("You have to enter a valid number: ");
                ok = int.TryParse(Console.ReadLine(), out chose)  && chose >=1 && chose <= 4;
            }

            bool result = false;
                
            switch (chose)
            {
                case 1:
                    Console.Clear();
                    hrac.DamageTo(enemy);
                    result = false;
                    break;
                case 2:
                    Console.Clear();
                    hrac.UseItem(potion);
                    result = false;
                    break;
                    
                case 3:
                    Console.Clear();
                    hrac.Counter(enemy); 
                    result = false;
                    break;
                    
                case 4:
                    Console.Clear();
                    Console.WriteLine("You are running home to your mommy");
                    //neeed some wrok still
                    result = true;
                    break;
            }
            


            if (confused)
                hrac.Attack += 30;
            return result;



        }




        // ----------------class----------------------------

        abstract class Item
        {
            public string Name { get;set; }

            
            

            protected int count;
            public virtual int Count
            {
                get { return count;}
                protected set
                {
                    if (value <= 0)
                    {
                        count = 0;
                    }
                    else if (value > 1)
                    {
                        count = 1;
                    }
                    else
                    {
                        count = value;
                    }
                    
                }
            }

            public abstract void Use();
            

            public void ShowInfo()
            {
                Console.WriteLine($"{Name} : {Count}");
            }

            protected Hrac hrac;

            public Item(string name, int count, Hrac hrac)
            {
                Name = name;
                Count = count;
                this.hrac = hrac;
                
                if(hrac.inventory.Count < 10)
                    hrac.AddItem(this);
                else
                {
                    throw new FullInventory("Your inventory is full");
                }
            }
        }



        abstract class Postava
        {
            private int health;

            public int Health
            {
                get{return health;}
                private set
                {
                    if (value < 0)
                        health = 0;
                    else if (value > 100)
                        health = 100;
                    else
                    {
                        health = value;
                    }
                }
            }

            public void ShowInfo(Postava postava)
            {
                Console.WriteLine("-------------------");
                Console.WriteLine($"{Name} | Health : {Health}\nAttack: {Attack} | Defense: {Defense}");
            }

            protected int attack;

            public int Attack
            {
                get
                {
                    return attack;
                }
                set
                {
                    if (value < 0)
                        attack = 0;
                    else
                    {
                        attack = value;
                    }
                }
            }

            private int defense;

            public int Defense
            {
                get { return defense;}
                set
                {
                    if (value < 0)
                        defense = 0;
                    else
                    {
                        defense = value;
                    }
                }
            }

            public string Name { get; set; }



            public void IgnoreDefense(Postava postava)
            {
                postava.Health -= Attack;
            }
            public void DamageTo(Postava postava)
            {
                int damage = Math.Max(0, Attack - postava.Defense);
                postava.Health -= damage;
                Console.WriteLine($"{Name} damaged {postava.Name} for {damage}.");
            }

            public void AddHealth(int amount)
            {
                Health += amount;
                
            }

            public void Counter(Postava postava)
            {
                int halfit = postava.Attack / 2;
                Health -= halfit;
                postava.Health -= halfit;
                Console.WriteLine($"Health - {halfit}. You countered the attack for {halfit}dp");
            }

            public Postava(string name, int health, int defense, int attack)
            {
                Name = name;
                Health = health;
                Defense = defense;
                Attack = attack;
                
            }

            public void UseItem(Item item)
            {
                item.Use();
            }
            
            
        }

        class Hrac : Postava
        {
            public List<Item> inventory = new List<Item>(10);





            public void OpenInventory()
            {
                
                int chose;
                do
                {
                    Console.WriteLine("------------------Inventory----------------");
                    Console.WriteLine("1-Use Item\n2-Drop Item\n3-Inspect Item\n4-Show Inventory\n5-back");
                    bool ok = int.TryParse(Console.ReadLine(), out chose);
                    while (!ok)
                    {
                        Console.WriteLine("You have to enter a number: ");
                        ok = int.TryParse(Console.ReadLine(), out chose);
                    }

                    switch (chose)
                    {
                        case 1:
                            Console.Clear();
                            UseItemInLobby();
                            break;
                        case 2:
                            Console.Clear();
                            DropItem();
                            break;
                        case 3:
                            Console.Clear();
                            FindInInventory().ShowInfo();
                            break;
                        case 4:
                            Console.Clear();
                            SeeInvent();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("Going back");
                            break;
                        default:
                            Console.WriteLine("INVALID INPUT");
                            break;

                    }
                    
                } while (chose != 5);
            }
            
            
            
            
            public void ClearOutInventory()
            {
                inventory.RemoveAll(item => item.Count <= 0);
            }

            public void SeeInvent()
            {
                foreach (Item i in inventory)
                {
                    i.ShowInfo();
                }
            }

            public void DropItem()
            {
                SeeInvent();
                Item toDrop = FindInInventory();
                Console.Write($"Are you sure you want to drop {toDrop.Name}?\n[y/n] : ");
                string uSure = Console.ReadLine();
                if (uSure == "y")
                {
                    inventory.Remove(toDrop);
                    if ( toDrop is Weapon weapon && weapon.equiped)
                    {
                        Attack -= weapon.Attack;
                        Console.WriteLine($"Your attack went down for {weapon.Attack}dp.\nYou now have {Attack}dp. ");
                    }
                    else if (toDrop is Armor a && a.equiped)
                    {
                        Defense -= a.Defense;
                        Console.WriteLine($"Your defemnse went down for {a.Defense} defense points.\nYou now have {Defense}defense points");
                        
                    }
                    
                    
                }
                    
                else
                {
                    Console.WriteLine($"Okay, {toDrop.Name} is still in inventory");
                }
                    
            }
            
            
            public void AddItem(Item i)
            {
                inventory.Add(i);
            }

            public Item FindInInventory()
            {
                Console.WriteLine("Enter name of what you are looking for: ");
                string lookingFor = Console.ReadLine();
                Item found =  inventory.FirstOrDefault(item => item.Name.ToUpper() == lookingFor.ToUpper());
                
                if (found == null)
                    throw new NotFound("The Item was not found");
                return found;
                
            }
            
            public bool EquipedW = false;
            public bool EquipedA = false;
            
            public void UseItemInLobby()
            {
                SeeInvent();

                FindInInventory().Use();
                
                    
                    
                
                ClearOutInventory();
            }
            
            
            
            
            
            public Hrac(string name,int health, int defense, int attack) : base(name,health,defense,attack){}
        }



        abstract class Enemy : Postava
        {
            abstract public bool Act(Hrac target);
            
            public Enemy(string name, int health,int defense, int attack) : base(name,health,defense,attack){}
        }

        class Goblin : Enemy
        {
            public Goblin(string name, int health, int defense, int attack) : base(name, health, defense, attack){}

            override public bool Act(Hrac target)
            {
                int chance = Random.Shared.Next(1, 5);
                if (chance == 1)
                {
                    DamageTo(target);
                    DamageTo(target);
                    return false;
                }
                else
                {
                    DamageTo(target);
                    return false;
                }
            }
        }

        class Warrior : Enemy
        {

            override public bool Act(Hrac target)
            {
                int chance = Random.Shared.Next(1, 4);
                switch (chance)
                {
                    case 1:
                        DamageTo(target);
                        return false;
                    
                    case 2:
                        target.Defense -= 20;
                        Console.WriteLine($"The {Name} have damaged your armor for 20 points");
                        return false;
                    
                    case 3:
                        Console.WriteLine("You have been confused. You attack for 30 damage less.");
                        return true;
                    default:
                        Console.WriteLine("Chyba v syntaxi random");
                        return false;
                }
            }
            public Warrior(string name, int health, int defense, int attack) : base(name, health, defense, attack){}
        }

        class Dragon : Enemy
        {
            private int turnCounter = 0;

            public Dragon(string name, int health, int defense, int attack) : base(name, health, defense, attack) {}

            override public bool Act(Hrac target)
            {
                turnCounter++;
                if (turnCounter % 3 == 0 && turnCounter != 0)
                {
                    IgnoreDefense(target);
                    Console.WriteLine("Dragons fire catches you of guard");
                    return false;

                }
                else
                {
                    DamageTo(target);
                    return false;
                }
            }
        }


        abstract class Equipment : Item
        {
            abstract public 
            
        }
        abstract class Armor : Item
            {
                public bool equiped;
                protected int defense;

                public virtual int Defense
                {
                    get { return defense; }
                    set
                    {
                        if (value < 0)
                            defense = 0;
                        else
                        {
                            defense = value;
                        }
                    }
                }



                public override void Use()
                {
                    if (!hrac.EquipedA)
                    {
                        hrac.Defense += Defense;
                        Console.WriteLine(
                            $"You have equiped {Name}\nYour defense went up {Defense} to  {hrac.Defense}");
                        equiped = true;
                        hrac.EquipedA = true;
                    }
                    else
                    {
                        foreach (Item i in hrac.inventory)
                        {
                            if (i is Armor a && a.equiped)
                            {
                                hrac.Defense -= a.Defense;
                                Console.WriteLine($"{a.Name} bylo sundano");
                                a.equiped = false;
                                break;
                            }
                        }

                        hrac.Defense += Defense;
                        Console.WriteLine(
                            $"You have equiped {Name}\nYour defense went up {Defense} to  {hrac.Defense}");
                        equiped = true;

                    }

                }

                public Armor(string name, int count, int defense, Hrac hrac) : base(name, count, hrac)
                {
                    Defense = defense;
                }
            }




            abstract class Weapon : Item
            {
                public bool equiped = false;
                protected int attack;

                public virtual int Attack
                {
                    get { return attack; }
                    protected set
                    {
                        if (value < 0)
                            attack = 0;
                        else
                            attack = value;

                    }
                }

                public override void Use()
                {
                    if (!hrac.EquipedW)
                    {
                        hrac.Attack += Attack;
                        Console.WriteLine($"You have equiped {Name}\nYour Attack went up {Attack} to  {hrac.Attack}");
                        equiped = true;
                        hrac.EquipedW = true;
                    }
                    else
                    {
                        Weapon foundWeapon = hrac.inventory.OfType<Weapon>().FirstOrDefault(w => w.equiped);

                        if (foundWeapon == null)
                        {
                            hrac.Attack += Attack;
                            Console.WriteLine($"You have equiped {Name}\nYour Attack went up {Attack} to  {hrac.Attack}");
                            equiped = true;
                        }
                        else
                        {
                            
                            hrac.Attack -= foundWeapon.Attack;
                            Console.WriteLine($"You have equiped {foundWeapon.Name}\nYour Attack went up {Attack} to  {hrac.Attack}");
                            foundWeapon.equiped = false;
                            
                        }
                            
                        
                        // foreach (Item i in hrac.inventory)
                        // {
                        //     if (i is Weapon w && w.equiped)
                        //     {
                        
                        //     }
                        // }
                        //
                        // hrac.Attack += Attack;
                        // Console.WriteLine($"You have equiped {Name}\nYour Attack went up {Attack} to  {hrac.Attack}");
                        // equiped = true;

                    }

                }


                public Weapon(string name, int count, int attack, Hrac hrac) : base(name, count, hrac)
                {
                    Attack = attack;
                }
            }

            class Sword : Weapon
            {



                public override int Count
                {
                    get { return count; }
                    protected set
                    {
                        if (value < 0)
                        {
                            count = 0;

                        }

                        else if (value > 2)
                            count = 2;
                        else
                        {
                            count = value;
                        }
                    }

                }


                public Sword(string name, int count, int attack, Hrac hrac) : base(name, count, attack, hrac)
                {
                }

            }

            class HeavyArmor : Armor
            {





                public HeavyArmor(string name, int count, int defense, Hrac hrac) : base(name, count, defense, hrac)
                {
                }
            }



            class Potion : Item
            {

                public int HealPower { get; set; }

                public Potion(string name, int count, int healPower, Hrac hrac) : base(name, count, hrac)
                {
                    HealPower = healPower;
                }

                public override int Count
                {
                    get { return count; }
                    protected set
                    {
                        if (value < 0)
                            count = 0;
                        else
                            count = value;
                    }
                }

                public override void Use()
                {
                    if (Count > 0)
                    {
                        Count--;
                        hrac.AddHealth(HealPower);
                        Console.WriteLine($"Health restored for {HealPower}. You now have {hrac.Health} hp.");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough potions.");
                    }

                }
            }

            class NotFound : Exception
            {
                public NotFound(string messsage) : base(messsage)
                {
                }
            }

            class FullInventory : Exception
            {
                public FullInventory(string messsage) : base(messsage) {}
            }
        

