using Core;
using Data;
using System.Dynamic;
using System.Xml.Linq;

namespace Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Start();
            System.Console.WriteLine("End");
        }
        private static async Task Start()
        {
            while (true)
            {
                System.Console.WriteLine("0-create\n1-Get info\n2-delete\n3-update name\n-1-exit");
                System.Console.WriteLine("");
                switch (int.Parse(System.Console.ReadLine()))
                {
                    case 0:
                        await Create();
                        break;
                    case 1:
                        await Get();
                        break;
                    case 2:
                        await Delete();
                        break;
                    case 3:
                        await UpdateName();
                        break;
                    case -1:
                        return;
                    default:
                        break;
                }
            }
        }
        private static async Task Get()
        {
            System.Console.WriteLine("0-get user info\n1-get playlist info\n-1-back");
            switch (int.Parse(System.Console.ReadLine()))
            {
                case 0:
                    {
                        System.Console.WriteLine("0-get all useres info\n1-get user info by mail\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    await TryGet(
                                        async () =>
                                        {
                                            return await Core.User.GetAll();
                                        },
                                        users =>
                                        {
                                            foreach (var user in (List<Core.User>)users)
                                            {
                                                System.Console.WriteLine($"{user.Number} - {user.Id} {user.Name} {user.Mail}");
                                            }
                                        });
                                }
                                break;
                            case 1:
                                {
                                    System.Console.WriteLine("enter mail");
                                    var mail = System.Console.ReadLine();
                                    var user = await Core.User.GetByMail(mail);
                                    await TryGet(
                                        async () =>
                                        {
                                            return await Core.User.GetByMail(mail);
                                        },
                                        user =>
                                        {
                                            var userUnPacked = (Core.User)user;
                                            System.Console.WriteLine($"id: {userUnPacked.Id}, name: {userUnPacked.Name} mail: {userUnPacked.Mail}");
                                        });
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        System.Console.WriteLine("0-get all users playlist info\n1-get all playlist info\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    await TryGet(
                                            async () =>
                                            {
                                                return await Core.Playlist.GetAllUsers();
                                            },
                                            playlists =>
                                            {
                                                foreach (var platlist in (List<Core.Playlist>)playlists)
                                                {
                                                    System.Console.Write($"{platlist.Number} - (id: {platlist.PlaylistId}, name: {platlist.PlaylistName}); added by (id: {platlist.CreatorId}, {platlist.CreatorName}); liked by ");
                                                    foreach (var user in platlist.Users)
                                                    {
                                                        System.Console.Write(user.Name + ", ");
                                                    }
                                                    System.Console.WriteLine();
                                                }
                                            });
                                }
                                break;
                            case 1:
                                {
                                    await TryGet(
                                            async () =>
                                            {
                                                return await Core.Playlist.GetAll();
                                            },
                                            playlists =>
                                            {
                                                foreach (var platlist in (List<Core.Playlist>)playlists)
                                                {
                                                    System.Console.WriteLine($"{platlist.Number} - (id: {platlist.PlaylistId}, name: {platlist.PlaylistName}); added by (id: {platlist.CreatorId}, {platlist.CreatorName})");

                                                }
                                            });
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }
                    }
                    break;
                case -1:
                    return;
                default:
                    break;
            }
        }
        private static async Task Create()
        {
            System.Console.WriteLine("0-user\n1-playlist");
            switch (int.Parse(System.Console.ReadLine()))
            {
                case 0:
                    {
                        System.Console.WriteLine("enter name");
                        var name = System.Console.ReadLine();
                        System.Console.WriteLine("enter mail");
                        var mail = System.Console.ReadLine();
                        await TryCreate(async () => await Core.User.Create(name, mail));
                    }
                    break;
                case 1:
                    {
                        System.Console.WriteLine("enter name");
                        var name = System.Console.ReadLine();
                        System.Console.WriteLine("enter creatorId");
                        var creatorId = int.Parse(System.Console.ReadLine());
                        await TryCreate(async () => await Core.Playlist.Create(name, creatorId));
                    }
                    break;
                default:
                    break;
            }
        }
        private static async Task Delete()
        {
            System.Console.WriteLine("0-user\n1-playlist\n-1-back");
            switch (int.Parse(System.Console.ReadLine()))
            {
                case 0:
                    {
                        System.Console.WriteLine("0-By mail\n1-By Id\n2-By number\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    System.Console.WriteLine("enter mail");
                                    var mail = System.Console.ReadLine();
                                    await TryDelete(async () => await Core.User.DeleteByMail(mail));
                                }
                                break;
                            case 1:
                                {
                                    System.Console.WriteLine("enter id");
                                    var id = int.Parse(System.Console.ReadLine());
                                    await TryDelete(async () => await Core.User.DeleteById(id));
                                }
                                break;
                            case 2:
                                {
                                    System.Console.WriteLine("enter number");
                                    var number = int.Parse(System.Console.ReadLine());
                                    await TryDelete(async () => await Core.User.DeleteByNumber(number));
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }

                    }
                    break;
                case 1:
                    {
                        System.Console.WriteLine("0-By Id\n1-By number from \"get all playlist info\"\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    System.Console.WriteLine("enter id");
                                    var id = int.Parse(System.Console.ReadLine());
                                    await TryDelete(async () => await Core.Playlist.DeleteById(id));
                                }
                                break;
                            case 1:
                                {
                                    System.Console.WriteLine("enter number");
                                    var number = int.Parse(System.Console.ReadLine());
                                    await TryDelete(async () => await Core.Playlist.DeleteByNumber(number));
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }
                    }
                    break;
                case -1:
                    return;
                default:
                    break;
            }
        }

        private static async Task UpdateName()
        {
            System.Console.WriteLine("0-user\n1-playlist\n-1-back");
            switch (int.Parse(System.Console.ReadLine()))
            {
                case 0:
                    {
                        System.Console.WriteLine("0-By mail\n1-By Id\n2-By number\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    System.Console.WriteLine("enter mail");
                                    var mail = System.Console.ReadLine();
                                    System.Console.WriteLine("enter name");
                                    var name = System.Console.ReadLine();
                                    await TryUpdateName(async () => await Core.User.UpdateNameByMail(name, mail));
                                }
                                break;
                            case 1:
                                {
                                    System.Console.WriteLine("enter id");
                                    var id = int.Parse(System.Console.ReadLine());
                                    System.Console.WriteLine("enter name");
                                    var name = System.Console.ReadLine();
                                    await TryUpdateName(async () => await Core.User.UpdateNameById(name, id));
                                }
                                break;
                            case 2:
                                {
                                    System.Console.WriteLine("enter number");
                                    var number = int.Parse(System.Console.ReadLine());
                                    System.Console.WriteLine("enter name");
                                    var name = System.Console.ReadLine();
                                    await TryUpdateName(async () => await Core.User.UpdateNameByNumber(name, number));
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }

                    }
                    break;
                case 1:
                    {
                        System.Console.WriteLine("0-By Id\n1-By number from \"get all playlist info\"\n-1-back");
                        switch (int.Parse(System.Console.ReadLine()))
                        {
                            case 0:
                                {
                                    System.Console.WriteLine("enter id");
                                    var id = int.Parse(System.Console.ReadLine());
                                    System.Console.WriteLine("enter name");
                                    var name = System.Console.ReadLine();
                                    await TryUpdateName(async () => await Core.Playlist.UpdateNameById(name, id));
                                }
                                break;
                            case 1:
                                {
                                    System.Console.WriteLine("enter number");
                                    var number = int.Parse(System.Console.ReadLine());
                                    System.Console.WriteLine("enter name");
                                    var name = System.Console.ReadLine();
                                    await TryUpdateName(async () => await Core.Playlist.UpdateNameByNumber(name, number));
                                }
                                break;
                            case -1:
                                return;
                            default:
                                break;
                        }
                    }
                    break;
                case -1:
                    return;
                default:
                    break;
            }
        }


        private delegate void PrintDel(object res);
        private static async Task TryGet(Func<Task<object>> fun, PrintDel print)
        {
            object get;
            try
            {
                get = await fun();
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine("Get failed:\n" + ex.Message);
                return;
            }
            System.Console.WriteLine("-------------------");
            print(get);
            System.Console.WriteLine("-------------------");
        }

        //тут 3 одинаковых функции, предполагается открытие разных меню или разные сообщения об ошибке
        private static async Task TryCreate(Func<Task> fun)
        {
            try
            {
                await fun();
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine("Create failed:\n" + ex.Message);
            }
            System.Console.WriteLine("-------------------");
        }
        private static async Task TryDelete(Func<Task> fun)
        {
            try
            {
                await fun();
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine("Delete failed:\n" + ex.Message);
            }
            System.Console.WriteLine("-------------------");
        }
        private static async Task TryUpdateName(Func<Task> fun)
        {
            try
            {
                await fun();
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine("Updete failed:\n" + ex.Message);
            }
            System.Console.WriteLine("-------------------");
        }
    }
}