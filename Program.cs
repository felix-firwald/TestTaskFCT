using TestTask.Common;

namespace TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Это тестовое задание, выполненное Магон Д. А. для ФЦТ");
            try
            {
                Globals.OnIncomingMessage += Globals_OnIncomingMessage;
                Globals.DoMainWork();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла ошибка!\n" + ex.ToString());
            }
        }

        private static void Globals_OnIncomingMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
