

namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("create new creature");
            System.Console.WriteLine("enter number\n0-user\n1-playlist");
            switch (int.Parse(System.Console.ReadLine()))
            {
                case 0:
                    {
                        System.Console.WriteLine("enter name");
                        var name = System.Console.ReadLine();
                        System.Console.WriteLine("enter mail");
                        var mail = System.Console.ReadLine();
                        TryAdd(() => Core.User.Create(name, mail));
                    }
                    break;
                case 1:
                    {
                        System.Console.WriteLine("enter name");
                        var name = System.Console.ReadLine();
                        System.Console.WriteLine("enter creatorId");
                        var creatorId = int.Parse(System.Console.ReadLine());
                        TryAdd(() => Core.Playlist.Create(name, creatorId));
                    }
                    break;
                default:
                    break;
            }
        }
        private static void TryAdd(Action fun)
        {
            try
            {
                fun();
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine("Failed:\n"+ex.Message);
            }
        }
    }
}