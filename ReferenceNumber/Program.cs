﻿using System;

namespace ReferenceNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            // Checks inputted reference number or creates new ones
            char userChoise;
            do
            {
                Console.Clear();
                userChoise = UserInterface();
                switch (userChoise)
                {
                    // Check input
                    case 'C':
                        RefChecker();
                        break;
                    // Create a new one
                    case 'N':
                        RefCreator();
                        break;
                    // Create multiple new ones
                    case 'M':
                        RefMultiCreator();
                        break;
                    // Exit
                    case 'X':
                        break;
                    // In other cases
                    default:
                        Error(1, null);
                        break;
                }
                Console.ReadLine();
            } while (userChoise != 'X');
        }

        static char UserInterface()
        {
            Console.WriteLine("Reference number validator and creator.");
            Console.WriteLine("[C] Validate reference number.");
            Console.WriteLine("[N] Create a new one.");
            Console.WriteLine("[M] Create multiple.");
            Console.WriteLine("[X] Close the program.");
            Console.Write("Choose procedure: ");

            return char.ToUpper(Console.ReadKey().KeyChar);
        }

        static void RefChecker()
        {
            string input = Inputter("", 21, 0, true);
            if (Validator(input, 21, true) == true)
            {
                Console.WriteLine("Reference number is valid.");
            }
            else
            {
                Console.WriteLine("Reference number is invalid.");
            }
        }

        static void RefCreator()
        {
            string basePart = BasePart(1, 20);
            string created = RefCreate(basePart);
            Console.WriteLine($"Created reference number: {created}");
        }

        static void RefMultiCreator()
        {
            int count = HowMany();
            string countNum = count.ToString();
            int max = 20 - countNum.Length;
            string basePart = BasePart(2, max);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    string created = RefCreate(BaseMulti(basePart, i + 1));
                    Vault(created);
                    Console.WriteLine($"Created reference number: {created}");
                }
            }
            else
                Console.WriteLine("Creation was cancelled.");
        }

        static string Inputter(string input, int max, int caller, bool validate)
        {
            bool correctForm = false;
            do
            {
                switch (caller)
                {
                    case 0:
                        Console.Write("\nInput the reference number: ");
                        input = Console.ReadLine();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
                RemoveExtra(ref input);
                if (IsNumbersOnly(input) == true)
                {
                    if (Validator(input, max, validate) == true)
                        correctForm = true;
                }
                else if (input == "X" || input == "x")
                    break;
                else
                    Error(2, "X");
            } while (correctForm == false);
            
            Console.WriteLine(input);
            return input;
        }

        static void RemoveExtra(ref string userInput)
        {
            userInput = userInput.Replace(" ", "");
        }

        static bool IsNumbersOnly(string userInput)
        {
            int failCount = 0;
            for (int i = 0; i < userInput.Length; i++)
            {
                bool tryParse = int.TryParse(userInput[i].ToString(), out _);
                if (tryParse == false)  
                    failCount++;
            }
            if (failCount == 0)
                return true;
            else
                return false;
        }

        static bool Validator(string checkInput, int max, bool validate)
        {
            if (checkInput.Length > 3 && checkInput.Length < max)
            {
                if (validate == false)
                    return true;
                else
                {
                    int i = 0; // ref part is only for creation
                    if (CheckNumber(checkInput, 0, ref i) == true)
                        return true;
                    else
                        Error(5, "X");
                }
            }
            else
                Error(3, null);
            return false;
        }

        static bool CheckNumber(string input, int caller, ref int checkNumber)
        {
            string refNumber = input;
            int[] multiplier = { 7, 3, 1 };
            int correctCheckNumber, product = 1, sum = 0, nearestTenth;
            if (caller == 0)
            {
                refNumber = refNumber.Remove(input.Length - 1);
                checkNumber = int.Parse(input[input.Length - 1].ToString());
            }
            for (int i = refNumber.Length - 1; i >= 0; i--)
            {
                if (product == 1)
                {
                    product = int.Parse(refNumber[i].ToString()) * multiplier[0];
                    sum += product;
                    product = 7;
                }
                else if (product == 7)
                {
                    product = int.Parse(refNumber[i].ToString()) * multiplier[1];
                    sum += product;
                    product = 3;
                }
                else if (product == 3)
                {
                    product = int.Parse(refNumber[i].ToString()) * multiplier[2];
                    sum += product;
                    product = 1;
                }
            }
            nearestTenth = Round(sum);
            correctCheckNumber = nearestTenth - sum;
            if (caller == 0)
            {
                if (checkNumber == correctCheckNumber)
                {
                    return true;
                }
            }
            if (caller == 1 || caller == 2)
                checkNumber = correctCheckNumber;
            return false;
        }

        static int Round(int sum)
        {
            return (int)(Math.Ceiling(sum / 10.0d) * 10);
        }

        static string BasePart(int caller, int max)
        {
            string userInput;
            bool correctForm = false;
            do
            {
                Console.Write("\nInput basepart: ");
                userInput = Console.ReadLine();
                RemoveExtra(ref userInput);
                if (userInput.Length > 3 && userInput.Length < max)
                {
                    if (IsNumbersOnly(userInput) == true)
                    {
                        correctForm = true;
                    }
                    else if (userInput == "X" || userInput == "x")
                        break;
                    else
                        Error(2, null);
                }
                else
                    Error(3, null);
            } while (correctForm == false);

        string basePart = Inputter(userInput, max, caller, false);
            if (caller == 2)
                basePart += 0;
            return basePart;
        }

        static string RefCreate(string basePart)
        {
            int checkNumber = 0;
            CheckNumber(basePart, 1, ref checkNumber);
            basePart += checkNumber;
            return basePart;
        }

        static int HowMany()
        {
            int howMany = 1, max = 999;
            bool okInput = false;
            do
            {
                Console.Write("\nHow many numbers will be created? ");
                string input = Console.ReadLine();
                bool tryParse = int.TryParse(input, out _);
                if (tryParse == true)
                {
                    if (int.Parse(input) < max)
                    {
                        howMany = int.Parse(input);
                        okInput = true;
                    }
                    else
                        Error(4, max.ToString());
                }
                else
                    Error(2, "0");
            } while (okInput == false);
            return howMany;
        }

        static string BaseMulti(string basePart, int i)
        {
            basePart += i;
            return basePart;
        }

        static void Vault(string created)
        {
            
        }

        static void Error(int errorCode, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            switch (errorCode)
            {
                case 1:
                    Console.WriteLine($"\nCheck input. Proceed by pressing Enter.");
                    break;
                case 2:
                    Console.WriteLine($"Input must contain only numbers! Typing {msg} and pressing enter will stop.");
                    break;
                case 3:
                    Console.WriteLine($"Invalid amount of numbers!");
                    break;
                case 4:
                    Console.WriteLine($"Maximum of {msg} reference numbers can be created at a time.");
                    break;
                case 5:
                    Console.WriteLine($"Check number is invalid! Typing {msg} and pressing enter will stop.");
                    break;
                default:
                    Console.WriteLine("\nError occured!");
                    break;
            }
            Console.ResetColor();
        }
    }
}
