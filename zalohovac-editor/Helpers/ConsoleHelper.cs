namespace GradeRegisterAdminApp.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteConditionalColor(object? value, bool condition, ConsoleColor color)
        {
            if (condition)
                Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void WriteLineConditionalColor(object? value, bool condition, ConsoleColor color)
        {
            WriteConditionalColor(value, condition, color);
            Console.WriteLine();
        }
    }
}
