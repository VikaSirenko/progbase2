using Terminal.Gui;
using System.Collections.Generic;
public class ShowGamesDialog : Dialog
{
    private ListView allGamesListView;
    private Label pathToFile;
    private GameRepository gameRepository;
    private ConnectionRepository connectionRepository;
    private Label isEmptyListLbl;
    public ShowGamesDialog()
    {
        this.Title = "Show Games";
        allGamesListView = new ListView(new List<Game>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),

        };
        Button backBtn = new Button(30, 21, "Back");
        backBtn.Clicked += OnShowDialogBack;
        this.Add(backBtn);


        FrameView frameView = new FrameView("Games")
        {
            X = 4,
            Y = 2,
            Width = Dim.Fill() - 4,
            Height = 10,
        };

        frameView.Add(allGamesListView);
        this.Add(frameView);

        isEmptyListLbl = new Label("There is no games.");
        frameView.Add(isEmptyListLbl);
        isEmptyListLbl.Visible = false;

        Button deleteAllBtn = new Button(4, 14, "Delete all games");
        deleteAllBtn.Clicked += OnDeleteAllClicked;
        this.Add(deleteAllBtn);

        allGamesListView.OpenSelectedItem += OnExportGame;

        Button selectedFileBtn = new Button(4, 16, "Select file");
        selectedFileBtn.Clicked += SelectFile;

        pathToFile = new Label("")
        {
            X = Pos.Right(selectedFileBtn) + 2,
            Y = Pos.Top(selectedFileBtn),
            Width = Dim.Fill(),
        };
        this.Add(selectedFileBtn, pathToFile);


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

    private void OnExportGame(ListViewItemEventArgs args)
    {
        Game game = (Game)args.Value;
        int index = MessageBox.Query("Export game", "Do you really want to export the game?", "NO", "YES");
        if (index == 1)
        {
            if (!pathToFile.Text.IsEmpty)
            {
                try
                {
                    if (pathToFile.Text.ToString().EndsWith(".xml"))
                    {
                        game.platform = gameRepository.GetGamePlatformsById(game);
                        Export.DoExportOfGame(game, pathToFile.Text.ToString());
                        MessageBox.Query("Export", $"Data exported to:'{pathToFile.Text.ToString()}'", "OK");
                    }
                    else
                    {
                        MessageBox.ErrorQuery("ERROR", "The file format is incorrect", "OK");
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.ErrorQuery("ERROR", $"Data cannot be exported:'{ex.Message.ToString()}'", "OK");
                }
            }
            else
            {
                MessageBox.ErrorQuery("ERRROR", "Fields are not filled", "OK");
            }
            
        }

    }


    private void OnDeleteAllClicked()
    {
        int index = MessageBox.Query("Removal", "Are you sure?", "NO", "YES");
        if (index == 1)
        {
            gameRepository.DeleteAll();
            connectionRepository.DeleteAll();
            MessageBox.Query("Removal", "All games have been deleted from the database", "OK");
            ShowCurrentPage();
        }
    }

    private void OnShowDialogBack()
    {
        Application.RequestStop();
    }

    public void SetData(GameRepository gameRepository, ConnectionRepository connectionRepository)
    {
        this.gameRepository = gameRepository;
        this.connectionRepository = connectionRepository;
        ShowCurrentPage();
    }


    private void ShowCurrentPage()
    {

        this.allGamesListView.SetSource(gameRepository.GetAll());

        if (gameRepository.GetAll().Count == 0)
        {
            isEmptyListLbl.Visible = true;
        }

        else
        {
            isEmptyListLbl.Visible = false;
        }

    }

}
