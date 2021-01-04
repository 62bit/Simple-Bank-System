using System;
namespace bank_system
{
    class Program
    {
        enum States
        {
            LOGED_IN,
            LOGGED_OUT,
            EXIT
        }

        static void Main(string[] args)
        {
            Database db = new Database("card.s3db");
            Interfacer inter = new Interfacer(db);

            States state = States.LOGGED_OUT;

            Card currentCard = new Card();

            bool shouldExit = false;
            while(!shouldExit)
            {
                switch(state)
                {
                    case States.LOGGED_OUT:
                        {
                            Console.Write(login_or_create);
                            int input = Convert.ToInt32(Console.ReadLine());
                            if(input == 1)
                            {
                                Card card = inter.CreateAccount();
                                Console.WriteLine("Your card has been created\nYour card number:\n" + card.card_number + "\nYour card PIN:\n" + card.pin + "\n");
                            }
                            else if(input == 2)
                            {
                                Console.Write("\nEnter your card number:\n> ");
                                string number = Console.ReadLine();
                                Console.Write("\nEnter your PIN:\n> ");
                                string pin = Console.ReadLine();
                                if(inter.LogIntoAccount(number, pin))
                                {
                                    state = States.LOGED_IN;
                                    currentCard.card_number = number;
                                    currentCard.pin = pin;
                                    Console.WriteLine("You have successfully logged in!");
                                    break;
                                }
                                Console.WriteLine("Card Number or PIN is wrong!");
                            }
                            else if(input == 0)
                                state = States.EXIT;

                            break;
                        }
                    case States.LOGED_IN:
                        {
                            Console.Write(logged_in);
                            int input = Convert.ToInt32(Console.ReadLine());
                            if(input == 1)
                            {
                                Console.WriteLine("Balance: " + inter.GetBalance(currentCard).ToString());
                                break;
                            }
                            else if(input == 2)
                            {
                                Console.Write("\nEnter income:\n> ");
                                int income = Convert.ToInt32(Console.ReadLine());
                                inter.AddIncome(currentCard, income);
                                Console.WriteLine("Income was added!");
                            }
                            else if(input == 3)
                            {
                                Console.Write("\nTransfer\nEnter card number:\n> ");
                                string number = Console.ReadLine();
                                if(inter.ValidateForLuhn(number))
                                {
                                    Card c = new Card();
                                    c.card_number = number;
                                    if(inter.CheckIfExists(c))
                                    {
                                        Console.Write("\nEnter how much money you want to transfer:\n> ");
                                        int amount = Convert.ToInt32(Console.ReadLine());
                                        if(inter.TransferMoney(currentCard, number, amount))
                                            Console.WriteLine("Success!");
                                        else
                                            Console.WriteLine("Not enough money!");
                                    }
                                    else { Console.WriteLine("Such a card does not exist."); }
                                }
                                else { Console.WriteLine("Probably you made a mistake in the card number. Please try again!"); }
                            }
                            else if(input == 4)
                            {
                                inter.CloseAccount(currentCard);
                                currentCard.card_number = "";
                                currentCard.pin = "";
                                Console.WriteLine("The account has been closed!");
                                state = States.LOGGED_OUT;
                            }
                            else if(input == 5)
                            {
                                currentCard.card_number = "";
                                currentCard.pin = "";
                                Console.WriteLine("Logged Out!");
                                state = States.LOGGED_OUT;
                            }
                            else if(input == 0)
                                state = States.EXIT;

                            break;
                        }

                    case States.EXIT:
                        {
                            shouldExit = true;
                            Console.WriteLine("Bye!");
                            break;
                        }
                    default: break;
                }
            }
        }

        static string login_or_create = "\n1. Create an account\n2. Log into account\n0. Exit\n> ";
        static string logged_in = "\n1. Balance\n2. Add income\n3. Do transfer\n4. Close account\n5. Log out\n0. Exit\n> ";

    }
}