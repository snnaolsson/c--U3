namespace GuestbookApp
{
    // Definierar en post i gästboken
    public class GuestbookEntry
    {
        // Egenskap för ägare av inlägget
        public string Owner { get; set; }

        // Egenskap för själva meddelandet
        public string Message { get; set; }

        // Konstruktor som kräver att ägaren och meddelandet tilldelas ett värde
        public GuestbookEntry(string owner, string message)
        {
            Owner = owner;
            Message = message;
        }
    }
}
