using Terminal.Gui;

public class MainWindow : Window
{
    private Label pathToFile;
    private GameRepository gameRepository;
    private ConnectionRepository connectionRepository;
    private PlatformRepository platformRepository;
    public MainWindow()
    {
        this.Title = "Information about games";
        Button importBtn = new Button(4, 6, "Import data");
        importBtn.Clicked += OnImportDataClicked;
        this.Add(importBtn);
        Button selectedFileBtn = new Button(4, 4, "Select file");
        selectedFileBtn.Clicked += SelectFile;

        pathToFile = new Label("")
        {
            X = Pos.Right(selectedFileBtn) + 2,
            Y = Pos.Top(selectedFileBtn),
            Width = Dim.Fill(),
        };
        this.Add(selectedFileBtn, pathToFile);

        Button showGamesBtn = new Button(4, 10, "Show games");
        showGamesBtn.Clicked += OnShowGames;
        this.Add(showGamesBtn);

        Button showPlatformsBtn = new Button(4, 12, "Show platforms");
        showPlatformsBtn.Clicked += OnShowPlatforms;
        this.Add(showPlatformsBtn);


    }

    private void OnShowPlatforms()
    {
        ShowPlatformsDialog dialog = new ShowPlatformsDialog();
        dialog.SetData(platformRepository, connectionRepository);
        Application.Run(dialog);
    }

    private void OnShowGames()
    {
        ShowGamesDialog dialog = new ShowGamesDialog();
        dialog.SetData(gameRepository, connectionRepository);
        Application.Run(dialog);
    }

    private void OnImportDataClicked()
    {
        try
        {
            if (!pathToFile.Text.IsEmpty)
            {
                ImportData.FillDataBase(pathToFile.Text.ToString(), gameRepository, platformRepository, connectionRepository);
                MessageBox.Query("Import", "Data imported", "OK");
            }
            else
            {
                MessageBox.ErrorQuery("ERROR", "The file from which you want to read the data is not specified", "OK");
            }

        }
        catch (System.Exception ex)
        {
            MessageBox.ErrorQuery("ERROR", $"Unable to read data:'{ex.Message.ToString()}'", "OK");
        }
    }

    private void SelectFile()
    {
        OpenDialog dialog = new OpenDialog("Open file", "Open?");
        dialog.DirectoryPath = "./";
        Application.Run(dialog);

        if (!dialog.Canceled)
        {
            NStack.ustring filePath = dialog.FilePath;
            pathToFile.Text = filePath;
        }
        else
        {
            pathToFile.Text = "";
        }
    }

    public void SetData(GameRepository gameRepository, PlatformRepository platformRepository, ConnectionRepository connectionRepository)
    {
        this.gameRepository = gameRepository;
        this.platformRepository = platformRepository;
        this.connectionRepository = connectionRepository;

    }
}
