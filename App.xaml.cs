using System.Windows;

namespace FilmRentalSystem
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация БД и тестовых данных
            using (var context = new Data.AppDbContext())
            {
                context.Database.EnsureCreated();
                Data.TestDataInitializer.InitializeTestData(context);
            }
        }
    }
}