using Terminal.Gui;

namespace lab6
{
    public class OpenTaskDialog : Dialog
    {
        public bool deleted;
        public bool updated;
        private Task task;
        private Label taskTopicOutput;
        private TextView taskDescriptionOutput;
        private Label taskMaxScoreOutput;
        private Label taskIspublishedOutput;
        private Label taskPublishedAtOutput;

        public Task GetTask()
        {
            return task;
        }

        public OpenTaskDialog()
        {
            this.Title = "Open task";
            int rightColumn = 25;
            int coordinateX = 2;
            Button backBtn = new Button("Back");
            Button editBtn = new Button("Edit");
            Button deleteBtn = new Button("Delete");
            backBtn.Clicked += OnOpenDialogBack;
            editBtn.Clicked += OnOpenDialogEdit;
            deleteBtn.Clicked += OnOpenDialogDelete;
            this.AddButton(editBtn);
            this.AddButton(deleteBtn);
            this.AddButton(backBtn);

            Label taskTopicLbl = new Label(coordinateX, 2, "Topic:");
            taskTopicOutput = new Label()
            {
                X = rightColumn,
                Y = Pos.Top(taskTopicLbl),
            };
            this.Add(taskTopicLbl, taskTopicOutput);

            Label taskDescriptionLbl = new Label(coordinateX, 4, "Description:");
            taskDescriptionOutput = new TextView()
            {
                X = rightColumn,
                Y = Pos.Top(taskDescriptionLbl),
                Width = 40,
                Height = 5,
                ReadOnly = true,

            };
            this.Add(taskDescriptionLbl, taskDescriptionOutput);

            Label taskMaxScoreLbl = new Label(coordinateX, 11, "Max Score:");
            taskMaxScoreOutput = new Label()
            {
                X = rightColumn,
                Y = Pos.Top(taskMaxScoreLbl),
            };
            this.Add(taskMaxScoreLbl, taskMaxScoreOutput);

            Label taskIsPublishedLbl = new Label(coordinateX, 13, "Is published:");
            taskIspublishedOutput = new Label()
            {
                X = rightColumn,
                Y = Pos.Top(taskIsPublishedLbl),

            };
            this.Add(taskIsPublishedLbl, taskIspublishedOutput);

            Label taskPublishedAtLbl = new Label(coordinateX, 15, "Published at:");
            taskPublishedAtOutput = new Label()
            {
                X = rightColumn,
                Y = Pos.Top(taskPublishedAtLbl),
            };
            this.Add(taskPublishedAtLbl, taskPublishedAtOutput);
        }

        public void SetTask(Task task)
        {
            this.task = task;
            this.taskTopicOutput.Text = task.topic;
            this.taskDescriptionOutput.Text = task.description;
            this.taskMaxScoreOutput.Text = task.maxScore.ToString();
            this.taskIspublishedOutput.Text = task.isPublished.ToString();
            this.taskPublishedAtOutput.Text = task.publishedAt.ToString();
        }

        private void OnOpenDialogBack()
        {
            Application.RequestStop();
        }

        private void OnOpenDialogEdit()
        {
            EditTaskDialog dialog = new EditTaskDialog();
            dialog.SetTask(this.task);
            Application.Run(dialog);
            if (dialog.canceled == false)
            {
                Task updatedTask = dialog.GetTaskFromFields();
                if (updatedTask == null)
                {
                    this.updated = false;
                }
                else
                {
                    updatedTask.publishedAt = task.publishedAt;
                    this.SetTask(updatedTask);
                    this.updated = true;
                }
            }
        }

        private void OnOpenDialogDelete()
        {
            int index = MessageBox.Query("Delete task", "Are you sure?", "No", "Yes");
            if (index == 1)
            {
                Application.RequestStop();
                deleted = true;
            }
        }



    }
}