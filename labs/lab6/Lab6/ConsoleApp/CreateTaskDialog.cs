using Terminal.Gui;


namespace lab6
{
    public class CreateTaskDialog : Dialog
    {
        public bool canceled;
        protected TextField taskTopicInput;
        protected TextView taskDescriptionInput;
        protected TextField taskMaxScoreInput;
        protected CheckBox taskIsPublishedCheck;

        public CreateTaskDialog()
        {
            this.Title = "Create task";
            Button okBtn = new Button("Ok");
            Button cancelBtn = new Button("Cancel");
            cancelBtn.Clicked += OnCreateDialogCanceled;
            okBtn.Clicked += OnCreateButtonSubmit;
            this.AddButton(cancelBtn);
            this.AddButton(okBtn);
            int rightColumn = 25;
            int coordinateX = 2;

            Label taskTopicLbl = new Label(coordinateX, 2, "Topic:");
            taskTopicInput = new TextField("")
            {
                X = rightColumn,
                Y = Pos.Top(taskTopicLbl),
                Width = 40,

            };
            this.Add(taskTopicLbl, taskTopicInput);

            Label taskDescriptionLbl = new Label(coordinateX, 4, "Description:");
            taskDescriptionInput = new TextView()
            {
                X = rightColumn,
                Y = Pos.Top(taskDescriptionLbl),
                Width = 40,
                Height = 5,

            };
            this.Add(taskDescriptionLbl, taskDescriptionInput);

            Label taskMaxScoreLbl = new Label(coordinateX, 11, "Max Score:");
            taskMaxScoreInput = new TextField()
            {
                X = rightColumn,
                Y = Pos.Top(taskMaxScoreLbl),
                Width = 40,
            };
            this.Add(taskMaxScoreLbl, taskMaxScoreInput);

            Label taskIsPublishedLbl = new Label(coordinateX, 13, "Is published:");
            taskIsPublishedCheck = new CheckBox()
            {
                X = rightColumn,
                Y = Pos.Top(taskIsPublishedLbl),
                Width = 40,

            };
            this.Add(taskIsPublishedLbl, taskIsPublishedCheck);

        }

        private void OnCreateDialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }

        private void OnCreateButtonSubmit()
        {
            this.canceled = false;
            Application.RequestStop();
        }

        public Task GetTaskFromFields()
        {
            Task task = new Task();
            bool isMaxScore = double.TryParse(this.taskMaxScoreInput.Text.ToString(), out task.maxScore);
            if (isMaxScore && this.taskTopicInput.Text.Length != 0 && this.taskDescriptionInput.Text.Length != 0 && task.maxScore >= 0)
            {
                task.topic = this.taskTopicInput.Text.ToString();
                task.description = this.taskDescriptionInput.Text.ToString();
                task.maxScore = double.Parse(this.taskMaxScoreInput.Text.ToString());
                task.isPublished = this.taskIsPublishedCheck.Checked;
                return task;
            }

            return null;
        }
    }
}