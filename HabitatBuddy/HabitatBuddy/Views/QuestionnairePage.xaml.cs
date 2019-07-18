using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HabitatBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnairePage : ContentPage
    {
        private Models.DecisionTree tree;
        private List<ImageButton> answerButtons;
        private List<Label> answerButtonLabels;
        private bool buttonClicked = false;

        public QuestionnairePage(Models.DecisionTree t)
        {
            InitializeComponent();

            if(t.getAnswerText() != "blank answer")
            {
                Title = t.getAnswerText();
            } else
            {
                Title = "Diagnose An Issue";
            }
            


            tree = t;

            answerButtons = new List<ImageButton>();
            answerButtons.Add(answer_button_1);
            answerButtons.Add(answer_button_2);
            answerButtons.Add(answer_button_3);
            answerButtons.Add(answer_button_4);
            answerButtons.Add(answer_button_5);
            answerButtons.Add(answer_button_6);
            answerButtons.Add(answer_button_7);
            answerButtons.Add(answer_button_8);
            answerButtons.Add(answer_button_9);
            answerButtons.Add(answer_button_10);

            answerButtonLabels = new List<Label>();
            answerButtonLabels.Add(answer_button_1_label);
            answerButtonLabels.Add(answer_button_2_label);
            answerButtonLabels.Add(answer_button_3_label);
            answerButtonLabels.Add(answer_button_4_label);
            answerButtonLabels.Add(answer_button_5_label);
            answerButtonLabels.Add(answer_button_6_label);
            answerButtonLabels.Add(answer_button_7_label);
            answerButtonLabels.Add(answer_button_8_label);
            answerButtonLabels.Add(answer_button_9_label);
            answerButtonLabels.Add(answer_button_10_label);


            reloadButtons();
        }

        protected override void OnDisappearing()
        {
            if (!buttonClicked)
            {
                tree.moveToParent();
            }
            else
            {
                buttonClicked = false;
            }
        }

        protected override void OnAppearing()
        {
            reloadButtons();
        }

        private void reloadButtons()
        {
            int i = 0;
            if (answerButtons.Count() != 0)
            {
                QuestionPrompt.Text = tree.getQuestionText();
                Console.WriteLine("TESTT-> " + tree.getQuestionText() + " -- " + tree.getNumChildren());
                while (i < tree.getNumChildren())
                {
                    tree.moveToChild(i);
                    answerButtonLabels[i].Text = tree.getAnswerText();
                    answerButtons[i].Source = tree.getIcon();
                    tree.moveToParent();
                    i++;
                }
                while (i < answerButtons.Count)
                {
                    answerButtonLabels[i].Text = "";
                    answerButtons[i].IsVisible = false; //Hide any buttons that aren't needed
                    i++;
                }
            }
        }

        private void Answer_1_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(0);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_2_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(1);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_3_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(2);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_4_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(3);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_5_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(4);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_6_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(5);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_7_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(6);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }

        private void Answer_8_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(7);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }
        private void Answer_9_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(8);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }
        private void Answer_10_Button_Clicked(object sender, EventArgs e)
        {
            tree.moveToChild(9);
            if (tree.getActionPlan() is null)
            {
                buttonClicked = true;
                Navigation.PushAsync(new HabitatBuddy.Views.QuestionnairePage(tree));
            }
            else
            {
                Navigation.PushAsync(new HabitatBuddy.Views.ActionPlanPage(tree.getActionPlan())); //Launch the action plan page
            }
        }
    }
}