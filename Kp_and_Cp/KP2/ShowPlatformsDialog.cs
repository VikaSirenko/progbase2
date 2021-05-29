using Terminal.Gui;
using System.Collections.Generic;

public class ShowPlatformsDialog : Dialog
{

    private ListView allPlatformsListView;
    private PlatformRepository platformRepository;
    private ConnectionRepository connectionRepository;
    private Label pathToFile;
    private Label isEmptyListLbl;
    public ShowPlatformsDialog()
    {
        this.Title = "Show Platforms";
        allPlatformsListView = new ListView(new List<Platform>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),

        };
        Button backBtn = new Button(30, 21, "Back");
        backBtn.Clicked += OnShowDialogBack;
        this.Add(backBtn);



        FrameView frameView = new FrameView("Platforms")
        {
            X = 4,
            Y = 2,
            Width = Dim.Fill() - 4,
            Height = 10,
        };

        frameView.Add(allPlatformsListView);
        this.Add(frameView);

        isEmptyListLbl = new Label("There is no platforms.");
        frameView.Add(isEmptyListLbl);
        isEmptyListLbl.Visible = false;

        Button deleteAllBtn = new Button(4, 14, "Delete all platforms");
        deleteAllBtn.Clicked += OnDeleteAllClicked;
        this.Add(deleteAllBtn);

        Button selectedFileBtn = new Button(4, 16, "Select file");
        selectedFileBtn.Clicked += SelectFile;

        pathToFile = new Label("")
        {
            X = Pos.Right(selectedFileBtn) + 2,
            Y = Pos.Top(selectedFileBtn),
            Width = Dim.Fill(),
        };
        this.Add(selectedFileBtn, pathToFile);

        Button exportBtn = new Button(4, 18, "Do export platforms");
        exportBtn.Clicked += OnExportPlatforms;
        this.Add(exportBtn);

    }

    private void OnExportPlatforms()
    {
        List<Platform> platforms = platformRepository.GetAll();
        if (!pathToFile.Text.IsEmpty)
        {
            try
            {
                if (pathToFile.Text.ToString().EndsWith(".xml"))
                {
                    Export.DoExportOfPlatforms(platforms, pathToFile.Text.ToString());
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

    private void OnDeleteAllClicked()
    {
        int index = MessageBox.Query("Removal", "Are you sure?", "NO", "YES");
        if (index == 1)
        {
            platformRepository.DeleteAll();
            connectionRepository.DeleteAll();
            MessageBox.Query("Removal", "All platforms have been deleted from the database", "OK");
            ShowCurrentPage();
        }
    }
    private void OnShowDialogBack()
    {
        Application.RequestStop();
    }

    public void SetData(PlatformRepository platformRepository, ConnectionRepository connectionRepository)
    {
        this.platformRepository = platformRepository;
        this.connectionRepository = connectionRepository;
        ShowCurrentPage();
    }



    private void ShowCurrentPage()
    {

        this.allPlatformsListView.SetSource(platformRepository.GetAll());

        if (platformRepository.GetAll().Count == 0)
        {
            isEmptyListLbl.Visible = true;
        }

        else
        {
            isEmptyListLbl.Visible = false;
        }

    }

}
