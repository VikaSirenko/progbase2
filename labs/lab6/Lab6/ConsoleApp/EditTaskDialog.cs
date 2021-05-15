namespace lab6
{
    public class EditTaskDialog : CreateTaskDialog
    {
        public EditTaskDialog()
        {
            this.Title = "Edit task";
        }

        public void SetTask(Task task)
        {
            this.taskTopicInput.Text = task.topic;
            this.taskDescriptionInput.Text = task.description;
            this.taskMaxScoreInput.Text = task.maxScore.ToString();
            this.taskIsPublishedCheck.Checked = task.isPublished;
        }

    }
}