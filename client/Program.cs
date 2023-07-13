namespace NeaClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            try // I am sure this is terrible code and is definataly not the propper way to stop it from eroring on close, but it works.
            {
                Application.Run(new frmChat());
            }
            catch { }
        }
    }
}