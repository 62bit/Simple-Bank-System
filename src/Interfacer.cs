using System;

namespace bank_system
{
    struct Card
    {
        public string card_number;
        public string pin;
    }
    class Interfacer
    {
        private Database db;
        public Interfacer(Database db)
        {
            this.db = db;
        }
        public Card CreateAccount()
        {
            Card card = CreateCard();
            db.InsertCard(card.card_number, card.pin);
            return card;
        }
        public int GetBalance(Card card) { return db.GetBalance(card.card_number); }
        public bool CheckIfExists(Card card) { return db.CardExists(card.card_number); }
        public bool LogIntoAccount(string card_number, string pin)
        {
            if(db.CardExists(card_number))
            {
                if(db.GetPinNumber(card_number) == pin)
                {
                    return true;
                }
            }
            return false;
        }

        public void CloseAccount(Card card) { db.DeleteCard(card.card_number); }
       
        public bool ValidateForLuhn(string numbers)
        {
            int sum = 0;
            bool oddness = true;
            foreach(char n in numbers)
            {
                int number = n - '0';
                if(oddness)
                {
                    number *= 2;
                    if(number > 9)
                        number -= 9;
                    oddness = false;
                }
                else
                    oddness = true;

                sum += number;
            }

            if(sum % 10 == 0)
                return true;

            return false;
        }

        public void AddIncome(Card card, int amount)
        {
            int currentBalance = db.GetBalance(card.card_number);
            int newBalance = currentBalance + amount;

            db.SetBalance(card.card_number, newBalance);
        }

        public bool TransferMoney(Card myCard, string card_number, int amount)
        {
            int myBalance = db.GetBalance(myCard.card_number);
            int otherBalance = db.GetBalance(card_number);
            if(myBalance < amount)
                return false;

            db.SetBalance(myCard.card_number, myBalance - amount);
            db.SetBalance(card_number, otherBalance + amount);

            return true;
        }

        private Card CreateCard()
        {
            Card card = new Card();
            string card_number = GenerateCardNumber();

            while(true)
            {
                if(!db.CardExists(card_number))
                {
                    card.card_number = card_number;
                    break;
                }
                else
                    card_number = GenerateCardNumber();
            }

            card.pin = GeneratePinNumber();

            return card;
        }

        private string GenerateCardNumber()
        {
            string iin_number = "400000";
            string acc_number = "";
            int acc_number_lenght = 9;


            var rng = new Random();
            for(int i = 0; i < acc_number_lenght; ++i)
            {
                int nbr = rng.Next(0, 9);
                acc_number += nbr.ToString();
            }


            acc_number = iin_number + acc_number;

            bool oddness = true;
            int sum = 0;
            foreach(char number in acc_number)
            {
                int iNumber = number - '0';

                if(oddness)
                {
                    iNumber *= 2;
                    if(iNumber > 9)
                        iNumber -= 9;
                    oddness = false;
                }
                else oddness = true;

                sum += iNumber;

            }
            int last_digid = (sum / 10 + 1) * 10 - sum;
            if(last_digid == 10)
            {
                acc_number += '0';
                return acc_number;
            }
            acc_number += last_digid.ToString();

            return acc_number;
        }

        private string GeneratePinNumber()
        {
            string pin = "";
            Random random = new Random();
            for(int i = 0; i < 4; ++i)
            {
                int n = random.Next(0, 9);
                pin += n.ToString();
            }
            return pin;
        }
    }
}