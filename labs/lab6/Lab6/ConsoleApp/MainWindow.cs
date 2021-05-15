using Terminal.Gui;
using System.Collections.Generic;

namespace lab6
{
    public class MainWindow : Window
    {
        private ListView allTasksListView;
        private int pageLength = 10;
        private int currentPage = 1;
        private Label currentPageLbl;
        private Label allPagesLbl;
        private Button prevPageButton;
        private Button nextPageButton;
        private TaskRepository repository;
        private Label isEmptyListLbl;


        public MainWindow()
        {
            this.Title = "Task Room";

            allTasksListView = new ListView(new List<Task>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),

            };

            allTasksListView.OpenSelectedItem += OnOpenTask;
            prevPageButton = new Button(28, 14, "<");
            nextPageButton = new Button(44, 14, ">");
            this.currentPageLbl = new Label(36, 14, "?");
            Label slash = new Label(38, 14, "/");
            this.allPagesLbl = new Label(40, 14, "?");

            nextPageButton.Clicked += OnNextButtonClicked;
            prevPageButton.Clicked += OnPrevButtonClicked;

            this.Add(prevPageButton, nextPageButton, currentPageLbl, slash, allPagesLbl);

            FrameView frameView = new FrameView("Tasks")
            {
                X = 4,
                Y = 2,
                Width = Dim.Fill() - 4,
                Height = pageLength + 2,

            };

            frameView.Add(allTasksListView);
            this.Add(frameView);

            isEmptyListLbl = new Label("There is no task.");
            frameView.Add(isEmptyListLbl);
            isEmptyListLbl.Visible = false;

            Button createNewTaskBtn = new Button(58, 20, "Create task");
            createNewTaskBtn.Clicked += OnCreateButtonClicked;
            this.Add(createNewTaskBtn);

            MenuBar menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_New","Ctrl+N", OnNew),
                    new MenuItem("_Quit", "Ctrl+Q", OnQuit)
                }),
                new MenuBarItem("_Help", new MenuItem[]{
                    new MenuItem("_About", "Ctrl+A", OnAbout)
                })
            });

            this.Add(menu);
        }

        private void OnQuit()
        {
            Application.RequestStop();
        }

        private void OnAbout()
        {
            AboutProgramDialog dialog = new AboutProgramDialog();
            Application.Run(dialog);
        }

        private void OnNew()
        {
            OnCreateButtonClicked();
        }


        private void OnNextButtonClicked()
        {
            int totalPages = repository.GetTotalPages(pageLength);
            if (currentPage >= totalPages)
            {
                return;
            }

            this.currentPage++;
            ShowCurrentPage();
        }


        private void OnPrevButtonClicked()
        {
            int totalPages = repository.GetTotalPages(pageLength);
            if (currentPage <= 1)
            {
                return;
            }

            this.currentPage--;
            ShowCurrentPage();
        }


        public void SetRepository(TaskRepository repository)
        {
            this.repository = repository;
            ShowCurrentPage();
        }

        private void ShowCurrentPage()
        {
            this.currentPageLbl.Text = currentPage.ToString();
            int totalPages = repository.GetTotalPages(pageLength);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            this.allPagesLbl.Text = totalPages.ToString();

            this.allTasksListView.SetSource(repository.GetPageTask(currentPage, pageLength));

            prevPageButton.Visible = (currentPage != 1);
            nextPageButton.Visible = (currentPage != int.Parse(this.allPagesLbl.Text.ToString()));

            if (repository.GetAll().Count == 0)
            {
                isEmptyListLbl.Visible = true;
            }

            else
            {
                isEmptyListLbl.Visible = false;
            }
        }

        private void OnCreateButtonClicked()
        {
            CreateTaskDialog dialog = new CreateTaskDialog();
            Application.Run(dialog);

            if (dialog.canceled == false)
            {
                Task task = dialog.GetTaskFromFields();
                if (task == null)
                {
                    MessageBox.ErrorQuery("Create task", "Can not create task.\nAll fields must be filled in the correct format", "OK");
                }

                else
                {
                    task.publishedAt = System.DateTime.Now;
                    long id = repository.Insert(task);
                    task.id = id;
                    DoOpenTask(task);
                    ShowCurrentPage();
                }
            }

        }

        private void OnOpenTask(ListViewItemEventArgs args)
        {
            Task task = (Task)args.Value;
            DoOpenTask(task);
        }


        private void DoOpenTask(Task task)
        {
            OpenTaskDialog dialog = new OpenTaskDialog();
            dialog.SetTask(task);

            Application.Run(dialog);

            if (dialog.deleted == true)
            {
                ProcessDeleteTask(task);

            }

            else if (dialog.updated == true)
            {
                ProcessEditTask(dialog, task);
            }
        }


        private void ProcessDeleteTask(Task task)
        {
            bool isDeleted = repository.Delete(task.id);
            if (isDeleted)
            {
                int countOfPages = repository.GetTotalPages(pageLength);
                if (currentPage > countOfPages && currentPage > 1)
                {
                    currentPage--;
                }
                ShowCurrentPage();
            }

            else
            {
                MessageBox.ErrorQuery("Delete task", "Can not delete task.", "OK");
            }

        }

        private void ProcessEditTask(OpenTaskDialog dialog, Task task)
        {
            Task updatedTask = dialog.GetTask();
            if (repository.Update(updatedTask, task.id))
            {
                ShowCurrentPage();
            }

            else
            {
                MessageBox.ErrorQuery("Edit task", "Can not edit task.", "OK");
            }

        }

    }
}