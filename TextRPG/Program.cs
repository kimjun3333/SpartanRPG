using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameFunction gameFunction = new GameFunction();
            gameFunction.ItemAdd();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다. \n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            while (true)
            {
                gameFunction.TitleChoice();
            }
        }
    }

    class Player // 
    {
        public int level = 1; //레벨
        public string chad = "류동균(세계최강의사나이)"; //직업
        public float attackpow = 10; //공격력
        public int defencepow = 5; //방어력
        public int health = 100; //체력
        public int gold = 150000; //재화
        public int equipAtk = 0; //장착 공격력
        public int equipDef = 0; // 장착 방어력
    }

    class Item
    {
        public string itemName; // 아이템 이름
        public string itemType; // 아이템 타입
        public int itemPow; // 아이템 능력치
        public string itemDescription; // 아이템 설명
        public int itemPrice; // 아이템 가격
        public bool isEquip; //장착여부
        public bool isSold; // 판매여부

        public Item(string name, string type, int pow, string description, int price, bool equip, bool sold)
        {
            itemName = name;
            itemType = type;
            itemPow = pow;
            itemDescription = description;
            itemPrice = price;
            isEquip = equip;
            isSold = sold;
        }

        public override string ToString()
        {
            string equipText = isEquip ? "[E]" : "";
            return $"{equipText}{itemName} | {itemType} +{itemPow} | {itemDescription}";
        }

        public string ToStoreString()
        {
            if (!isSold)
                return $"{itemName} | {itemType} +{itemPow} | {itemDescription} | {itemPrice}G";
            else
                return $"{itemName} | {itemType} +{itemPow} | {itemDescription} | 판매 완료";
        }
    }


    class GameFunction : Player
    {
        List<Item> itemList = new List<Item>();
        List<Item> playerItemList = new List<Item>();

        public int dungeonCount = 0; //던전 카운트
        public void TitleChoice()
        {
            //Console.Clear();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            int input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    ShowPlayerIndex();
                    break;

                case 2:
                    PlayerInventory();
                    break;

                case 3:
                    Store();
                    break;

                case 4:
                    EnterDungeonSwitch();
                    break;

                case 5:
                    Rest();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        public void ShowPlayerIndex() // 상태보기
        {
            Console.WriteLine("현재 캐릭터의 상태");
            Console.WriteLine($"Chad : {chad}");

            if (equipAtk == 0)
            {
                Console.WriteLine($"공격력 : {attackpow}");
            }
            else
            {
                Console.WriteLine($"공격력 : {attackpow} + ({equipAtk})");
            }

            if (equipDef == 0)
            {
                Console.WriteLine($"방어력 : {defencepow}");
            }
            else
            {
                Console.WriteLine($"방어력 : {defencepow} + ({equipDef})");
            }


            Console.WriteLine($"체 력 : {health}");
            Console.WriteLine($"Gold : {gold}");
            Console.WriteLine("\n0. 나가기\n원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "0")
                Console.Clear();
            TitleChoice();

        }

        public void PlayerInventory()
        {
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < playerItemList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. " + playerItemList[i]);
            }
            Console.WriteLine("\n1. 장착 관리\n0. 나가기\n원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "1")
            {
                Console.WriteLine("장착하실 혹은 장착해제 하실 아이템의 번호를 선택하여주세요.");
                int playerEquip = int.Parse(Console.ReadLine());

                if (playerEquip > playerItemList.Count || playerEquip <= 0) // 리스트는 [1] 0
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    return;
                }
                if (playerItemList[playerEquip - 1].isEquip == false)
                {
                    if (playerItemList[playerEquip - 1].itemType == "공격력")
                    {
                        playerItemList[playerEquip - 1].isEquip = true;
                        equipAtk += playerItemList[playerEquip - 1].itemPow;
                        attackpow += equipAtk;
                    }
                    else if (playerItemList[playerEquip - 1].itemType == "방어력")
                    {
                        playerItemList[playerEquip - 1].isEquip = true;
                        equipDef += playerItemList[playerEquip - 1].itemPow;
                        defencepow += equipDef;
                    }
                }
                else
                {
                    if (playerItemList[playerEquip - 1].itemType == "공격력")
                    {
                        playerItemList[playerEquip - 1].isEquip = false;
                        attackpow -= equipAtk;
                        equipAtk -= playerItemList[playerEquip - 1].itemPow;

                    }
                    else if (playerItemList[playerEquip - 1].itemType == "방어력")
                    {
                        playerItemList[playerEquip - 1].isEquip = false;
                        defencepow -= equipDef;
                        equipDef -= playerItemList[playerEquip - 1].itemPow;
                    }
                }
            }
            else if (input == "0")
                Console.Clear();
            TitleChoice();
        }

        public void Store()
        {
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine($"[보유 골드]\n{gold} G");
            for (int i = 0; i < itemList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. " + itemList[i].ToStoreString());
            }
            Console.WriteLine("\n1. 아이템 구매\n2. 아이템 판매\n0. 나가기\n원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Console.Clear();
                    TitleChoice();
                    break;

                case "1":
                    Console.WriteLine("어떤 아이템을 구매 하시겠습니까?");
                    int buyNum = int.Parse(Console.ReadLine());
                    if (itemList.Count < buyNum || buyNum <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("그런 번호의 상품은 없습니다.");
                        return;
                    }
                    if (itemList[buyNum - 1].itemPrice > gold)
                    {
                        Console.Clear();
                        Console.WriteLine("돈이 부족합니다.");
                        return;
                    }
                    if (itemList[buyNum - 1].isSold)
                    {
                        Console.Clear();
                        Console.WriteLine("이미 판매된 상품입니다.");
                        return;
                    }

                    itemList[buyNum - 1].isSold = true;
                    gold -= itemList[buyNum - 1].itemPrice;
                    playerItemList.Add(itemList[buyNum - 1]);
                    break;

                case "2":
                    //아이템 판매
                    for(int i = 0; i < playerItemList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {playerItemList[i]}");                    
                    }
                    Console.WriteLine("\n어떤 아이템을 판매하시겠습니까?");
                    int sellItem = int.Parse(Console.ReadLine());
                    if (sellItem >= 1 && sellItem <= playerItemList.Count)
                    {
                        gold += playerItemList[sellItem - 1].itemPrice * 85 / 100;
                        playerItemList[sellItem - 1].isSold = false;
                        playerItemList[sellItem - 1].isEquip = false;
                        Console.WriteLine($"{playerItemList[sellItem - 1].itemPrice * 85 / 100}G를 받았습니다.");
                        playerItemList.RemoveAt(sellItem - 1);   
                    }
                    break;
            }
        }


        public void ItemAdd()
        {
            itemList.Add(new Item("낡은 검", "공격력", 50, "너무나도 낡은 검", 500, false, false));
            itemList.Add(new Item("청동 검", "공격력", 10, "청동으로 만든 검", 1000, false, false));
            itemList.Add(new Item("나무 창", "공격력", 7, "나무로 만든 창", 1500, false, false));
            itemList.Add(new Item("엑스칼리버", "공격력", 100, "명검", 10000, false, false));
            itemList.Add(new Item("낡은갑옷", "방어력", 5, "너무나도 낡은 방어구", 700, false, false));
            itemList.Add(new Item("청동갑옷", "방어력", 10, "청동으로 만든 방어구", 2000, false, false));
            itemList.Add(new Item("다이아갑옷", "방어력", 15, "무엇이든 막는 갑옷", 1000, false, false));
        }

        public void Rest()
        {
            Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. 보유 골드 :{gold}");
            Console.WriteLine("\n1. 휴식하기\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            if (gold < 500)
            {
                Console.Clear();
                Console.WriteLine("Gold가 부족합니다.");
                return;
            }

            Console.Clear();
            gold -= 500;
            health += 100;
            Console.WriteLine("휴식을 완료했습니다.");
            return;
        }
        public void EnterDungeonSwitch()
        {
            Console.WriteLine("던전에 오신것을 환영합니다... 난이도를 선택하여 주세요..");
            Console.WriteLine("1.쉬움, 2.일반, 3.어려움");
            int dungeonChoice = int.Parse(Console.ReadLine());
            switch (dungeonChoice)
            {
                case 1:
                    {
                        EnterDungeon("쉬움", 5, 1000);
                        break;
                    }
                case 2:
                    {
                        EnterDungeon("일반", 11, 1700);
                        break;
                    }
                case 3:
                    {
                        EnterDungeon("어려움", 17, 2500);
                        break;
                    }
            }
        }

        public void LevelUp()
        {
            Console.WriteLine($"레벨업! 현재 레벨 :{level} 공격력이 0.5 방어력이 1 올랐습니다!");
            level++;
            attackpow += 0.5f;
            defencepow += 1;
        }

        public void EnterDungeon(string dungeonLevel, int recomDef, int clearMon)
        {
            // 쉬움던전 권장방어력 5

            Console.WriteLine($"{dungeonLevel} 던전에 온걸 환영한다!");
            Random random = new Random();
            int clearPercent = random.Next(0, 10);
            if (defencepow < recomDef) // 방어력 < 권장방어력
            {
                if (clearPercent < 4)
                {
                    Console.WriteLine(clearPercent);
                    Console.WriteLine("던전에 실패하셨습니다...");
                    health /= 2;
                    Console.WriteLine($"{health} 체력이 절반이나 깎였다!!!!");
                    Console.WriteLine("당신은 빈손으로 마을로 돌아갔다...");
                    return;
                }
            }
            Console.WriteLine(clearPercent);
            int takeDmg = random.Next(20, 36);
            health = health - takeDmg + (defencepow - recomDef);
            Console.WriteLine($"{takeDmg + (defencepow - recomDef)}의 피해를 입었습니다.");
            int Percent = random.Next((int)attackpow, (int)attackpow * 2);
            double Bonus = clearMon * Percent / 100;
            Console.WriteLine($"보너스 배율: {Percent}%");
            gold = (int)Bonus + clearMon;
            Console.WriteLine($"던전에 클리어하셨습니다!!! {(int)Bonus + clearMon}G을 얻었습니다.");
            dungeonCount++;

            switch (dungeonCount)
            {
                case 1:
                    LevelUp();
                    break;

                case 3:
                    LevelUp();
                    break;

                case 7:
                    LevelUp();
                    break;

                case 11:
                    LevelUp();
                    break;
            }
        }        
    }
}
