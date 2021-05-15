using System;
using Microsoft.Data.Sqlite;
using Terminal.Gui;

namespace lab6
{
    class Program
    {


        static void Main(string[] args)
        {
            string databaseFileName = "/home/vika/projects/progbase2/labs/lab6/Lab6/DataLib/taskBD";
            SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
            TaskRepository repository = new TaskRepository(connection);

            Application.Init();
            
            Toplevel top = Application.Top;
            MainWindow window = new MainWindow();
            window.SetRepository(repository);
            top.Add(window);

            Application.Run();

        }



    }
}
