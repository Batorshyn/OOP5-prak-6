using OOP5_prak_6;
using System.Text.Encodings.Web;
using System.Text.Json;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

List<Book> books = [];
const string filePath = "catalog.json";

LoadFromFile();

while (true)
{
    Console.WriteLine("\t Каталог книг ");
    Console.WriteLine("1. Показати всі книги");
    Console.WriteLine("2. Додати книгу");
    Console.WriteLine("3. Знайти книгу за назвою");
    Console.WriteLine("4. Видалити книгу");
    Console.WriteLine("5. Вийти та зберегти");
    Console.Write("Виберіть дію (1-5): ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            ShowAllBooks();
            break;
        case "2":
            AddBook();
            break;
        case "3":
            SearchBook();
            break;
        case "4":
            RemoveBook();
            break;
        case "5":
            SaveToFile();
            return;
        default:
            Console.WriteLine("Невірний вибір, спробуйте ще раз.");
            break;
    }
}

void ShowAllBooks()
{
    if (books.Count == 0)
    {
        Console.WriteLine("Каталог порожній.");
        return;
    }

    Console.WriteLine("Список книг:");
    for (int i = 0; i < books.Count; i++)
    {
        Console.WriteLine($"{i + 1}. \"{books[i].Title}\" - {books[i].Author}");
    }
}

void AddBook()
{
    Console.Write("Введіть назву книги: ");
    string title = Console.ReadLine() ?? "";
    Console.Write("Введіть автора: ");
    string author = Console.ReadLine() ?? "";

    books.Add(new Book { Title = title, Author = author });
    Console.WriteLine("Книгу успішно додано!");
    SaveToFile();
}

void SearchBook()
{
    Console.Write("Введіть частину назви для пошуку: ");
    string query = Console.ReadLine() ?? "";

    var foundBooks = books.Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

    if (foundBooks.Count == 0)
    {
        Console.WriteLine("Нічого не знайдено.");
    }
    else
    {
        Console.WriteLine("Результати пошуку:");
        foreach (var book in foundBooks)
        {
            Console.WriteLine($"- \"{book.Title}\" ({book.Author})");
        }
    }
}

void RemoveBook()
{
    ShowAllBooks();
    if (books.Count == 0) return;

    Console.Write("Введіть номер книги для видалення: ");
    if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= books.Count)
    {
        books.RemoveAt(index - 1);
        Console.WriteLine("Книгу видалено!");
        SaveToFile();
    }
    else
    {
        Console.WriteLine("Невірний номер.");
    }
}

void SaveToFile()
{
    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    string json = JsonSerializer.Serialize(books, options);
    File.WriteAllText(filePath, json);
}

void LoadFromFile()
{
    if (File.Exists(filePath))
    {
        try
        {
            string json = File.ReadAllText(filePath);
            books = JsonSerializer.Deserialize<List<Book>>(json) ?? [];
        }
        catch
        {
            books = [];
        }
    }
}