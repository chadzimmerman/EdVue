using System.Text.Json;
using WguMauiMobileApplication.Models;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication.Pages;

public partial class NotesPage : ContentPage
{

    private List<Note> _notes = new();
    private readonly DatabaseService _databaseService = new DatabaseService();


    public NotesPage()
    {
        InitializeComponent();
        _ = LoadNotesAsync();
    }

    public NotesPage(Note note)
    {
        InitializeComponent();
        _notes = new List<Note> { note };
    }

    private async void OnAddNoteClicked(object sender, EventArgs e)
    {
        var note = new Note { Title = "New Note", Body = "" };
        await _databaseService.AddNoteAsync(note);
        _notes.Add(note);

        var noteView = CreateNoteView(note);
        NotesContainer.Children.Add(noteView);
    }

    private View CreateNoteView(Note note)
    {
        var titleLayout = new Grid
        {
            BackgroundColor = Color.FromArgb("#0078D7"),
            Padding = new Thickness(10),
            HeightRequest = 70,
        };
        var titleEntry = new Entry
        {
            Text = note.Title,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
        };
        titleEntry.TextChanged += async (s, e) =>
        {
            note.Title = e.NewTextValue;
            await _databaseService.UpdateNoteAsync(note);
        };
        var deleteButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.White,
            WidthRequest = 40,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
        };
        deleteButton.Clicked += (s, e) =>
        {
            NotesContainer.Children.Remove(titleLayout.Parent as View);
        };
        deleteButton.Clicked += async (s, e) =>
        {
            NotesContainer.Children.Remove(titleLayout.Parent as View);
            await _databaseService.DeleteNoteAsync(note);
        };

        titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        titleLayout.Add(titleEntry);
        titleLayout.Add(deleteButton);
        Grid.SetColumn(titleEntry, 0);
        Grid.SetColumn(deleteButton, 1);

        // Body
        var bodyEntry = new Editor
        {
            Text = note.Body,
            BackgroundColor = Colors.White,
            TextColor = Colors.Black,
            AutoSize = EditorAutoSizeOption.TextChanges,
            HeightRequest = 100,
        };
        bodyEntry.TextChanged += async (s, e) =>
        {
            note.Body = e.NewTextValue;
            await _databaseService.UpdateNoteAsync(note);
        };

        // Share button
        var shareButton = new Button
        {
            Text = "Share",
            HorizontalOptions = LayoutOptions.Center,
        };
        shareButton.Clicked += async (s, e) =>
        {
            var shareText = $"Title: {note.Title}\n\n{note.Body}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareText,
                Title = "Share Note"
            });
        };


        // Compose whole note layout
        var noteLayout = new VerticalStackLayout
        {
            Spacing = 5,
            Children = { titleLayout, bodyEntry, shareButton },
            BackgroundColor = Colors.LightGray,
            Padding = new Thickness(0, 0, 0, 10),
        };
        return noteLayout;
    }

    private async Task LoadNotesAsync()
    {
        var notes = await _databaseService.GetNotesAsync();
        foreach (var note in notes)
        {
            NotesContainer.Children.Add(CreateNoteView(note));
        }
    }

}