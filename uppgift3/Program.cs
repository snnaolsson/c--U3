using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GuestbookApp;  // Importera namespace för att använda GuestbookEntry-klassen

namespace GuestbookApp
{
    class Program
    {
        // Filens sökväg där gästboken kommer att sparas och laddas
        static string filePath = "guestbook.json";

        static void Main()
        {
            // Ladda in gästbokens inlägg från en JSON-fil (om den existerar)
            List<GuestbookEntry> guestbook = LoadGuestbook();

            while (true) // Huvudloopen för menyhantering
            {
                Console.Clear(); // Rensa konsolen för att visa menyn tydligt
                Console.WriteLine("Välkommen till Gästboken!");
                Console.WriteLine("1. Visa alla inlägg");
                Console.WriteLine("2. Lägg till ett inlägg");
                Console.WriteLine("3. Ta bort ett inlägg");
                Console.WriteLine("4. Avsluta");
                Console.Write("Välj ett alternativ: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowEntries(guestbook); // Visa alla inlägg
                        break;
                    case "2":
                        AddEntry(guestbook); // Lägg till ett nytt inlägg
                        break;
                    case "3":
                        RemoveEntry(guestbook); // Ta bort ett inlägg
                        break;
                    case "4":
                        SaveGuestbook(guestbook); // Spara och avsluta
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen!");
                        break;
                }

                // Be användaren trycka på en knapp för att fortsätta
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }

        // Funktion som laddar in gästbokens inlägg från en JSON-fil
        static List<GuestbookEntry> LoadGuestbook()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<GuestbookEntry>>(json) ?? new List<GuestbookEntry>();
            }
            return new List<GuestbookEntry>();
        }

        // Funktion som sparar gästbokens inlägg till en JSON-fil
        static void SaveGuestbook(List<GuestbookEntry> guestbook)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(guestbook, options);
            File.WriteAllText(filePath, json);
        }

        // Funktion för att visa alla inlägg i gästboken
        static void ShowEntries(List<GuestbookEntry> guestbook)
        {
            Console.Clear();

            if (guestbook.Count == 0)
            {
                Console.WriteLine("Gästboken är tom.");
            }
            else
            {
                for (int i = 0; i < guestbook.Count; i++)
                {
                    Console.WriteLine($"[{i}] Ägare: {guestbook[i].Owner}, Inlägg: {guestbook[i].Message}");
                }
            }
        }

        // Funktion för att lägga till ett nytt inlägg i gästboken
        static void AddEntry(List<GuestbookEntry> guestbook)
        {
            Console.Clear();
            Console.Write("Ange ägare till inlägget: ");
            string? owner = Console.ReadLine();
            Console.Write("Ange inläggets text: ");
            string? message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Fel: Ägare och meddelande får inte vara tomma.");
                return;
            }

            guestbook.Add(new GuestbookEntry(owner, message));
            SaveGuestbook(guestbook);
            Console.WriteLine("Inlägg tillagt!");
        }

        // Funktion för att ta bort ett inlägg från gästboken
        static void RemoveEntry(List<GuestbookEntry> guestbook)
        {
            Console.Clear();

            if (guestbook.Count == 0)
            {
                Console.WriteLine("Gästboken är tom.");
                return;
            }

            ShowEntries(guestbook);
            Console.Write("Ange index på det inlägg du vill ta bort: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < guestbook.Count)
            {
                guestbook.RemoveAt(index);
                SaveGuestbook(guestbook);
                Console.WriteLine("Inlägg borttaget!");
            }
            else
            {
                Console.WriteLine("Fel: Ogiltigt index.");
            }
        }
    }
}
