using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FilmRentalSystem.Data;
using FilmRentalSystem.Models;

namespace FilmRentalSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Repository _repository;

        public MainViewModel()
        {
            _repository = new Repository();

            // Команды
            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddCinemaCommand = new RelayCommand(_ => AddCinema());
            EditCinemaCommand = new RelayCommand<Cinema>(EditCinema);
            DeleteCinemaCommand = new RelayCommand<Cinema>(async c => await DeleteCinemaAsync(c));

            AddMovieCommand = new RelayCommand(_ => AddMovie());
            EditMovieCommand = new RelayCommand<Movie>(EditMovie);
            DeleteMovieCommand = new RelayCommand<Movie>(async m => await DeleteMovieAsync(m));

            AddRentalCommand = new RelayCommand(_ => AddRental());
            ReturnRentalCommand = new RelayCommand<Rental>(async r => await ReturnRentalAsync(r));

            // Коллекции
            Cinemas = new ObservableCollection<Cinema>();
            Movies = new ObservableCollection<Movie>();
            Suppliers = new ObservableCollection<Supplier>();
            Rentals = new ObservableCollection<Rental>();
            ActiveRentals = new ObservableCollection<Rental>();
            OverdueRentals = new ObservableCollection<Rental>();

            MovieCategories = new ObservableCollection<string>
            {
                "Боевик", "Триллер", "Комедия", "Драма", "Мелодрама",
                "Фантастика", "Фэнтези", "Ужасы", "Приключения", "Детектив"
            };

            // Загрузка данных
            LoadDataCommand.Execute(null);
        }

        // Коллекции
        public ObservableCollection<Cinema> Cinemas { get; }
        public ObservableCollection<Movie> Movies { get; }
        public ObservableCollection<Supplier> Suppliers { get; }
        public ObservableCollection<Rental> Rentals { get; }
        public ObservableCollection<Rental> ActiveRentals { get; }
        public ObservableCollection<Rental> OverdueRentals { get; }
        public ObservableCollection<string> MovieCategories { get; }

        // Выбранные элементы
        private Cinema _selectedCinema;
        public Cinema SelectedCinema
        {
            get => _selectedCinema;
            set => SetProperty(ref _selectedCinema, value);
        }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set => SetProperty(ref _selectedMovie, value);
        }

        private Rental _selectedRental;
        public Rental SelectedRental
        {
            get => _selectedRental;
            set => SetProperty(ref _selectedRental, value);
        }

        // Команды
        public RelayCommand LoadDataCommand { get; }
        public RelayCommand AddCinemaCommand { get; }
        public RelayCommand<Cinema> EditCinemaCommand { get; }
        public RelayCommand<Cinema> DeleteCinemaCommand { get; }
        public RelayCommand AddMovieCommand { get; }
        public RelayCommand<Movie> EditMovieCommand { get; }
        public RelayCommand<Movie> DeleteMovieCommand { get; }
        public RelayCommand AddRentalCommand { get; }
        public RelayCommand<Rental> ReturnRentalCommand { get; }

        // Статистика
        private decimal _totalIncome;
        public decimal TotalIncome
        {
            get => _totalIncome;
            private set => SetProperty(ref _totalIncome, value);
        }

        private int _totalMovies;
        public int TotalMovies
        {
            get => _totalMovies;
            private set => SetProperty(ref _totalMovies, value);
        }

        private int _totalRentals;
        public int TotalRentals
        {
            get => _totalRentals;
            private set => SetProperty(ref _totalRentals, value);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;

                var cinemas = await _repository.GetAllCinemasAsync();
                var movies = await _repository.GetAllMoviesAsync();
                var rentals = await _repository.GetAllRentalsAsync();
                var activeRentals = await _repository.GetActiveRentalsAsync();
                var overdueRentals = await _repository.GetOverdueRentalsAsync();
                var suppliers = await _repository.GetAllSuppliersAsync();

                Cinemas.Clear(); Movies.Clear(); Suppliers.Clear();
                Rentals.Clear(); ActiveRentals.Clear(); OverdueRentals.Clear();

                foreach (var item in cinemas) Cinemas.Add(item);
                foreach (var item in movies) Movies.Add(item);
                foreach (var item in suppliers) Suppliers.Add(item);
                foreach (var item in rentals) Rentals.Add(item);
                foreach (var item in activeRentals) ActiveRentals.Add(item);
                foreach (var item in overdueRentals) OverdueRentals.Add(item);

                TotalIncome = await _repository.GetTotalRentalIncomeAsync();
                TotalMovies = movies.Count;
                TotalRentals = rentals.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddCinema()
        {
            try
            {
                var newCinema = new Cinema
                {
                    Name = "Новый кинотеатр",
                    Address = "Адрес",
                    Phone = "+79999999999",
                    SeatCount = 100,
                    Director = "Директор",
                    Owner = "Владелец",
                    Bank = "Банк",
                    AccountNumber = "1234567890",
                    INN = "1234567890"
                };

                _repository.AddCinemaAsync(newCinema).Wait();
                LoadDataCommand.Execute(null);
                MessageBox.Show("Кинотеатр добавлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditCinema(Cinema cinema)
        {
            if (cinema == null)
            {
                MessageBox.Show("Выберите кинотеатр", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Редактирование: {cinema.Name}", "Редактирование");
        }

        private async Task DeleteCinemaAsync(Cinema cinema)
        {
            if (cinema == null) return;

            if (MessageBox.Show($"Удалить '{cinema.Name}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.DeleteCinemaAsync(cinema.Id);
                    LoadDataCommand.Execute(null);
                    MessageBox.Show("Удалено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddMovie()
        {
            if (!Suppliers.Any())
            {
                MessageBox.Show("Сначала добавьте поставщика", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newMovie = new Movie
                {
                    Title = "Новый фильм",
                    Category = "Комедия",
                    Screenwriter = "Автор",
                    Director = "Режиссер",
                    ProductionCompany = "Кинокомпания",
                    ReleaseYear = DateTime.Now.Year,
                    PurchaseCost = 1000,
                    SupplierId = Suppliers.First().Id
                };

                _repository.AddMovieAsync(newMovie).Wait();
                LoadDataCommand.Execute(null);
                MessageBox.Show("Фильм добавлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMovie(Movie movie)
        {
            if (movie == null)
            {
                MessageBox.Show("Выберите фильм", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Редактирование: {movie.Title}", "Редактирование");
        }

        private async Task DeleteMovieAsync(Movie movie)
        {
            if (movie == null) return;

            if (MessageBox.Show($"Удалить '{movie.Title}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.DeleteMovieAsync(movie.Id);
                    LoadDataCommand.Execute(null);
                    MessageBox.Show("Удалено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddRental()
        {
            if (!Movies.Any() || !Cinemas.Any())
            {
                MessageBox.Show("Добавьте фильмы и кинотеатры", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newRental = new Rental
                {
                    MovieId = Movies.First().Id,
                    CinemaId = Cinemas.First().Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    RentalFee = 500,
                    PenaltyFee = 0,
                    IsReturned = false
                };

                _repository.AddRentalAsync(newRental).Wait();
                LoadDataCommand.Execute(null);
                MessageBox.Show("Аренда добавлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ReturnRentalAsync(Rental rental)
        {
            if (rental == null) return;

            if (MessageBox.Show($"Вернуть '{rental.Movie.Title}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.ReturnRentalAsync(rental.Id);
                    LoadDataCommand.Execute(null);
                    MessageBox.Show("Фильм возвращен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
