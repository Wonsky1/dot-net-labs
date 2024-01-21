using BLL.DTOs;
using BLL.UnitOfWork;
using DAL.Models;

namespace Lab3_10.Utilities;

public class Demo
{
    private static User User { get; set; } = null!;

    public static async Task DemoAsync(IUnitOfWork workModel)
    {
        await UnauthorizedDemo(workModel);
    }

    private static async Task UnauthorizedDemo(IUnitOfWork workModel)
    {
        while (true)
        {
            Console.WriteLine(GetMenu(true));
            Console.Write("Введіть номер пункту меню: ");

            var input = Console.ReadLine();

            if (input == "1")
            {
                await GetAllNews(workModel);
            }
            else if (input == "2")
            {
                await SearchBy(workModel);
            }
            else if (input == "3")
            {
                var res = await Authorize(workModel);

                if (!res)
                    continue;

                await AuthorizedDemo(workModel);
                break;
            }
            else if (input == "4")
            {
                var res = await Register(workModel);

                if (!res)
                    continue;

                await AuthorizedDemo(workModel);
                break;
            }
            else if (input == "5")
            {
                Console.WriteLine("До побачення!");
                break;
            }
            else
            {
                Console.WriteLine("Ви ввели невірний номер пункту меню");
            }
        }
    }

    private static async Task AuthorizedDemo(IUnitOfWork workModel)
    {
        while (true)
        {
            Console.WriteLine(GetMenu(false));
            Console.Write("Введіть номер пункту меню: ");

            var input = Console.ReadLine();

            if (input == "1")
            {
                await GetAllNews(workModel);
            }
            else if (input == "2")
            {
                await SearchBy(workModel);
            }
            else if (input == "3")
            {
                await AddNews(workModel);
            }
            else if (input == "4")
            {
                Console.WriteLine("До побачення!");
                break;
            }
            else
            {
                Console.WriteLine("Ви ввели невірний номер пункту меню");
            }
        }
    }

    private static async Task GetAllNews(IUnitOfWork workModel)
    {
        var news = await workModel.News.GetAllAsync(null, "Author", "Category");

        Console.WriteLine("Новини:\n" + string.Join("\n", news) + "\n");
    }

    private static async Task SearchBy(IUnitOfWork workModel)
    {
        Console.WriteLine("1. Пошук за назвою" +
                          "2. Пошук за датою" +
                          "3. Пошук за автором" +
                          "4. Пошук за темою");

        Console.Write("Введіть номер пункту меню: ");

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
            {
                Console.Write("Введіть назву: ");
                var title = Console.ReadLine() ?? throw new InvalidOperationException();

                var news = await workModel.News
                    .GetAllAsync(n => n.Title.Contains(title), "Author", "Category");

                Console.WriteLine("Новини: " + string.Join(", ", news.ToString()));
                break;
            }
            case "2":
            {
                string stringDate;
                DateTime date;
                do
                {
                    Console.Write("Введіть дату: ");
                    stringDate = Console.ReadLine() ?? throw new InvalidOperationException();
                } while (!DateTime.TryParse(stringDate, out date));

                var news = await workModel.News
                    .GetAllAsync(n => n.PublishedDate.Date == date.Date, "Author", "Category");

                Console.WriteLine("Новини: " + string.Join(", ", news.ToString()));
                break;
            }
            case "3":
            {
                Console.Write("Введіть автора: ");
                var author = Console.ReadLine() ?? throw new InvalidOperationException();

                var news = await workModel.News
                    .GetAllAsync(n => n.Author.Name.Contains(author), "Author", "Category");

                Console.WriteLine("Новини: " + string.Join(", ", news.ToString()));
                break;
            }
            case "4":
            {
                Console.Write("Введіть тему: ");
                var category = Console.ReadLine() ?? throw new InvalidOperationException();

                var news = await workModel.News
                    .GetAllAsync(n => n.Category.Name.Contains(category), "Author", "Category");

                Console.WriteLine("Новини: " + string.Join(", ", news.ToString()));
                break;
            }
            default:
                Console.WriteLine("Ви ввели невірний номер пункту меню");
                break;
        }
    }

    private static async Task<bool> Authorize(IUnitOfWork workModel)
    {
        Console.Write("Введіть логін: ");
        var login = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть пароль: ");
        var password = Console.ReadLine() ?? throw new InvalidOperationException();

        var res = await workModel.UserAuthRepository
            .ValidateUser(new LoginDto { Email = login, Password = password });

        if (res is not null)
        {
            User = res;
            return true;
        }

        Console.WriteLine("Ви ввели невірний логін або пароль");
        return false;
    }

    private static async Task<bool> Register(IUnitOfWork workModel)
    {
        Console.Write("Введіть логін: ");
        var login = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть пароль: ");
        var password = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть ім'я: ");
        var name = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть прізвище: ");
        var surname = Console.ReadLine() ?? throw new InvalidOperationException();

        var res = await workModel.UserAuthRepository
            .RegisterUserAsync(new RegisterDto
            {
                Email = login, Password = password, FirstName = name, LastName = surname
            });

        if (res.Succeeded)
        {
            User = (await workModel.UserAuthRepository
                .ValidateUser(new LoginDto { Email = login, Password = password }))!;
            return true;
        }

        Console.WriteLine(string.Join("\n", res.Errors.Select(e => e.Description)));
        return false;
    }

    private static async Task AddNews(IUnitOfWork workModel)
    {
        Console.Write("Введіть назву: ");
        var title = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть текст: ");
        var content = Console.ReadLine() ?? throw new InvalidOperationException();

        string stringDate;
        DateTime date;
        do
        {
            Console.Write("Введіть дату: ");
            stringDate = Console.ReadLine() ?? throw new InvalidOperationException();
        } while (!DateTime.TryParse(stringDate, out date));

        Console.Write("Введіть тему: ");
        var category = Console.ReadLine() ?? throw new InvalidOperationException();

        Console.Write("Введіть автора: ");
        var author = Console.ReadLine() ?? throw new InvalidOperationException();

        var news = new News
        {
            Title = title,
            Content = content,
            PublishedDate = date,
            Category = new Category { Name = category },
            Author = new Author { Name = author },
            User = User
        };

        await workModel.News.AddAsync(news);
        await workModel.SaveChangesAsync();

        Console.WriteLine("Новина успішно додана!");
    }

    private static string GetMenu(bool unauthorized) =>
        unauthorized
            ? "Вітаємо на порталі!\n" +
              "1. Переглянути новини\n" +
              "2. Пошук новин\n" +
              "3. Авторизуватись\n" +
              "4. Зареєструватись\n" +
              "5. Вийти\n"
            : "Вітаємо на порталі!\n" +
              "1. Переглянути новини\n" +
              "2. Пошук новин\n" +
              "3. Додати новину\n" +
              "4. Вийти\n";
}
