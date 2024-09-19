
using System.Text.Json;

// Klassen representerar ett inlägg i gästboken, med två fält: Ägare och Meddelande.
class GuestbookEntry
{
    // Ägaren av inlägget. Fältet får inte vara null, måste sättas vid objektets skapande.
    public string Owner { get; set; }

    // Meddelandet som ägaren skriver. Även detta fält får inte vara null.
    public string Message { get; set; }

    // Konstruktor för att säkerställa att egenskaperna Owner och Message alltid sätts när ett objekt skapas
    public GuestbookEntry(string owner, string message)
    {
        Owner = owner;      // Tilldela ägare
        Message = message;  // Tilldela meddelandet
    }
}

class Program
{
    // Filens sökväg där gästboken kommer att sparas och laddas
    static string filePath = "guestbook.json";

    static void Main()
    {
        // Ladda in gästbokens inlägg från en JSON-fil (om den existerar), annars skapa en tom lista
        List<GuestbookEntry> guestbook = LoadGuestbook();

        while (true) //Loopar tills användaren avslutar programmet
        {
            Console.Clear(); // Rensar konsolen för att ge användaren en tom meny

            // Visa menyn med olika alternativ för användaren
            Console.WriteLine("Välkommen till Gästboken!");
            Console.WriteLine("1. Visa alla inlägg");
            Console.WriteLine("2. Lägg till ett inlägg");
            Console.WriteLine("3. Ta bort ett inlägg");
            Console.WriteLine("4. Avsluta");
            Console.Write("Välj ett alternativ: ");

            string? choice = Console.ReadLine(); // Läs in användarens val

            switch (choice) // Hantera användarens val med en switchsats
            {
                case "1":
                    ShowEntries(guestbook); // Visa alla gästboksinlägg
                    break;
                case "2":
                    AddEntry(guestbook); // Lägga till ett nytt inlägg
                    break;
                case "3":
                    RemoveEntry(guestbook); // Ta bort ett inlägg baserat på index
                    break;
                case "4":
                    SaveGuestbook(guestbook); // Spara gästboken till fil innan programmet avslutas
                    Console.WriteLine("Avslutar programmet...");
                    return; // Avsluta programmet
                default:
                    // Felhantering om användaren väljer ett ogiltigt alternativ
                    Console.WriteLine("Ogiltigt val, försök igen!");
                    break;
            }

            // Efter varje åtgärd, be användaren att trycka på valfri tangent för att fortsätta
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Funktion som laddar in gästbokens inlägg från en JSON-fil
    /// </summary>
    /// <returns>new List<GuestbookEntry></returns>
    static List<GuestbookEntry> LoadGuestbook()
    {
        // Kontrollera om filen existerar
        if (File.Exists(filePath))
        {
            // Om filen finns, läs in innehållet som en sträng
            string json = File.ReadAllText(filePath);

            // Deserialisera JSON-strängen till en lista av GuestbookEntry-objekt
            return JsonSerializer.Deserialize<List<GuestbookEntry>>(json) ?? new List<GuestbookEntry>();
        }
        // Om filen inte finns, returnera en tom lista för att börja med en tom gästbok
        return new List<GuestbookEntry>();
    }

    /// <summary>
    /// Funktion som sparar gästbokens inlägg till en JSON-fil
    /// </summary>
    /// <param name="guestbook"></param>
    static void SaveGuestbook(List<GuestbookEntry> guestbook)
    {
        // Serialisera listan med gästboksinlägg till en indenterad JSON-sträng (för läsbarhet)
        var options = new JsonSerializerOptions { WriteIndented = true }; // Gör JSON lättläst
        string json = JsonSerializer.Serialize(guestbook, options);

        // Skriv JSON-strängen till filen och spara den på disk
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Funktion för att visa alla inlägg i gästboken
    /// </summary>
    /// <param name="guestbook"></param>
    static void ShowEntries(List<GuestbookEntry> guestbook)
    {
        Console.Clear(); // Rensa konsolen innan inläggen skrivs ut

        // Om gästboken är tom, informera användaren
        if (guestbook.Count == 0)
        {
            Console.WriteLine("Gästboken är tom.");
        }
        else
        {
            // Loopa igenom alla inlägg och skriv ut dem med deras index, ägare och meddelande
            for (int i = 0; i < guestbook.Count; i++)
            {
                Console.WriteLine($"[{i}] Ägare: {guestbook[i].Owner}, Inlägg: {guestbook[i].Message}");
            }
        }
    }

    /// <summary>
    /// Funktion för att lägga till ett nytt inlägg i gästboken
    /// </summary>
    /// <param name="guestbook"></param>
    static void AddEntry(List<GuestbookEntry> guestbook)
    {
        Console.Clear(); // Rensa konsolen innan användaren matar in ett nytt inlägg

        // Be användaren mata in ägarens namn och inläggets text
        Console.Write("Ange ägare till inlägget: ");
        string? owner = Console.ReadLine();
        Console.Write("Ange inläggets text: ");
        string? message = Console.ReadLine();

        // Felhantering: kontrollera att både ägaren och meddelandet är ifyllda
        if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(message))
        {
            // Informera användaren om att inga fält får vara tomma
            Console.WriteLine("Fel: Ägare och meddelande får inte vara tomma.");
            return; // Avbryt åtgärden och återgå till huvudmenyn
        }

        // Lägg till det nya inlägget i gästboken
        guestbook.Add(new GuestbookEntry(owner, message));

        // Spara gästboken efter att det nya inlägget har lagts till
        SaveGuestbook(guestbook);

        // Informera användaren att inlägget har lagts till
        Console.WriteLine("Inlägg tillagt!");
    }

    /// <summary>
    /// Funktion för att ta bort ett inlägg från gästboken
    /// </summary>
    /// <param name="guestbook"></param>
    static void RemoveEntry(List<GuestbookEntry> guestbook)
    {
        Console.Clear(); // Rensa konsolen innan indexet tas emot

        // Kontrollera om gästboken är tom
        if (guestbook.Count == 0)
        {
            Console.WriteLine("Gästboken är tom.");
            return; // Avbryt åtgärden om det inte finns några inlägg att ta bort
        }

        // Visa alla inlägg för att hjälpa användaren att välja rätt inlägg att ta bort
        ShowEntries(guestbook);

        // Be användaren att mata in indexet för det inlägg som ska tas bort
        Console.Write("Ange index på det inlägg du vill ta bort: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < guestbook.Count)
        {
            // Om ett giltigt index anges, ta bort inlägget från listan
            guestbook.RemoveAt(index);

            // Spara gästboken efter att inlägget har tagits bort
            SaveGuestbook(guestbook);

            // Informera användaren att inlägget har tagits bort
            Console.WriteLine("Inlägg borttaget!");
        }
        else
        {
            // Felhantering om användaren anger ett ogiltigt index
            Console.WriteLine("Fel: Ogiltigt index.");
        }
    }
}
